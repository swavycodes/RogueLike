using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    // Algorithm for calculating field of view
    private AdamMilVisibility algorithm;

    // List to store the positions within the actor's field of view
    public List<Vector3Int> FieldOfView = new List<Vector3Int>();

    // Range of the field of view
    public int FieldOfViewRange = 8;

    [Header("Powers")]
    [SerializeField] private int maxHitPoints = 30; // Maximum hit points
    [SerializeField] private int hitPoints = 30; // Current hit points
    [SerializeField] private int defense; // Defense value
    [SerializeField] private int power; // Power value

    // Public properties to access the private fields
    public int MaxHitPoints => maxHitPoints;
    public int HitPoints => hitPoints;
    public int Defense => defense;
    public int Power => power;

    // Called when the script instance is being loaded
    private void Start()
    {
        // Initialize the visibility algorithm
        algorithm = new AdamMilVisibility();

        // Update the field of view based on the current position
        UpdateFieldOfView();

        // If this actor is a player, update the health UI
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }
    }

    // Method to heal the actor by a specified amount of hit points
    public void Heal(int hp)
    {
        int previousHitPoints = hitPoints; // Store the previous hit points
        hitPoints += hp; // Increase the current hit points

        // Ensure hit points do not exceed the maximum
        if (hitPoints > maxHitPoints)
        {
            hitPoints = maxHitPoints;
        }

        int healedAmount = hitPoints - previousHitPoints; // Calculate the healed amount

        // If this actor is the player, update the health UI and show a message
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
            UIManager.Instance.AddMessage($"You have been healed by {healedAmount} hit points.", Color.green);
        }
    }

    // Method to handle the actor's death
    private void Die()
    {
        // If this actor is the player, display a death message
        if (GetComponent<Player>())
        {
            UIManager.Instance.AddMessage("You died!", Color.red);
        }
        else
        {
            UIManager.Instance.AddMessage($"{name} is dead!", Color.green);
            GameManager.Get.RemoveEnemy(this); // Remove the actor from the game manager's enemy list
        }

        // Create a gravestone at the actor's position
        GameObject gravestone = GameManager.Get.CreateActor("Dead", transform.position);
        gravestone.name = $"Remains of {name}";

        // Destroy this actor's game object
        Destroy(gameObject);
    }

    // Method to deal damage to the actor
    public void DoDamage(int hp)
    {
        hitPoints -= hp; // Decrease the current hit points

        // Ensure hit points do not drop below zero
        if (hitPoints < 0)
        {
            hitPoints = 0;
        }

        // If this actor is the player, update the health UI
        if (GetComponent<Player>())
        {
            UIManager.Instance.UpdateHealth(hitPoints, maxHitPoints);
        }

        // If hit points are zero, call Die()
        if (hitPoints == 0)
        {
            Die();
        }
    }

    // Method to move the actor in a specified direction
    public void Move(Vector3 direction)
    {
        // Check if the target position is walkable
        if (MapManager.Get.IsWalkable(transform.position + direction))
        {
            transform.position += direction; // Move the actor
        }
    }

    // Method to update the actor's field of view
    public void UpdateFieldOfView()
    {
        // Get the actor's position in cell coordinates
        var pos = MapManager.Get.FloorMap.WorldToCell(transform.position);

        // Clear the current field of view list
        FieldOfView.Clear();

        // Compute the new field of view
        algorithm.Compute(pos, FieldOfViewRange, FieldOfView);

        // If this actor is the player, update the fog map
        if (GetComponent<Player>())
        {
            MapManager.Get.UpdateFogMap(FieldOfView);
        }
    }
}

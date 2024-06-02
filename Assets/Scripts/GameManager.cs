using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Items; // Import the Items namespace where Consumable class is defined

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public Actor Player { get; set; } // Reference to the player character

    // List to hold enemies in the game
    public List<Actor> Enemies { get; private set; } = new List<Actor>();

    // List to hold consumable items in the game
    public List<Consumable> Items { get; private set; } = new List<Consumable>();

    // Awake is called when the script instance is being loaded
    private void Awake()
    {
        // Ensure only one instance of GameManager exists
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Property to get the singleton instance of GameManager
    public static GameManager Get { get => instance; }

    // Method to get an actor (player or enemy) at a specific location
    public Actor GetActorAtLocation(Vector3 location)
    {
        // Check if the player is at the specified location
        if (Player != null && Player.transform.position == location)
        {
            return Player;
        }

        // Check if any enemy is at the specified location
        foreach (var enemy in Enemies)
        {
            if (enemy != null && enemy.transform.position == location)
            {
                return enemy;
            }
        }
        return null;
    }

    // Method to get a consumable item at a specific location
    public Consumable GetItemAtLocation(Vector3 location)
    {
        foreach (var item in Items)
        {
            if (item != null && item.transform.position == location)
            {
                return item;
            }
        }
        return null;
    }

    // Method to create an actor (player or enemy) at a specific position
    public GameObject CreateActor(string name, Vector2 position)
    {
        GameObject actor = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);

        // If the created actor is the player, assign it to the Player property, otherwise add it as an enemy
        if (name == "Player")
        {
            Player = actor.GetComponent<Actor>();
        }
        else
        {
            AddEnemy(actor.GetComponent<Actor>());
        }

        actor.name = name;
        return actor;
    }

    // Method to create a consumable item at a specific position
    public GameObject CreateItem(string name, Vector2 position)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>($"Prefabs/{name}"), new Vector3(position.x + 0.5f, position.y + 0.5f, 0), Quaternion.identity);
        AddItem(item.GetComponent<Consumable>());
        item.name = name;
        return item;
    }

    // Method to add an enemy to the Enemies list
    public void AddEnemy(Actor enemy)
    {
        Enemies.Add(enemy);
    }

    // Method to add a consumable item to the Items list
    public void AddItem(Consumable item)
    {
        Items.Add(item);
    }

    // Start is called before the first frame update
    private void Start()
    {
        // Assign the Player reference during initialization
        Player = GetComponent<Actor>();
    }

    // Method to start the turn of enemies
    public void StartEnemyTurn()
    {
        // Iterate through all enemies and execute their AI behavior if they have one
        foreach (var enemy in GameManager.Get.Enemies)
        {
            if (enemy != null)
            {
                Enemy enemyComponent = enemy.GetComponent<Enemy>();
                if (enemyComponent != null)
                {
                    enemyComponent.RunAI();
                }
            }
        }
    }

    // Method to remove an enemy from the Enemies list
    public void RemoveEnemy(Actor enemy)
    {
        // Remove the enemy from the list and destroy its GameObject
        Enemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }

    // Method to remove a consumable item from the Items list
    public void RemoveItem(Consumable item)
    {
        // Remove the item from the list and destroy its GameObject
        Items.Remove(item);
        Destroy(item.gameObject);
    }

    // Method to get nearby enemies within a certain distance from a location
    public List<Actor> GetNearbyEnemies(Vector3 location)
    {
        List<Actor> nearbyEnemies = new List<Actor>();

        // Check distance between each enemy and the specified location
        foreach (Actor enemy in Enemies)
        {
            if (enemy != null && Vector3.Distance(enemy.transform.position, location) < 5)
            {
                nearbyEnemies.Add(enemy);
            }
        }

        return nearbyEnemies;
    }
}

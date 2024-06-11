using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorInfo : MonoBehaviour
{
    // References to the UI Text components that display floor and enemy information.
    public Text floorText;
    public Text enemiesText;

    // This method is called when the script instance is being loaded.
    private void Start()
    {
        // Update the UI text to display the current floor number.
        UpdateFloorText();

        // Update the UI text to display the number of remaining enemies.
        UpdateEnemiesText();
    }

    // This method updates the text component with the current floor number.
    public void UpdateFloorText()
    {
        // Check if the MapManager instance is available.
        if (MapManager.Get != null)
        {
            // Set the floorText to display the current floor number.
            floorText.text = "Floor " + MapManager.Get.floor;
        }
    }

    // This method updates the text component with the number of remaining enemies.
    public void UpdateEnemiesText()
    {
        // Check if the GameManager instance is available.
        if (GameManager.Get != null)
        {
            // Get the number of remaining enemies from the GameManager.
            int remainingEnemies = GameManager.Get.Enemies.Count;

            // Set the enemiesText to display the number of remaining enemies.
            enemiesText.text = remainingEnemies + " enemies left";
        }
    }
}

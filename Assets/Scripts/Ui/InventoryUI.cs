using Items;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryUI : MonoBehaviour
{
    // Array to store the labels for inventory items
    public Label[] labels = new Label[8];

    // Root element of the UI document
    private VisualElement root;

    // Index of the currently selected item
    private int selected;

    // Number of items currently in the inventory
    private int numItems;

    // Singleton instance of the InventoryUI
    private static InventoryUI instance;

    // Public static property to access the singleton instance
    public static InventoryUI Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<InventoryUI>();
            }
            return instance;
        }
    }

    // Property to get the index of the selected item
    public int Selected
    {
        get { return selected; }
    }

    // Method called when the script instance is being loaded
    private void Start()
    {
        var UIDocument = GetComponent<UIDocument>(); // Get the UIDocument component
        root = UIDocument.rootVisualElement; // Get the root visual element

        // Initialize the labels array with the UI elements
        for (int i = 0; i < 8; i++)
        {
            labels[i] = root.Q<Label>($"Item{i + 1}");
        }

        Clear(); // Clear the labels
        root.style.display = DisplayStyle.None; // Hide the inventory UI initially
    }

    // Method to clear the inventory UI labels
    public void Clear()
    {
        // Clear the text and background color of each label
        for (int i = 0; i < labels.Length; i++)
        {
            labels[i].text = string.Empty;
            labels[i].style.backgroundColor = new StyleColor(Color.clear);
        }
        selected = 0; // Reset the selected index
        numItems = 0; // Reset the number of items
    }

    // Method to update the background color of the selected label
    public void UpdateSelected()
    {
        // Update the background color of each label based on whether it is selected
        for (int i = 0; i < labels.Length; i++)
        {
            if (i == selected)
            {
                labels[i].style.backgroundColor = new StyleColor(Color.blue);
            }
            else
            {
                labels[i].style.backgroundColor = new StyleColor(Color.clear);
            }
        }
    }

    // Method to select the next item in the inventory
    public void SelectNextItem()
    {
        if (numItems == 0)
        {
            return; // Do nothing if there are no items
        }
        selected = (selected + 1) % numItems; // Increment the selected index
        UpdateSelected(); // Update the UI to reflect the new selection
    }

    // Method to select the previous item in the inventory
    public void SelectPreviousItem()
    {
        if (numItems == 0)
        {
            return; // Do nothing if there are no items
        }
        selected = (selected - 1 + numItems) % numItems; // Decrement the selected index
        UpdateSelected(); // Update the UI to reflect the new selection
    }

    // Method to show the inventory UI with a list of items
    public void Show(List<Consumable> list)
    {
        selected = 0; // Reset the selected index
        numItems = list.Count; // Set the number of items
        Clear(); // Clear the labels

        // Update the labels with the names of the items in the list
        for (int i = 0; i < list.Count && i < labels.Length; i++)
        {
            if (list[i] != null)
            {
                labels[i].text = list[i].name;
            }
        }
        UpdateSelected(); // Update the UI to reflect the new selection
        root.style.display = DisplayStyle.Flex; // Show the inventory UI
    }

    // Method to hide the inventory UI
    public void Hide()
    {
        root.style.display = DisplayStyle.None; // Hide the inventory UI
    }
}

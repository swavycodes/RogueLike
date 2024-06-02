using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Items
{
    public class Inventory
    {
        // Public list to store consumable items
        public List<Consumable> Items = new List<Consumable>();

        // Variable to store the maximum number of items allowed in the inventory
        public int MaxItems;

        // Constructor to initialize the inventory with a specified maximum number of items
        public Inventory(int maxItems)
        {
            MaxItems = maxItems;
        }

        // Function to add an item to the inventory
        public bool AddItem(Consumable item)
        {
            // Check if the current count of items is less than the maximum allowed
            if (Items.Count < MaxItems)
            {
                Items.Add(item); // Add the item to the list
                return true; // Return true if the item was added successfully
            }
            return false; // Return false if the inventory is full
        }

        // Function to remove an item from the inventory
        public void DropItem(Consumable item)
        {
            Items.Remove(item); // Remove the item from the list
        }
    }
}

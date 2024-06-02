using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Items
{
    public class Consumable : MonoBehaviour
    {
        // Enum to represent different types of consumable items
        public enum ItemType
        {
            HealthPotion,
            Fireball,
            ScrollOfConfusion
        }

        // Serialized private field to specify the type of the consumable item
        [SerializeField]
        private ItemType type;

        // Public property to get the type of the consumable item
        public ItemType Type
        {
            get { return type; }
        }

        // Method to check if the item's type matches the provided type
        public bool IsType(ItemType itemType)
        {
            return type == itemType;
        }

        // Method to get the type of the item (same as the property above)
        public ItemType GetItemType()
        {
            return type;
        }

        // Start method is called before the first frame update
        private void Start()
        {
            // Add this item to the GameManager's items list when the game starts
            GameManager.Get.AddItem(this);
        }
    }
}

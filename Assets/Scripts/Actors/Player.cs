using Items;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Actor))]
public class Player : MonoBehaviour, Controls.IPlayerActions
{
    private Controls controls;
    public Inventory inventory;
    private bool inventoryIsOpen = true;
    private bool droppingItem = true;
    private bool usingItem = false;


    private void Awake()
    {
        controls = new Controls();
    }

    private void Start()
    {
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        GameManager.Get.Player = GetComponent<Actor>();

        // Adjust the camera position
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);


    }

    private void OnEnable()
    {
        controls.Player.SetCallbacks(this);
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Player.SetCallbacks(null);
        controls.Disable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        // Controleer of de context-actie is uitgevoerd (bijvoorbeeld een toets is ingedrukt)
        if (context.performed)
        {
            // Controleer of de inventaris momenteel open is
            if (inventoryIsOpen)
            {
                // Lees de bewegingsrichting van de speler
                Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();

                // Als de speler omhoog beweegt (y-waarde > 0), selecteer het vorige item in de inventaris
                if (direction.y > 0)
                {
                    InventoryUI.Instance.SelectPreviousItem();
                }
                // Als de speler omlaag beweegt (y-waarde < 0), selecteer het volgende item in de inventaris
                else if (direction.y < 0)
                {
                    InventoryUI.Instance.SelectNextItem();
                }
            }
            // Als de inventaris niet open is, beweeg de speler normaal
            else
            {
                Move();
            }
        }
    }


   
        public void OnExit(InputAction.CallbackContext context)
        {
            // Controleer of de actie is uitgevoerd (bijvoorbeeld een toets of knop is ingedrukt)
            if (context.performed)
            {
                // Controleer of het inventarisvenster open is
                if (inventoryIsOpen)
                {
                    // Verberg het inventarisvenster
                    UIManager.Instance.inventoryUI.Hide();

                    // Update de status van het inventarisvenster en andere gerelateerde acties
                    inventoryIsOpen = false;
                    droppingItem = false;
                    usingItem = false;
                }
                else
                {
                    // Voer andere exit-acties uit indien nodig
                }
            }
        }

    

    private void Move()
    {
        Vector2 direction = controls.Player.Movement.ReadValue<Vector2>();
        Vector2 roundedDirection = new Vector2(Mathf.Round(direction.x), Mathf.Round(direction.y));
        Debug.Log("roundedDirection");
        Action.Move(GetComponent<Actor>(), roundedDirection);
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
    }
    
        public void OnGrab(InputAction.CallbackContext context)
        {
            // Controleer of de actie is uitgevoerd (bijvoorbeeld een toets of knop is ingedrukt)
            if (context.performed)
            {
                // Zoek naar een consumable item op de locatie van de speler
                Consumable item = GameManager.Get.GetItemAtLocation(transform.position);

                // Als er geen item op deze locatie is
                if (item == null)
                {
                    Debug.Log("No item at this location");
                }
                // Als er wel een item is en het kan worden toegevoegd aan de inventaris
                else if (inventory.AddItem(item))
                {
                    // Schakel het spelobject van het item uit (het wordt verondersteld te zijn opgepakt)
                    item.gameObject.SetActive(false);

                    // Verwijder het item uit het spel
                    GameManager.Get.RemoveItem(item);

                    // Geef een bericht weer dat het item aan de inventaris is toegevoegd
                    Debug.Log("Item added to inventory");
                }
                // Als de inventaris vol is en het item niet kan worden toegevoegd
                else
                {
                    Debug.Log("Inventory is full");
                }
            }
        }

    



    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!inventoryIsOpen)
            {
                // Gebruik de singleton instantie om de Show methode aan te roepen
                InventoryUI.Instance.Show(GameManager.Get.Player.GetComponent<Inventory>().Items);
                inventoryIsOpen = true;
                droppingItem = true;
            }
        }
    }




    
    
        public void OnUse(InputAction.CallbackContext context)
        {
            // Controleer of de context-actie is uitgevoerd (bijvoorbeeld een knop is ingedrukt)
            if (context.performed)
            {
                // Controleer of de inventaris momenteel gesloten is
                if (!inventoryIsOpen)
                {
                    // Haal de inventaris van de speler op en toon deze via de InventoryUI
                    InventoryUI.Instance.Show(GameManager.Get.Player.GetComponent<Inventory>().Items);

                    // Stel de vlaggen in om aan te geven dat de inventaris open is en dat er een item wordt gebruikt
                    inventoryIsOpen = true;
                    usingItem = true;
                }
            }
        }



    // Placeholder voor de UseItem-functie
    private void UseItem(Consumable item)
    {
        Debug.Log($"Using item: {item.name}");
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        // Controleer of de context-actie is uitgevoerd (bijvoorbeeld een toets is ingedrukt)
        if (context.performed)
        {
            // Controleer of de inventaris momenteel open is
            if (inventoryIsOpen)
            {
                // Verkrijg het geselecteerde item uit de inventaris
                Consumable selectedItem = inventory.Items[InventoryUI.Instance.Selected];

                // Verwijder het geselecteerde item uit de inventaris
                inventory.DropItem(selectedItem);

                // Als een item wordt gedropt
                if (droppingItem)
                {
                    // Stel de positie van het item gelijk aan de positie van de speler
                    selectedItem.transform.position = transform.position;

                    // Voeg het item opnieuw toe aan de GameManager
                    GameManager.Get.AddItem(selectedItem);

                    // Maak het gameObject van het item actief
                    selectedItem.gameObject.SetActive(true);
                }
                // Als een item wordt gebruikt
                else if (usingItem)
                {
                    // Voer de (nog lege) functie UseItem uit met dit item als argument
                    UseItem(selectedItem);

                    // Verwijder het item met Destroy
                    Destroy(selectedItem.gameObject);
                }

                // Verberg de GUI
                InventoryUI.Instance.Hide();

                // Zet de inventaris gerelateerde vlaggen terug naar false
                inventoryIsOpen = false;
                droppingItem = false;
                usingItem = false;
            }
        }
    }




}

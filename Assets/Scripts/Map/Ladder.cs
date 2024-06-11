using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    // Bool om te bepalen of de ladder omhoog gaat. Deze kan vanuit de editor worden aangepast.
    [SerializeField]
    private bool up;

    // Eigenschap om toegang te krijgen tot de 'up' variabele
    public bool Up { get => up; set => up = value; }

    // Start wordt één keer aangeroepen voordat de eerste update plaatsvindt
    void Start()
    {
        // Voeg deze ladder toe aan de GameManager
        GameManager.Get.AddLadder(this);
    }

    // Update wordt één keer per frame aangeroepen
    void Update()
    {
        // Momenteel is er geen logica nodig in de update-methode
    }
}

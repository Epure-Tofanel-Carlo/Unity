using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricianOpener : MonoBehaviour
{
    // Reference to the main HUD canvas.
    public Canvas electrician;
    // Reference to the informational button canvas.
    public Canvas buttoninfo;
    // Reference to the heads-up display canvas.
    public Canvas hud;
    // Reference to the wire task logic.
    public WireTask wires;
    // Reference to the props generation logic.
    public GenerateProps props;
    // Flag to check if currently waiting on a timer.
    private bool waiting = false;

    // Initialize the state of the canvases and wire tasks.
    void Start()
    {
        // Initially, the HUD is visible, and other UI elements are not.
        hud.enabled = true;
        buttoninfo.enabled = false;
        electrician.enabled = false;
    }

    // Continuously check for player interactions in a trigger area.
    private void OnTriggerStay2D(Collider2D collision)
    {
        // When the player presses the "E" key within a trigger area.
        if (Input.GetKey(KeyCode.E))
        {
            // Turn off the HUD, display the electrician canvas, and hide the button info.
            hud.enabled = false;
            electrician.enabled = true;
            buttoninfo.enabled = false;
            // Update the wire locations on the left and right.
            foreach (Wire wire in wires.leftWires)
            {
                wire.wireLocation();
            }
            foreach (Wire wire in wires.rightWires)
            {
                wire.wireLocation();
            }
            // Start a timer if not already waiting.
            if (!waiting)
            {
                StartCoroutine(WaitForTimer());
                waiting = true;
            }
        }
    }

    // Co-routine to manage game logic timing and interaction outcomes.
    private IEnumerator WaitForTimer()
    {
        // Start a timer within the wire task.
        wires.StartTimer();
        // Wait until the gaming session ends.
        yield return new WaitUntil(() => !wires.isGaming);

        // If the task is completed successfully.
        if (wires.counter == wires.leftWires.Count)
        {
            // Hide the electrician canvas and disable the object's collider.
            electrician.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            // Log a successful outcome.
            props.addSucces();
        }
        else
        {
            // If the task failed, hide the electrician canvas and disable the object's collider.
            electrician.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            // Show a failure indicator.
            gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
            // Log a failure outcome.
            props.addFail();
        }
    }

    // Enable the button info when an object enters the trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        buttoninfo.enabled = true;
    }

    // Reset the HUD and button info state when an object exits the trigger.
    private void OnTriggerExit2D(Collider2D collision)
    {
        hud.enabled = true;
        buttoninfo.enabled = false;
    }
}

using GamePlay;  // Custom namespace, possibly for project-specific types and logic.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireTask : MonoBehaviour
{
    public Text timer; // UI element to display the countdown timer.
    public List<Color> wireColors = new List<Color>(); // List of colors available for the wires.
    public List<Wire> leftWires = new List<Wire>(); // List of wire objects on the left side.
    public List<Wire> rightWires = new List<Wire>(); // List of wire objects on the right side.

    private List<Color> availableColors; // List to keep track of colors that are still available for assignment.
    private List<int> availableLeftWireIndex; // Indices of unassigned left wires.
    private List<int> availableRightWireIndex; // Indices of unassigned right wires.
    public int counter = -1; // Counter for successful connections.
    public Wire currentDraggedWire; // Currently dragged wire.
    public Wire currentHoveredWire; // Currently hovered wire.
    [SerializeField] private PlayerMovement player; // Player movement component, likely to be disabled during the task.
    public bool isGaming = false; // Flag to check if the game (task) is active.

    private void Start()
    {
        // Disable player movement when the canvas is active and player is not null.
        if (player != null && GetComponent<Canvas>().enabled == true)
        {
            player.enabled = false;
        }

        availableColors = new List<Color>(wireColors);
        availableLeftWireIndex = new List<int>();
        availableRightWireIndex = new List<int>();

        // Initialize indices for left and right wires.
        for (int i = 0; i < leftWires.Count; i++) { availableLeftWireIndex.Add(i); }
        for (int i = 0; i < rightWires.Count; i++) { availableRightWireIndex.Add(i); }

        // Randomly assign colors to pairs of left and right wires.
        while (availableColors.Count > 0 && availableLeftWireIndex.Count > 0 && availableRightWireIndex.Count > 0)
        {
            Color picked = availableColors[Random.Range(0, availableColors.Count)];
            int pickedLeftWireIndex = Random.Range(0, availableLeftWireIndex.Count);
            int pickedRightWireIndex = Random.Range(0, availableRightWireIndex.Count);

            leftWires[availableLeftWireIndex[pickedLeftWireIndex]].SetColor(picked);
            rightWires[availableRightWireIndex[pickedRightWireIndex]].SetColor(picked);
            availableColors.Remove(picked);
            availableLeftWireIndex.RemoveAt(pickedLeftWireIndex);
            availableRightWireIndex.RemoveAt(pickedRightWireIndex);
        }
    }

    // Start the timer for the wiring task.
    public void StartTimer()
    {
        if (!isGaming)
        {
            StartCoroutine(TimerCoroutine());
            isGaming = true;
            player.enabled = false; // Disable player movement during the task.
        }
    }

    // Coroutine to handle the timing of the wire task.
    private IEnumerator TimerCoroutine()
    {
        bool fried = false; // Flag to indicate a wrong connection.

        for (int i = 10; i >= 0; i--)
        {
            counter = 0;
            yield return new WaitForSeconds(1f);
            timer.text = i.ToString() + " second remaining";

            // Check each left wire for success or failure.
            foreach (Wire wire in leftWires)
            {
                if (wire.isSucces)
                {
                    counter++;
                }
                else if (wire.isFried) { fried = true; break; }
            }
            if (counter == leftWires.Count || fried)
            {
                break;
            }
        }

        // Reset all wires after the task is completed or failed.
        foreach (Wire wire in leftWires)
        {
            wire.ResetWire();
        }
        foreach (Wire wire in rightWires)
        {
            wire.ResetWire();
        }

        isGaming = false; // Mark the game as inactive.
        player.enabled = true; // Re-enable player movement.
    }
}

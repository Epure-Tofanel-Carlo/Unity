using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMission : MonoBehaviour
{
    public Canvas dialog; // Canvas for displaying the mission dialog.
    public Canvas buttoninfo; // Canvas for displaying button prompts or additional information.
    public MissionText missionText; // Reference to the MissionText component to control the mission dialog.

    // Initializes the dialog and button information canvases.
    void Start()
    {
        // Initially disable the dialog and button information canvases.
        buttoninfo.enabled = false;
        dialog.enabled = false;
    }

    // Check for player interaction while staying within a trigger zone.
    private void OnTriggerStay2D(Collider2D collision)
    {
        // If the player presses the 'E' key and no mission is currently active.
        if (Input.GetKey(KeyCode.E) && !dialog.GetComponent<GenerateProps>().isMissionActive())
        {
            // Enable the dialog canvas to start showing the mission text.
            dialog.enabled = true;
            // Disable the button information canvas as it's not needed when dialog is active.
            buttoninfo.enabled = false;
            // Start displaying the mission dialog.
            missionText.startDialogue();
        }
    }

    // Trigger when the player enters the collider area.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If no mission is currently active.
        if (!dialog.GetComponent<GenerateProps>().isMissionActive())
        {
            // Reset the mission text frame to be ready for new dialog.
            missionText.resetFrame();
            // Enable the button information canvas to show available actions or prompts.
            buttoninfo.enabled = true;
        }
    }

    // Trigger when the player exits the collider area.
    private void OnTriggerExit2D(Collider2D collision)
    {
        // Disable the button information canvas when the player leaves the area.
        buttoninfo.enabled = false;
    }
}

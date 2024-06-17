using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionText : MonoBehaviour
{
    [SerializeField] private Image npcFace; // UI element to display the NPC's face.
    [SerializeField] private Text npcMessage; // UI text element for displaying messages.
    public MissionSO mission; // Scriptable object holding mission data.
    [SerializeField] private GameObject responseFrame; // Frame for player response options.
    private bool isTexting = false; // Flag to check if the text is currently being typed out.

    // Initialize the NPC's face and clear any existing messages.
    private void Start()
    {
        npcFace.sprite = mission.getNpcFace(); // Set the sprite to the NPC's face from the mission data.
        npcMessage.text = ""; // Clear the message text.
        responseFrame.SetActive(false); // Hide the response options.
    }

    // Reset the dialogue frame to its initial state.
    public void resetFrame()
    {
        npcFace.sprite = mission.getNpcFace(); // Update the NPC face in case it changes.
        npcMessage.text = ""; // Clear any existing text.
        responseFrame.SetActive(false); // Ensure response options are hidden.
    }

    // Start the dialogue by typing out the mission dialogue one character at a time.
    public void startDialogue()
    {
        if (!isTexting)
        {
            StartCoroutine(TypeLine()); // Start the coroutine that types out the text.
            isTexting = true;
        }
    }

    // Coroutine to type out each character of the mission dialogue with a delay.
    private IEnumerator TypeLine()
    {
        string message = mission.getMissionDialog(); // Retrieve the dialogue from the mission data.
        foreach (char c in message.ToCharArray()) // Iterate through each character in the dialogue.
        {
            npcMessage.text += c; // Add the character to the displayed text.
            yield return new WaitForSeconds(0.05f); // Wait briefly between characters to simulate typing.
        }
        isTexting = false; // Mark the texting as complete.
        responseFrame.SetActive(true); // Show response options after the dialogue is fully displayed.
    }
}

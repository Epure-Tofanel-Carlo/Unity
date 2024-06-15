using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachineActivator : MonoBehaviour
{
    public string slotMachineScene = "SlotMachine"; // Numele scenei de slot machine
    private bool playerInRange = false;
    private Collider2D playerCollider;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Player pressed E while in range.");
            LoadSlotMachineScene();
        }
    }

    void LoadSlotMachineScene()
    {
        Debug.Log("Loading SlotMachine scene.");
        SceneManager.LoadScene(slotMachineScene);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerCollider = other;
            Debug.Log("Player entered the trigger zone.");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerCollider = null;
            Debug.Log("Player exited the trigger zone.");
        }
    }
}

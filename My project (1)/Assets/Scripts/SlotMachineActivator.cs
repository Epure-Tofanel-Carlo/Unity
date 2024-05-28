using UnityEngine;
using UnityEngine.SceneManagement;

public class SlotMachineActivator : MonoBehaviour
{
    public string slotMachine= "SlotMachine";

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        { // daca apasa e, trimite catre scena cu SlotMachine
            LoadSlotMachineScene();
        }
    }

    void LoadSlotMachineScene()
    {
        SceneManager.LoadScene(slotMachine);
    }
}

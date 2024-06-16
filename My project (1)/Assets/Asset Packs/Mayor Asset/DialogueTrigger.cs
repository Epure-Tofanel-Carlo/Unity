using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Canvas dialog; // Canvas to display the dialogue
    public Canvas buttoninfo; // Canvas to display the "Press E" info
    void Start()
    {
        buttoninfo.enabled = false;
        dialog.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            dialog.enabled = true;
            buttoninfo.enabled = false;
           
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        buttoninfo.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        buttoninfo.enabled = false;
        dialog.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMission : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas dialog;
    public Canvas buttoninfo;
    public MissionText missionText;
    void Start()
    {
       
        buttoninfo.enabled = false;
        dialog.enabled = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E) && !dialog.GetComponent<GenerateProps>().isMissionActive())
        {
            dialog.enabled = true;
            buttoninfo.enabled = false;
            missionText.startDialogue();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!dialog.GetComponent<GenerateProps>().isMissionActive())
        {
            missionText.resetFrame();
            buttoninfo.enabled = true;
        }
        

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       
        buttoninfo.enabled = false;
    }
}

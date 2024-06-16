using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class FishOpener : MonoBehaviour
{
    [SerializeField] GameObject fishingMinigame;
    [SerializeField] Canvas buttonInfo;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            fishingMinigame.SetActive(true);
            fishingMinigame.GetComponentInChildren<FishMovement>().startMiniGame();
            buttonInfo.enabled = false;
;        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        buttonInfo.enabled = true;

    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        // Debug.Log("A iesit");
        buttonInfo.enabled = false;
    }
}

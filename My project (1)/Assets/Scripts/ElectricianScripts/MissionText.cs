using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionText : MonoBehaviour
{
    [SerializeField] private Image npcFace;
    [SerializeField] private Text npcMessage;
    public MissionSO mission;
    [SerializeField] private GameObject responseFrame;
    private bool isTexting= false;

    private void Start(){
        npcFace.sprite = mission.getNpcFace();
        npcMessage.text = "";
        responseFrame.SetActive(false);
    }

    public void resetFrame()
    {
        npcFace.sprite = mission.getNpcFace();
        npcMessage.text = "";
        responseFrame.SetActive(false);
    }

    public void startDialogue()
    {

        if (!isTexting)
        {
            StartCoroutine(TypeLine());
            isTexting = true;
        }
            
        
    }
    private IEnumerator TypeLine()
    {
        string message = mission.getMissionDialog();
        foreach (char c in message.ToCharArray())
        {
            npcMessage.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        isTexting = false;
        responseFrame.SetActive(true);
    }
}

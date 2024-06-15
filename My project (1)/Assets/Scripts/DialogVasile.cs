using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogVasile : MonoBehaviour
{
    [SerializeField] private Image npcFace;
    [SerializeField] private Text npcMessage;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;
    [SerializeField] private GameObject responseFrame;
    private bool isTexting = false;

    public PlayerInventory playerInventory; // Instanta a clasei PlayerInventory

    private int goldCost = 20;

    private void Start()
    {
        npcMessage.text = "";
        responseFrame.SetActive(false);

        acceptButton.onClick.AddListener(OnAccept);
        declineButton.onClick.AddListener(OnDecline);

        playerInventory = new PlayerInventory();
    }

    public void ResetFrame()
    {
        npcMessage.text = "";
        responseFrame.SetActive(false);
    }

    public void StartDialogue()
    {
        if (!isTexting)
        {
            StartCoroutine(TypeLine());
            isTexting = true;
            Debug.Log("StartDialogue");
        }
    }

    private IEnumerator TypeLine()
    {
        string message = "Salut sefule, sunt Nea Vasile. Da-mi $20 sa ma imbogatesc. Nu te voi uita, sunt pe val si iti voi plati inapoi. Nu asculta ce spun oamenii, intr-o zi voi castiga la jocuri de noroc si ma voi imbogati. Mi-am vandut ferma, dar o sa cumpar tot orasul.";
        foreach (char c in message.ToCharArray())
        {
            npcMessage.text += c;
            yield return new WaitForSeconds(0.05f);
        }
        isTexting = false;
        responseFrame.SetActive(true);
        Debug.Log("Message typed, showing response frame");
    }

    public void OnAccept()
    {
        if (playerInventory.gold >= goldCost)
        {
            playerInventory.gold -= goldCost;
            Debug.Log("Tranzactie acceptata.");
        }
        else
        {
            Debug.Log("Nu ai suficient gold.");
        }
        ResetFrame();
    }

    public void OnDecline()
    {
        Debug.Log("Tranzactie refuzata.");
        ResetFrame();
    }
}

[System.Serializable]
public class PlayerInventory
{
    public int gold = 100;
}
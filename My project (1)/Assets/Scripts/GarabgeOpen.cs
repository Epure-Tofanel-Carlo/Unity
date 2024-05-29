using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarabgeOpen : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas garabge;
    public ItemGenerate itemGenerate;
    public Canvas infoButton;
     void Start()
    {
        garabge.enabled = false;
        infoButton.enabled = false;
    }
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            itemGenerate.OnCanvasOpenButtonClicked();
            garabge.enabled = true;
            infoButton.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Debug.Log("A intrat");
        infoButton.enabled = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        // Debug.Log("A iesit");
        infoButton.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public Canvas shop;
    public Canvas buttoninfo;
    public Canvas hud;
     void Start()
    {
        hud.enabled = true;
        buttoninfo.enabled = false;
        shop.enabled = false;
    }
    void Update()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
            hud.enabled = false;
            shop.enabled = true;
            buttoninfo.enabled = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log("A intrat");
        buttoninfo.enabled = true;
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        hud.enabled = true;
        // Debug.Log("A iesit");
        buttoninfo.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricianOpener : MonoBehaviour
{
    // Start is called before the first frame update
    // Start is called before the first frame update
    public Canvas electrician;
    public Canvas buttoninfo;
    public Canvas hud;
    public WireTask wires;
    public GenerateProps props;
    private bool waiting = false;
    
    void Start()
    {
        hud.enabled = true;
        buttoninfo.enabled = false;
        electrician.enabled = false;
        
    }
  
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.E))
        {
           
            hud.enabled = false;
            electrician.enabled = true;
            buttoninfo.enabled = false;
            foreach (Wire wire in wires.leftWires) {
                wire.wireLocation();
            }
            foreach (Wire wire in wires.rightWires)
            {
                wire.wireLocation();
            }
            if (!waiting)
            {
                StartCoroutine(WaitForTimer());
                waiting = true;
            }
            
        }
    }

    private IEnumerator WaitForTimer()
    {
        wires.StartTimer();
        yield return new WaitUntil(() => !wires.isGaming); 

        
       if(wires.counter == wires.leftWires.Count)
        {
          
            electrician.enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            props.addSucces();

        }
        else
        {
           
            electrician.enabled = false;
           // gameObject.SetActive(false);
           gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponentsInChildren<SpriteRenderer>()[1].enabled = true;
            props.addFail();
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

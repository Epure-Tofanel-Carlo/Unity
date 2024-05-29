using GamePlay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WireTask : MonoBehaviour
{
    public Text timer;
    public List<Color> wireColors = new List<Color>();
    public List<Wire> leftWires = new List<Wire>();
    public List<Wire> rightWires = new List<Wire>();

    private List<Color> availableColors;
    private List<int> availableLeftWireIndex;
    private List<int> availableRightWireIndex;
    public int counter=-1;
    public Wire currentDraggedWire;
    public Wire currentHoveredWire;
    [SerializeField] private PlayerMovement player;
    public bool isGaming=false;
    private void Start()
    {
        if(player != null && GetComponent<Canvas>().enabled ==true)
        {
            player.enabled = false;
        }
        availableColors = new List<Color>(wireColors);
        availableLeftWireIndex = new List<int>();
        availableRightWireIndex = new List<int>();


        for (int i = 0;i<leftWires.Count;i++){availableLeftWireIndex.Add(i);}
        for (int i = 0; i < rightWires.Count; i++) { availableRightWireIndex.Add(i); }
        while(availableColors.Count >0 && availableLeftWireIndex.Count >0 && availableRightWireIndex.Count > 0)
        {
            Color picked = availableColors[Random.Range(0,availableColors.Count)];
            int pickedLeftWireIndex = Random.Range(0, availableLeftWireIndex.Count);
            int pickedRightWireIndex = Random.Range(0, availableRightWireIndex.Count);


            leftWires[availableLeftWireIndex[pickedLeftWireIndex]].SetColor(picked);
            rightWires[availableRightWireIndex[pickedRightWireIndex]].SetColor (picked);
            availableColors.Remove(picked);
            availableLeftWireIndex.RemoveAt(pickedLeftWireIndex);
            availableRightWireIndex.RemoveAt(pickedRightWireIndex);
           
        }
    }

    public void StartTimer()
    {
        if (!isGaming)
        {
            StartCoroutine(TimerCoroutine());
           
            isGaming = true;
            player.enabled = false;
        }
    }
   

    private IEnumerator TimerCoroutine()
    {
        bool fried = false;
        
        for (int i = 10; i >=0; i--)
        {
            counter = 0;
            yield return new WaitForSeconds(1f);
            timer.text = i.ToString() + " second remaining";
           
            foreach (Wire wire in leftWires)
            {

                if (wire.isSucces)
                {
                    counter++;
                }else if (wire.isFried) { fried = true; break; }
                //wire.ResetWire();
            }
           if(counter == leftWires.Count || fried)
            {
                
                break;
            }
        }
        //Debug.Log(fried);
        foreach (Wire wire in leftWires)
        {

            wire.ResetWire();
        }
        foreach (Wire wire in rightWires)
        {

            wire.ResetWire();
        }

        isGaming = false;
        player.enabled = true;

    }



}

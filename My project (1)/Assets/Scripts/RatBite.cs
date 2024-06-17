using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatBite : MonoBehaviour
{
    [SerializeField] private Canvas parentCanvas;
    [SerializeField] private ItemGenerate itemGenerate;
    void Start()
    {         
        parentCanvas = GetComponentInParent<Canvas>();  // tragem canvas u etc
        itemGenerate = GetComponentInParent<ItemGenerate>();
    }

    public void bite()
    {
        if (parentCanvas != null)
        {
            parentCanvas.enabled = false;
            itemGenerate.deleteItems();   // stergem si sobolanu dupa ce da bite
            Debug.Log("A rat bite you");
        }
    }

   
}

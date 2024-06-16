using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class ItemInfoManager : MonoBehaviour
{
    public static ItemInfoManager Instance;
    public TextMeshProUGUI itemDescription;


    private void Awake()
    {
        if(Instance != null && Instance!= this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        Cursor.visible = true;
        gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Input.mousePosition;
    }

    public void setAndShowInfo(string message)
    {
        gameObject.SetActive(true);
        itemDescription.text = message;
    }

    public void hideInfo()
    {
        gameObject.SetActive(false);
        itemDescription.text = string.Empty;
    }
}

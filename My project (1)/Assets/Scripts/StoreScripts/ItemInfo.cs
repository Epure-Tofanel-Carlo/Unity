using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public string message;
    /// <summary>
    /// "Price: 200\r\nName: Apple\r\nDescription: Just an apple"
    /// </summary>
    private void OnMouseEnter()
    {
        print("Mouse-ul a intrat pe obiectul: " + gameObject.name);
        ItemInfoManager.Instance.setAndShowInfo(message);   
    }
    private void OnMouseExit()
    {
        ItemInfoManager.Instance.hideInfo();
    }

}

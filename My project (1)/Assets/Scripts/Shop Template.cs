using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class ShopTemplate : MonoBehaviour
{
    [SerializeField] private Text price;
    [SerializeField] private Text nameText;
    [SerializeField] private Image photo;
    [SerializeField] private BodyPart_SO body;
    [SerializeField] private PlayerBluePrint player;
    
    
    void Start()
    {
        
        Text[] texts = GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
           
            if (text.gameObject.name == "Price")
            {
                price = text; 
            }
        }
    }
    public void setBodyPart(BodyPart_SO bodyPart)
    {
        this.body = bodyPart;
        setName(body.name);
        setImage(body.getImage());
        setPrice(body.getPrice().ToString());
    }
   public void setPrice(string price)
    {
        this.price.text = price;
    }
    public void setImage(Sprite sprite)
    {
        this.photo.sprite = sprite;
    }
    public void setName(string name)
    {
        if (name.Split('_')[0] == "Hairstyle")
        {
            this.name ="Hair_" + name.Split('_')[1];
        }
        else
        {
            this.name = name;
        }
    }

    public void buyItem()
    {
        if (player.updateMoney(body.getPrice()))
        {
            body.buyed();
            Debug.Log(transform.gameObject.name);
        }
        
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudManager : MonoBehaviour
{
    public Text money;
    public PlayerBluePrint player;
   void Start()
    {
        Text[] texts = GetComponentsInChildren<Text>();
        foreach(Text text in texts)
        {
            if(text.gameObject.name == "Wallet")
            {
                money = text;
                break;
            }
        }
        money.text = player.getMoney().ToString();

    }

    // Update is called once per frame
    public void updateMoney()
    {
        money.text = player.getMoney().ToString();
    }

}

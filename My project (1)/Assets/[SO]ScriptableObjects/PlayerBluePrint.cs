using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlayerBluePrint", menuName = "Custom/Create Player")]
[System.Serializable]
public class PlayerBluePrint : ScriptableObject
{
    //SerializeField pot sa il vad in unity, dar nu pot sa il modific(View-Only)
    [SerializeField] private float health;
    [SerializeField] private int armor;
    [SerializeField] private int money;

    public PlayerBluePrint()
    {
        health = 100;
        armor = 100;
        money = 10000;
    }
    public float getHealth() { return health; }
    public int getArmor() { return armor;}
    public int getMoney() {  return money; }

    public void setHealth(float health) { this.health = health;}

    public bool updateMoney(int coins) { 
    if (coins <= 0) return false;
        if (money - coins < 0) { 
            return false;
        }
        else { 
        money -= coins;
            return true;
        }
    }
    public void giveMoney(int coins)
    {
        money += coins;
    }





}

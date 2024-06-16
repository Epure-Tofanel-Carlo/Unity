using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "NewPlayerBluePrint", menuName = "Custom/Create Player")]
[System.Serializable]
public class PlayerBluePrint : ScriptableObject
{
    //SerializeField pot sa il vad in unity, dar nu pot sa il modific(View-Only)
    [SerializeField] private float health;
    [SerializeField] private int armor;
    [SerializeField] private int money;
    [SerializeField] private bool finalScene;
    public PlayerBluePrint()
    {
        health = 100;
        armor = 100;
        money = 10000;
        finalScene = false;
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
        checkWinGame(); //de fiecare data cand actualizam banii player-ului verificam daca s-a atins suma finala
    }

    public void checkWinGame() //functie care verifica daca player ul atinge o anumita suma de bani atunci jocul se termina 
    {//si se afiseaza credit scene ul 
        if (money >= 20000 && !finalScene) { finalScene = true; SceneManager.LoadScene(12);} 

    }


}

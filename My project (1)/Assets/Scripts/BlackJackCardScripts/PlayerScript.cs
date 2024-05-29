using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    // --- SCRIPTUL E SI PT PLAYER SI DEALER

    // Luam restu scripturilor
    public CardScript cardScript;   
    public DeckScript deckScript;

    // valoarea totala la mana jucatorului
    public int handValue = 0;

    // Cati bani ai
    private int money = 1000;

    // Cate carti ai pe masa
    public GameObject[] hand;
    // Index ul urmatoarei carti care sa fie intoarsa
    public int cardIndex = 0;
    // Dam track la Asi pt 11/1 conversii
    List<CardScript> aceList = new List<CardScript>();

    public void StartHand()
    {
        GetCard();
        GetCard();
    }

    // Adaugam o carte la hand
    public int GetCard()
    {
        // Luam o carte, folosim deal script sa asigneze un sprite si o valoare 
        int cardValue = deckScript.DealCard(hand[cardIndex].GetComponent<CardScript>());
        // Aratam cartea
        hand[cardIndex].GetComponent<Renderer>().enabled = true;
        // Adaugam valoarea cartii
        handValue += cardValue;
        // Daca e 1, e As
        if(cardValue == 1)
        {
            aceList.Add(hand[cardIndex].GetComponent<CardScript>());
        }
        // Verificam daca folosim As u ca 11 sau ca 1
        AceCheck();
        cardIndex++;
        return handValue;
    }

    // Verificam daca avem nevoie de As 11 sau 1
    public void AceCheck()
    {
        // pt fiecare As in mana
        foreach (CardScript ace in aceList)
        {
            if(handValue + 10 < 22 && ace.GetValueOfCard() == 1) // daca am da bust cu As-u fiind 11, il punem 1
            {
                // Daca convertim, schimbam valorile
                ace.SetValue(11);
                handValue += 10;
            } else if (handValue > 21 && ace.GetValueOfCard() == 11) // daca mai avem loc de As-u ca fiind 11, il adaugam/lasam asa
            {
                // Daca convertim, schimbam valorile
                ace.SetValue(1);
                handValue -= 10;
            }
        }
    }

    // adaugam sau scadem bani, pt bets
    public void AdjustMoney(int amount)
    {
        money += amount;
    }

    // Output la banii curenti la player 
    public int GetMoney()
    {
        return money;
    }

    // Ascunde toate cartile, reseteaza variabilele necesare 
    public void ResetHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            hand[i].GetComponent<CardScript>().ResetCard();
            hand[i].GetComponent<Renderer>().enabled = false;
        }
        cardIndex = 0;
        handValue = 0;
        aceList = new List<CardScript>();
    }
}

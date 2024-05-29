using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Butoanele
    public Button dealBtn;
    public Button hitBtn;
    public Button standBtn;
    public Button betBtn;

    private int standClicks = 0;

    // Dealer, Player script
    public PlayerScript playerScript;
    public PlayerScript dealerScript;

    // text pt hud
    public Text scoreText;
    public Text dealerScoreText;
    public Text betsText;
    public Text cashText;
    public Text mainText;
    public Text standBtnText; 

    // Cartea care ascunde prima carte a dealer-ului
    public GameObject hideCard;
    // Cat e pot u 
    int pot = 0;

    void Start()
    {
        // Linstenere pt butoane
        dealBtn.onClick.AddListener(() => DealClicked());
        hitBtn.onClick.AddListener(() => HitClicked());
        standBtn.onClick.AddListener(() => StandClicked());
        betBtn.onClick.AddListener(() => BetClicked());
    }

    private void DealClicked()
    {
        // Resetam runda, ascundem textul, pregatim pentru o noua mana
        playerScript.ResetHand();
        dealerScript.ResetHand();
        // Ascundem dealer hand score u la inceput 
        dealerScoreText.gameObject.SetActive(false);
        mainText.gameObject.SetActive(false);
        dealerScoreText.gameObject.SetActive(false);
        GameObject.Find("Deck").GetComponent<DeckScript>().Shuffle();
        playerScript.StartHand();
        dealerScript.StartHand();
        // Updatam score-urile
        scoreText.text = "Hand: " + playerScript.handValue.ToString();
        dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
        // Ascundem din nou cartea dealer-ului
        hideCard.GetComponent<Renderer>().enabled = true;
        // Vizibilitatea la butoane, ca se schimba daca esti in runda sau intre runde
        dealBtn.gameObject.SetActive(false);
        hitBtn.gameObject.SetActive(true);
        standBtn.gameObject.SetActive(true);
        standBtnText.text = "Stand";
        // Standard pot e 40
        pot = 40;
        betsText.text = "Bets: $" + pot.ToString();
        playerScript.AdjustMoney(-20);
        cashText.text = "$" + playerScript.GetMoney().ToString();

    }

    private void HitClicked()
    {
        // Verificam daca e loc pe masa, 2 carti standard + 8 pe langa
        // pur teoretic mai era loc, dar nu mai aveam loc pe masa sa arate dragut si sansa in viata reala sa ajungi pana acolo e 1 la 3 miliarde sau cv de genul
        if (playerScript.cardIndex <= 10)
        {
            playerScript.GetCard();
            scoreText.text = "Hand: " + playerScript.handValue.ToString();
            if (playerScript.handValue > 20) RoundOver();
        }
    }

    private void StandClicked()
    {
        standClicks++;
        if (standClicks > 1) RoundOver();
        HitDealer();
        standBtnText.text = "Call";
    }

    // Add bani la pot daca am dat click pe bet
    void BetClicked()
    {
        Text newBet = betBtn.GetComponentInChildren(typeof(Text)) as Text;
        int intBet = int.Parse(newBet.text.ToString().Remove(0, 1)); // strip la dolar
        playerScript.AdjustMoney(-intBet); // scadem din bani ca am adaugat la pot
        cashText.text = "$" + playerScript.GetMoney().ToString();
        pot += (intBet * 2);
        betsText.text = "Bets: $" + pot.ToString();
    }

    private void HitDealer()
    {
        while (dealerScript.handValue < 16 && dealerScript.cardIndex < 10)  // Dealer u da hit daca are spatiu, si sub 16 la handvalue, se intampla cand dam noi stand
        {
            dealerScript.GetCard();
            dealerScoreText.text = "Hand: " + dealerScript.handValue.ToString();
            if (dealerScript.handValue > 20) RoundOver();
        }
    }

    // Check for winner si loser, round over
    void RoundOver()
    {
        // booleans pt bust, sau 21
        bool playerBust = playerScript.handValue > 21;
        bool dealerBust = dealerScript.handValue > 21;
        bool player21 = playerScript.handValue == 21;
        bool dealer21 = dealerScript.handValue == 21;
   
        // Daca stand a fost dat click <2, n avem 21 sau bust, quit
        if (standClicks < 2 && !playerBust && !dealerBust && !player21 && !dealer21) return;
        bool roundOver = true;
        // All bust, banii revin
        if (playerBust && dealerBust)
        {
            mainText.text = "All Bust: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }

        // Daca un player a dat bust, dealaeru nu, sau daca dealeru are mai multe puncte, DEALER WIN
        else if (playerBust || (!dealerBust && dealerScript.handValue > playerScript.handValue))
        {
            mainText.text = "Dealer wins!";
        }
        
        // Daca dealaeru a dat bust, playeru nu, sau daca playeru are mai multe puncte, PLAYER WIN
        else if (dealerBust || playerScript.handValue > dealerScript.handValue)
        {
            mainText.text = "You win!";
            playerScript.AdjustMoney(pot);
        }
        // Verificam daca nu cumva e egal, inapoiem banii
        else if (playerScript.handValue == dealerScript.handValue)
        {
            mainText.text = "Push: Bets returned";
            playerScript.AdjustMoney(pot / 2);
        }
        else
        {
            roundOver = false;
        }
        // Dam setup la UI pt urmatoarea tura
        if (roundOver)
        {
            hitBtn.gameObject.SetActive(false);
            standBtn.gameObject.SetActive(false);
            dealBtn.gameObject.SetActive(true);
            mainText.gameObject.SetActive(true);
            dealerScoreText.gameObject.SetActive(true);
            hideCard.GetComponent<Renderer>().enabled = false;
            cashText.text = "$" + playerScript.GetMoney().ToString();
            standClicks = 0;
        }
    }

    
}

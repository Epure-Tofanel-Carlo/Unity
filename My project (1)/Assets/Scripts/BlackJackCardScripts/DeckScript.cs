using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    public Sprite[] cardSprites;
    int[] cardValues = new int[53];
    int currentIndex = 0;

    void Start()
    {
        GetCardValues();
    }

    void GetCardValues()
    {
        int num;
        for (int i = 0; i < cardSprites.Length; i++)
        {
            num = i % 13;
            if (num > 9) // pentru J, Q, K etc
            {
                num = 10;
            }
            else if (num == 0) // Pentru As
            {
                num = 1; // 1 deoarece facem check-uri cu el altundeva
            }
            else // Pentru 2-10
            {
                num = num + 1; // +1 ca avem indexare 0 
            }
            cardValues[i] = num;
        }
    }

    public void Shuffle()
    {
        // swap la 2 carti random cu metoda paharul cum ar spune profa mea din liceu
        for(int i = cardSprites.Length -1; i > 0; --i)
        {
            int j = Mathf.FloorToInt(Random.Range(0.0f, 1.0f) * cardSprites.Length - 1) + 1;
            Sprite face = cardSprites[i];
            cardSprites[i] = cardSprites[j];
            cardSprites[j] = face;

            int value = cardValues[i];
            cardValues[i] = cardValues[j];
            cardValues[j] = value;
        }
        currentIndex = 1;
    }

    public int DealCard(CardScript cardScript)
    {
        cardScript.SetSprite(cardSprites[currentIndex]);
        cardScript.SetValue(cardValues[currentIndex]);
        currentIndex++;
        return cardScript.GetValueOfCard();
    }

    public Sprite GetCardBack()
    {
        return cardSprites[0];
    }
}

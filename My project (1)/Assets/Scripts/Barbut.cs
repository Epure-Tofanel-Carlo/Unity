using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UI;
using System.Linq;

public class Barbut : MonoBehaviour
{
    Dices playerDice = new Dices();
    Dices oponentDice = new Dices();
    [SerializeField] private PlayerBluePrint playerStats;
    [SerializeField] InputField betInput;
    [SerializeField] HudManager hudManager;
    [SerializeField] List<Image> playerDices;
    [SerializeField] List<Image> oponentDices;
    public GameObject diceRol;
    public string spriteSheetPath;
    public List<Sprite> spriteList;
    public GameObject button;
    public GameObject exitButton;

    private void Start()
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (Image image in images)
        {
            string type = image.gameObject.name.Split('D')[0];
            Debug.Log(type);
            if (type == "Player")
            {
                playerDices.Add(image);
                image.gameObject.SetActive(false);
            }
            else if (type == "Oponent")
            {
                oponentDices.Add(image);
                image.gameObject.SetActive(false);

            }
            else if (type == "Rolling")
            {

                diceRol = image.gameObject;
                diceRol.SetActive(false);

            }else if(type == "StartRoll")
            {
                button = image.gameObject;
            }else if(type == "Exit")
            {
                exitButton = image.gameObject;
            }
           
        }
        Debug.Log(exitButton);
        Debug.Log(button);

        betInput = GetComponentInChildren<InputField>();
        Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath).OfType<Sprite>().ToArray();
       
        spriteList = new List<Sprite>(sprites);
        spriteList.Sort((a, b) => int.Parse(a.name.Split('_')[1]).CompareTo(int.Parse(b.name.Split('_')[1])));

        //rollDice();
    }
    public void HideAllDice()
    {
        foreach (var img in playerDices)
        {
            img.gameObject.SetActive(false);
        }

        foreach (var img in oponentDices)
        {
            img.gameObject.SetActive(false);
        }

    }

    public void rollDice()
    {
       
        int bet;
        if ((int.TryParse(betInput.text, out bet)))
        {
            if (bet > 0 && bet <= playerStats.getMoney())
            {
                StartCoroutine(RollAndEvaluate(bet));
            }
        }
        else
        {
            Debug.Log("Camp gol");
        }
       

    }

    private IEnumerator RollAndEvaluate(int bet)
    {
        button.SetActive(false);
        exitButton.SetActive(false);
        oponentDice.generateDices();
        playerDice.generateDices();
        float delay = 2f;
        for (int i = 0; i < playerDices.Count; i++)
        {
            diceRol.transform.position = playerDices[i].transform.position;
            playerDices[i].gameObject.SetActive(false);
            diceRol.SetActive(true);
            yield return new WaitForSeconds(delay); // Asteapta ca sa fie animat cat de cat
            diceRol.SetActive(false);
            playerDices[i].gameObject.SetActive(true);
            playerDices[i].sprite = spriteList[playerDice.getDices()[i] - 1];
        }

        for (int i = 0; i < oponentDices.Count; i++)
        {
            diceRol.transform.position = oponentDices[i].transform.position;
            oponentDices[i].gameObject.SetActive(false);
            diceRol.SetActive(true);
            yield return new WaitForSeconds(delay); // Asteapta ca sa fie animat cat de cat
            diceRol.SetActive(false);
            oponentDices[i].gameObject.SetActive(true);
            oponentDices[i].sprite = spriteList[oponentDice.getDices()[i] - 1];
        }

        if (playerDice.diceValue() > oponentDice.diceValue())
        {
            Debug.Log("Player a castigat: (" + playerDice.getDices()[0].ToString() + ", " + playerDice.getDices()[1].ToString() + ")");
            playerStats.giveMoney(bet);

        }
        else
        {
            playerStats.updateMoney(bet);
            Debug.Log("Oponent wins(" + oponentDice.getDices()[0].ToString() + ", " + oponentDice.getDices()[1].ToString() + ")");
        }
        exitButton.SetActive(true);
        button.SetActive(true);
        hudManager.updateMoney();

    }

    public class Dices
    {
        List<int> dices;

        public void generateDices()
        {
            dices = new List<int>();
            dices.Add(Random.Range(1, 6));
            dices.Add(Random.Range(1, 6));
        }
        public int diceValue()
        {
            return dices[0] + dices[1];
        }
        public List<int> getDices()
        {
            return dices;
        }
    }
}

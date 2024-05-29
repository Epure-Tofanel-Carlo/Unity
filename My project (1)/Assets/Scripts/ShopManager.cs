using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private List<BodyPartSelection> shopParts = new List<BodyPartSelection>();
    public string bodyPartsPath;
    public GameObject Content;
    public GameObject ItemPrefab;
    void Start()
    {
        getAllBodyParts();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void getAllBodyParts()
    {
        string[] folders = Directory.GetDirectories(bodyPartsPath, "*", SearchOption.TopDirectoryOnly);
        // iau din directorul care contine SO-urile iau toate folderele(path-ul de la ele)

        foreach (string folderPath in folders) // trec prin fiecare acum
        {

            string folderName = Path.GetFileName(folderPath); // Numele de la foldere sunt identice cu numele de la Child Object. Ex: Body, Hairstyle
            BodyPartSelection bodyPart2 = new BodyPartSelection(folderName); // Folosesc constructorul sa fac un Selector

            string[] bodypartstPaths = Directory.GetFiles(folderPath, "*.asset"); // Iau toate SO-urile 
            foreach (string bodypartPath in bodypartstPaths)
            {

                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(bodypartPath); // Pentru a nu da eroare folosesc clasa Object care e generala pentru SO-rui
                for (int i = 0; i < assets.Length; i++)
                {
                    
                   
                    if (assets[i] is BodyPart_SO) // Aici verific daca asset-ul selectat este chiar BodyPart_SO.
                    {
                        BodyPart_SO bodyPart_SO = (BodyPart_SO)assets[i];
                        if (!bodyPart_SO.isPurchased()) // verific daca item-ul este cumparat sau nu
                        {
                            
                            GameObject Item = Instantiate(ItemPrefab);
                            Item.name = assets[i].name;
                           
                            ShopTemplate shopTemplate = Item.GetComponent<ShopTemplate>();
                            shopTemplate.setBodyPart(bodyPart_SO);
                            Item.transform.SetParent(Content.transform);

                            ;
                            bodyPart2.addBodyPart(bodyPart_SO); // Aici il adaug in lista
                        }
                    }

                }

                shopParts.Add(bodyPart2);
            }
        }
    }
        [System.Serializable]
    public class BodyPartSelection
    {
        public string bodyPartName;
        public List<BodyPart_SO> bodyParts = new List<BodyPart_SO>();
        

        public BodyPartSelection(string name)
        {
            this.bodyPartName = name;
        }

        public void addBodyPart(BodyPart_SO part)
        {
            bodyParts.Add(part);
        }
    }
}

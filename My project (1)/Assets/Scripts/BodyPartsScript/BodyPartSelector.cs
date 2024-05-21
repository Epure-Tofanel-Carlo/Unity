using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using UnityEngine.UI;

public class BodyPartSelector : MonoBehaviour
{
    public string BodyPartPath = "Assets/AnimationFolder/BodyPartsSO";
    [SerializeField] private PlayerAnimSO characterBody;
    [SerializeField] private List<BodyPartSelection> bodyPartSelections;
    [SerializeField] private BodyPartManager bodyPartManager;
    //private List<int> currentIndexs;
   

    void Start()
    {
        getAllBodyParts();
        bodyPartManager = transform.GetComponent<BodyPartManager>();
       //ParcharacterBody.getBody();


    }
    /* Ordinea:
      list.Add(beard);
        list.Add(body);
         list.Add(glasses);
        list.Add(hat);
        list.Add(hairstyle);
        list.Add(outfit);
     */

    public void NextBodyPart(int partIndex)
    {
        int currentIndex =  bodyPartSelections[partIndex].bodyParts.IndexOf(characterBody.getAllParts()[partIndex]);
           
        if(currentIndex != -1)
        {
           if(currentIndex+1 < bodyPartSelections[partIndex].bodyParts.Count) {
                characterBody.UpdatePart( partIndex, bodyPartSelections[partIndex].bodyParts[currentIndex + 1]);
            }
            else
            {
                characterBody.UpdatePart(partIndex, bodyPartSelections[partIndex].bodyParts[0]);
            }
        }
    }

    public void PreviousBody(int partIndex)
    {
        int currentIndex = bodyPartSelections[partIndex].bodyParts.IndexOf(characterBody.getAllParts()[partIndex]);

        if (currentIndex != -1)
        {
            if (currentIndex -1 >=0)
            {
                characterBody.UpdatePart(partIndex, bodyPartSelections[partIndex].bodyParts[currentIndex - 1]);
            }
            else
            {
                characterBody.UpdatePart(partIndex, bodyPartSelections[partIndex].bodyParts[bodyPartSelections[partIndex].bodyParts.Count-1]);
            }
        }

    }
   
 

    public void getAllBodyParts()
{
        bodyPartSelections = new List<BodyPartSelection>();
        string[] folders = Directory.GetDirectories(BodyPartPath, "*", SearchOption.TopDirectoryOnly); 
        // iau din directorul care contine SO-urile iau toate folderele(path-ul de la ele)

    foreach (string folderPath in folders) // trec prin fiecare acum
    {
            if(folders.Length > characterBody.bodyCount())
            {
                characterBody.createNewBodyPart();
            }
            
            string folderName = Path.GetFileName(folderPath); // Numele de la foldere sunt identice cu numele de la Child Object. Ex: Body, Hairstyle
        BodyPartSelection bodyPart2 = new BodyPartSelection(folderName); // Folosesc constructorul sa fac un Selector

        string[] bodypartstPaths = Directory.GetFiles(folderPath, "*.asset"); // Iau toate SO-urile 
        foreach (string bodypartPath in bodypartstPaths) 
        {
            Object[] assets = AssetDatabase.LoadAllAssetsAtPath(bodypartPath); // Pentru a nu da eroare folosesc clasa Object care e generala pentru SO-rui
            foreach (Object asset in assets)
            {
                if (asset is BodyPart_SO) // Aici verific daca asset-ul selectat este chiar BodyPart_SO.
                {
                    BodyPart_SO bodyPart_SO = (BodyPart_SO)asset;
                        if (bodyPart_SO.isPurchased())
                        {
                            bodyPart2.addBodyPart(bodyPart_SO); // Aici il adaug in lista
                            
                        }
                    
                      
                        /*
                         Ex selector:
                        Nume: Body
                        Lista de BodyPart_SO care provin din folderul respectiv
                         */
                }
            }
        }

        bodyPartSelections.Add(bodyPart2);
    }
}
}

[System.Serializable]
public class BodyPartSelection
{
    public string bodyPartName;
    public List<BodyPart_SO> bodyParts = new List<BodyPart_SO>();
    [HideInInspector] public int bodyPartCurrentIndex;
   // public Text bodyPartNameTextComponent = ;
    public BodyPartSelection(string name)
    {
        this.bodyPartName = name;
       // bodyPartNameTextComponent.text = name;
    }

    public void addBodyPart(BodyPart_SO part)
    {
        bodyParts.Add(part);
    }
}

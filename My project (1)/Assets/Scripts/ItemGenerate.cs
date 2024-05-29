using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ItemGenerate : MonoBehaviour
{
    [SerializeField] private GameObject dragObject;
    [SerializeField] private GameObject ratObject;
    [SerializeField] private GameObject colletableObject;
    public int itemNumber;
    public int collectableItems;
    [SerializeField] private Canvas canvas;
    public string resourcesFolderPath;
    private List<Sprite> loadedSprites = new List<Sprite>();
    public List<GameObject> createdObjects = new List<GameObject>();
    private bool itemsGenerated = false;
    private void Start()
    {
        LoadSpritesFromFolder(resourcesFolderPath);
       // generateItems();
    }

    public void deleteItems()
    {
        if (createdObjects.Count > 0)
        {
            foreach (GameObject obj in createdObjects)
            {
                if(obj != null)
                {
                    try
                    {
                        Destroy(obj);
                    }catch(System.Exception e) { 
                        Debug.LogException(e);
                    }
                    
                }
            }
        }
        createdObjects.Clear();
        itemsGenerated = false;
    }
    void LoadSpritesFromFolder(string folderPath)
    {
        string[] spriteSheetPaths = Directory.GetFiles(folderPath, "*.png");
        foreach (string spritePath in spriteSheetPaths)
        {
            if (File.Exists(spritePath))
            {
            //    Debug.Log(spritePath);
                loadedSprites.Add(AssetDatabase.LoadAssetAtPath<Sprite>(spritePath));
            }
        }
        Debug.Log(loadedSprites.Count);
        
        //loadedSprites.AddRange(sprites);
    }

    

  
    public void generateItems()
    {
        RectTransform canvasRect = canvas.GetComponent<RectTransform>();
        float canvasWidth = canvasRect.rect.width;
        float canvasHeight = canvasRect.rect.height;

        // Ținem evidența câtor obiecte au fost create
        int objectsCreated = 0;

        while (objectsCreated < collectableItems)
        {
            float randomX = Random.Range(-canvasWidth / 3f, canvasWidth / 3f);
            float randomY = Random.Range(-canvasHeight / 3f, canvasHeight / 3f);
            Vector3 position = new Vector3(randomX, randomY, 0);
          
            int randomIndex = Random.Range(0, loadedSprites.Count);
            GameObject newObject = Instantiate(colletableObject, position, Quaternion.identity);
            Image img = newObject.GetComponent<Image>();

            img = colletableObject.GetComponent<Image>();
            newObject.transform.SetParent(canvas.transform, false);
            createdObjects.Add(newObject);

            // Incrementam contorul pentru obiectele create
            objectsCreated++;
        }
        objectsCreated = 0;
        // Cat timp nu am atins numarul dorit de obiecte, continuam sa cream
        while (objectsCreated < itemNumber)
        {
            float randomX = Random.Range(-canvasWidth / 3f, canvasWidth / 3f);
            float randomY = Random.Range(-canvasHeight / 3f, canvasHeight / 3f);
            Vector3 position = new Vector3(randomX, randomY, 0);
            GameObject objectPrefab;
            int randomIndex = Random.Range(0, loadedSprites.Count);
            int luck = Random.Range(0, 10);
            
            if (luck > 3)
            {
                objectPrefab = dragObject;
            }
            else
            {
                objectPrefab = ratObject;
            }
            GameObject newObject = Instantiate(objectPrefab, position, Quaternion.identity);
            Image img = newObject.GetComponent<Image>();

            img.sprite = loadedSprites[randomIndex];
            newObject.transform.SetParent(canvas.transform, false);
            createdObjects.Add(newObject);

            // Incrementam contorul pentru obiectele create
            objectsCreated++;
        }
    }
    public void OnCanvasOpenButtonClicked()
    {
        if (!itemsGenerated)
            generateItems();
        itemsGenerated = true;
    }
}



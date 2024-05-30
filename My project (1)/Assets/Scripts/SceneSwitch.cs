using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{


    public int sceneBuildIndex; // o sa folosesc index in loc de string, daca ar fi nume, tre sa schimbi codu cand schimbi numele scenei

    [SerializeField] private Scene sceneToSwith;
   [SerializeField] private string switherName;

    private void Start()
    {
        switherName = gameObject.name.Split(' ')[1];
       


    }

    private void OnTriggerEnter2D(Collider2D collision)  // cu printuri pt debug
    {
        print("Trigger Enterd");
        if (collision.tag == "Player") 
        {
            if (SceneExists(switherName))
            {
                StartCoroutine(LoadSceneAsync());
            }
            else
            {
                Debug.LogError("Scena " + switherName + " nu există!");
            }
        }
    }

    private IEnumerator LoadSceneAsync()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(switherName, LoadSceneMode.Single); // Ii da load la scena in timp ce tu esti deja in scena initiala 

       
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        
        Debug.Log("Scena " + switherName + " a fost încărcată cu succes!");
    }

    private bool SceneExists(string sceneName) // verifica daca scena exista ca sa nu faca load la ceva ce nu exista
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string name = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (name == sceneName)
            {
                return true;
            }
        }
        return false;
    }
}

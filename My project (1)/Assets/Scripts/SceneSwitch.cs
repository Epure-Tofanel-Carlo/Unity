using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{


    public int sceneBuildIndex; // o sa folosesc index in loc de string, daca ar fi nume, tre sa schimbi codu cand schimbi numele scenei


    private void OnTriggerEnter2D(Collider2D collision)  // cu printuri pt debug
    {
        print("Trigger Enterd");

        if (collision.tag == "Player") 
        {
            print("Switching scene");
            SceneManager.LoadScene(sceneBuildIndex, LoadSceneMode.Single) ; // sa fie o singura scena at a time
        }
    }
}

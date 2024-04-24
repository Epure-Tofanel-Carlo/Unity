using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{


    public string sceneBuildName; // o sa folosescstring


    private void OnTriggerEnter2D(Collider2D collision)  // cu printuri pt debug
    {
        print("Trigger Enterd");

        if (collision.tag == "Player") 
        {
            print("Switching scene");
            SceneManager.LoadScene(sceneBuildName, LoadSceneMode.Single) ; // sa fie o singura scena at a time
        }
    }
}

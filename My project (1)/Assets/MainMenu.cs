using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); // daca aleg play, ma baga in scena 1 care este de fapt jocul
    }

    public void Quit() 
    {
        Debug.Log("QUIT"); // pun un log ca sa vad daca functioneaza butonul
        Application.Quit(); 
    }
}

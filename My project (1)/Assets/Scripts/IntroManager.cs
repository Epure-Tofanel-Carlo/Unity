using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    public GameObject introImage;       // Referinta la obiectul Image pentru introducere
    public GameObject joacaResponsabil; // Referinta la GameObject-ul care contine elementele jocului

    void Start() //initial intro-ul e activ si jocul in sine nu 
    {
        introImage.SetActive(true);
        joacaResponsabil.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // daca apas e, intro-ul devine inactiv si se activeaza jocul in sine
        {
            introImage.SetActive(false);
            joacaResponsabil.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // daca apas esc, ies din slot machine si intru in scena Taverna
        {
            SceneManager.LoadScene("Tavern");
        }
    }

}

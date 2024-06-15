using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasileInteraction : MonoBehaviour
{
    public GameObject dialogVasile; // Referinta la panelul de dialog
    public Canvas info; // Referinta la panelul de informatii
    private bool playerInRange;

    private void Start()
    {
        dialogVasile.SetActive(false); // Asigura-te ca panelul de dialog este ascuns la inceput
        info.enabled = false; // Asigura-te ca panelul de informatii este ascuns la inceput
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogVasile.SetActive(true);
            info.enabled = false; // Ascunde info cand dialogul incepe
            // Logica pentru a incepe dialogul
            dialogVasile.GetComponent<DialogVasile>().StartDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            info.enabled = true; // Afiseaza info cand jucatorul este in apropiere
            // Afiseaza un mesaj sau un indicator pentru a apasa tasta E
            Debug.Log("Press E to interact");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogVasile.SetActive(false); // Ascunde dialogul cand jucatorul iese din zona de trigger
            info.enabled = false; // Ascunde info cand jucatorul iese din zona de trigger
        }
    }
}
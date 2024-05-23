using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Row : MonoBehaviour
{
    private int randomValue;
    private float timeInterval;

    public bool rowStopped; 
    public string stoppedSlot; 

    void Start()
    {
        rowStopped = true; // Initial, randul este oprit
        GameControl.HandlePulled += StartRotating; 
    }

    private void StartRotating()
    {
        stoppedSlot = ""; // Reseteaza simbolul oprit
        StartCoroutine("Rotate"); 
    }

    // Functia de rotatie, calculeaza pozitia folosind grade
    private IEnumerator Rotate()
    {
        rowStopped = false; // Seteaza flag-ul pentru a indica ca randul se roteste
        timeInterval = 0.025f; 

        // Roteste randul pentru un numar initial de iteratii
        for (int i = 0; i < 30; i++)
        {
            if (transform.position.y <= -3.5f)
                transform.position = new Vector2(transform.position.x, 1.75f);

            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);
            yield return new WaitForSeconds(timeInterval);
        }

        randomValue = Random.Range(60, 100);

        // Ajusteaza randomValue pentru a se asigura ca se opreste pe un simbol valid
        switch (randomValue % 3)
        {
            case 1:
                randomValue += 2;
                break;
            case 2:
                randomValue += 1;
                break;
        }

        // Continua rotatia randului pentru randomValue iteratii
        for (int i = 0; i < randomValue; i++)
        {
            if (transform.position.y <= -3.5f)
                transform.position = new Vector2(transform.position.x, 1.75f);

            transform.position = new Vector2(transform.position.x, transform.position.y - 0.25f);

            // Mareste treptat intervalul de timp pentru a incetini rotatia
            if (i > Mathf.RoundToInt(randomValue * 0.25f))
                timeInterval = 0.05f;
            if (i > Mathf.RoundToInt(randomValue * 0.5f))
                timeInterval = 0.1f;
            if (i > Mathf.RoundToInt(randomValue * 0.75f))
                timeInterval = 0.15f;
            if (i > Mathf.RoundToInt(randomValue * 0.95f))
                timeInterval = 0.2f;

            yield return new WaitForSeconds(timeInterval);
        }

        // Stabileste simbolul la care s-a oprit randul in functie de pozitia finala
        if (transform.position.y == -3.5f)
            stoppedSlot = "Diamond";
        else if (transform.position.y == -2.75f)
            stoppedSlot = "Crown";
        else if (transform.position.y == -2f)
            stoppedSlot = "Melon";
        else if (transform.position.y == -1.25f)
            stoppedSlot = "Bar";
        else if (transform.position.y == -0.5f)
            stoppedSlot = "Seven";
        else if (transform.position.y == 0.25f)
            stoppedSlot = "Cherry";
        else if (transform.position.y == 1f)
            stoppedSlot = "Lemon";
        else if (transform.position.y == 1.75f)
            stoppedSlot = "Diamond";

        rowStopped = true; 
    }

    private void OnDestroy()
    {
        GameControl.HandlePulled -= StartRotating; 
    }
}

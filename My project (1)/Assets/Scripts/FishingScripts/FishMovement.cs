using GamePlay;
using Inventory.Model;
using System.Collections;
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    
    public float speed = 0.4f;
    public float maxRange = 15f; // Distanța maximă de mișcare la stânga și la dreapta
    public float minRange = 5f;  // Distanța minimă de mișcare la stânga și la dreapta

    private Vector3 initialPosition;
    public Vector3 startPozition;
    private Vector3 targetPosition;
    private float journeyTime;
    private float elapsedTime;
    [SerializeField] private float score = 0;
    private int p = 1;
    private bool miniGameStatus;

    [SerializeField] private InventorySO inventory;
    [SerializeField] private ItemSO peste;

    [SerializeField] private GameObject canvasMiniGame;
    [SerializeField] private GameObject player;

    void Start()
    {
        
      //  startMiniGame();
    }


    public void startMiniGame()
    {
        if(startPozition == Vector3.zero)
        {
            startPozition = transform.position;
        }
        
        if (!miniGameStatus)
        {
            player.GetComponent<PlayerMovement>().enabled = false; 
            initialPosition = startPozition;
            miniGameStatus = true;
            StartCoroutine(MoveRandomly());
        }
        
    }

    System.Collections.IEnumerator MoveRandomly()
    {
        while (miniGameStatus)
        {
            // Alege o nouă poziție țintă random în intervalul specificat, asigurându-te că mișcarea este în limitele permise
            float targetX;

            if(score > 10 * p)
            {
                initialPosition.y += 1;
                p++;
            }
            do
            {
                targetX = initialPosition.x + Random.Range(-maxRange, maxRange);
            } while (Mathf.Abs(targetX - transform.position.x) < minRange);

            targetPosition = new Vector3(targetX, initialPosition.y, initialPosition.z);

            // Calcularea duratei de mișcare pe baza vitezei și distanței
            journeyTime = Vector3.Distance(transform.position, targetPosition) / speed;
            elapsedTime = 0f;
          
            // Mișcă peștele către poziția țintă
            while (elapsedTime < 2)
            {
                //Debug.Log(journeyTime + " " + elapsedTime);
                transform.position = Vector3.Lerp(transform.position, targetPosition, elapsedTime / journeyTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            //Debug.Log(targetX +" "+ transform.position);
            // Asigură-te că peștele ajunge exact la poziția țintă
            transform.position = targetPosition;
             if(score >= 100)
            {
                prindePeste();
            }
        }
    }

    private void prindePeste()
    {
        player.GetComponent<PlayerMovement>().enabled = true;
        miniGameStatus = false;
        p = 1;
        score = 0;
        canvasMiniGame.SetActive(false);
        inventory.AddItem(peste, 1, null);

    }

   
    private void OnMouseOver()
    {
        score += 0.1f;
        
    }
    
}

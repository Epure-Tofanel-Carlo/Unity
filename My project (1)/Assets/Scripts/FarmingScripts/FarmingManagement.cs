using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingManagement : MonoBehaviour
{
    bool isPlanted = false;
    bool playerInTrigger = false;
    public SpriteRenderer plant;
    public Sprite[] plantStages;
    int plantStage = 0;
    float timeBetweenStages = 2f;
    float timer;
    [SerializeField]
    private InventorySO inventoryData;
    [SerializeField]
    private GameObject droppedItemPrefab;
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    void Start()
    {
       
    }

    void Update()
    {
        if (playerInTrigger && Input.GetKeyDown(KeyCode.E)) //daca e apasata tasta E plantez 
        {
            OnEDownEvent();
        }

        if (isPlanted)
        {
            timer -= Time.deltaTime; // scadem timerul 
            if (timer < 0 && plantStage < plantStages.Length - 1)
            {
                timer = timeBetweenStages;
                plantStage++; // trecem la urmatorul pas de crestere
                UpdatePlant();
            }
        }
    }

    private void OnEDownEvent()
    {
        if (isPlanted) //daca e plantata
        {
            if (plantStage == plantStages.Length - 1) //daca e ajunsa in starea finala
            {
                Harvest(); //o culegem
            }
        }
        else
        {   //altfel o plantam
            Plant();
        }
    }

    public void Harvest()
    {
        Debug.Log("Harvested");
        isPlanted = false;
        plant.gameObject.SetActive(false);
        inventoryData.AddItem(InventoryItem,1,InventoryItem.DefaultParametersList);
    
    }

    public void Plant()
    {
        Debug.Log("Planted");
        isPlanted = true;
        plantStage = 0;
        UpdatePlant();
        timer = timeBetweenStages;
        plant.gameObject.SetActive(true);
    }

    void UpdatePlant()
    {
        plant.sprite = plantStages[plantStage];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    // pentru colliderul 2d
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}

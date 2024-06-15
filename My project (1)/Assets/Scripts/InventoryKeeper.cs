using Inventory.Model;
using UnityEditor;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private InventorySO playerInventory;
    private bool isInitialized = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // este dontdestroyonload strict ca sa mentina datele din inventar la schimbul intre scene
            LoadInventory();
        }
    }

    private void LoadInventory() // functie care imi cauta in proiect inventarul si il incarca in joc 
    {
        playerInventory = AssetDatabase.LoadAssetAtPath<InventorySO>("Assets/Data/PlayerInventory.asset");
        if (playerInventory == null)
        {
            Debug.LogError("Failed to load PlayerInventory from Resources.");
        }
    }

    public InventorySO GetInventoryData() // intoarce inventarul
    {
        return playerInventory;
    }


    public void AddItemToInventory(InventoryItem item)
    {
        playerInventory.AddItem(item);
    }
    public void setInitialised()
    {
        isInitialized = true;
    }
}

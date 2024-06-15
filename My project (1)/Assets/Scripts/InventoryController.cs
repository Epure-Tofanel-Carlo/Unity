using InventoryUI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;
using System.Text;
using UnityEditorInternal.Profiling.Memory.Experimental;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private InventoryPageScript inventoryUI; // the inventory page

        private InventorySO inventoryData;

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip dropClip;
        [SerializeField]
        private GameObject itemPrefab;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        //Functia de start care adauga structura de date unde sunt tinute itemele din inventar
        private void Start()
        {
            inventoryData  = InventoryManager.Instance.GetInventoryData();
            PrepareUI();
            PrepareInventoryData();
        }
        //Functie care initializeaza inventarul si adauga itemele necesare in caz de exista
        private void PrepareInventoryData()
        {
            //inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            if (inventoryData.inventoryItems[0].Item == null) // initial items doar daca e prima data cand inventarul este creat
            {
                foreach (InventoryItem item in initialItems)
                {
                    if (item.isEmpty)
                    {
                        continue;
                    }
                    inventoryData.AddItem(item);
                }
                InventoryManager.Instance.setInitialised();
            }
        }
        //Functia care updateaza partea UI a inventarului cand o modificare a aparut la nivelul inventarului
        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            if (inventoryUI != null)
            {
                inventoryUI.ResetAllItems();
                foreach (var item in inventoryState)
                {
                    inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.itemQuantity);
                }
            }
        }

        //Functia care pregateste partea de UI
        private void PrepareUI()
        {
            if (inventoryUI != null)
            {
                inventoryUI.InitializeInventoryUI(inventoryData.Size);
                // Add the events for the inventory UI
                inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
                inventoryUI.OnHoverDescription += RequestHoverDescription;
                inventoryUI.OnSwapItems += HandleSwapItems;
                inventoryUI.OnStartDragging += HandleDragging;
                inventoryUI.OnItemActionRequested += HandleItemActionRequest;
            }
        }
        private void HandleDescriptionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(itemIndex);
            if (inventoryItem.isEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            ItemSO item = inventoryItem.Item;
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(itemIndex,
                item.name, description);
        }
        //Functie care creeaza descrierea unui obiect in joc prin intermediul unui StringBuilder
        private string PrepareDescription(InventoryItem inventoryItem)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(inventoryItem.Item.Description);
            stringBuilder.AppendLine();
            for (int i = 0; i < inventoryItem.itemState.Count; i++) 
            {
                stringBuilder.Append($"{inventoryItem.itemState[i].itemParameter.ParameterName} "  + $": {inventoryItem.itemState[i].value} / " + $"{inventoryItem.Item.DefaultParametersList[i].value}");
                // Durability : 20 / 100
                stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }
        //Functia care descrie evenimentul de hover asupra unui obiect
        public void RequestHoverDescription(int index)
        {
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(index);
            if (inventoryItem.isEmpty)
            {
                ClearHoverDescription();
                return;
            }
            string description = PrepareDescription(inventoryItem);
            inventoryUI.UpdateDescription(index, inventoryItem.Item.Name, description);
        }

        public void ClearHoverDescription()
        {
            inventoryUI.DisableDescription();
        }
        //Functia care actioneaza actiunea dorita de jucator
        private void PerformAction(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(itemIndex);
            if (inventoryItem.isEmpty)
            {
                return;
            }
            IItemAction itemAction = inventoryItem.Item as IItemAction;
            if (itemAction != null)
            {
                itemAction.PerformAction(gameObject, inventoryItem.itemState);
                audioSource.PlayOneShot(itemAction.actionSFX);
            }
            IDestroyableItem destroyableItem = inventoryItem.Item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryData.RemoveItem(itemIndex, 1);
            }
        }
        //Functia care verifica ce actiuni poate sa aiba un obiect din inventar si sa il afiseze jucatorului
        public void HandleItemActionRequest(int itemIndex)
        {
            Debug.Log("HandleItemActionRequest called for item index: " + itemIndex);
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(itemIndex);
            if (inventoryItem.isEmpty)
            {
                Debug.Log("Item is empty");
                return;
            }
            

            IItemAction itemAction = inventoryItem.Item as IItemAction;
            if (itemAction != null)
            {
                Debug.Log("Item action found: " + itemAction.actionName);
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.actionName, () => PerformAction(itemIndex));
            }
            else
            {
                Debug.LogWarning("No item action found");
                inventoryUI.ShowItemAction(itemIndex);
            }
            IDestroyableItem destroyableItem = inventoryItem.Item as IDestroyableItem;
            if (destroyableItem != null)
            {
                Debug.Log("Destroyable item found");
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.itemQuantity));
            }
        }
        //Functia prin care se da drop la item pe jos de catre player
        private void DropItem(int itemIndex, int itemQuantity)
        {
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(itemIndex);
            if (!inventoryItem.isEmpty)
            {
                GameObject droppedItem = Instantiate(itemPrefab, CalculateDropPosition(), Quaternion.identity);
                Item itemComponent = droppedItem.GetComponent<Item>();
                if (itemComponent != null)
                {
                    itemComponent.SetItemDetails(inventoryItem.Item, itemQuantity);
                }
                else
                {
                    Debug.LogWarning("The item is already destroyed.");
                }
                // Remove the item from the inventory
                inventoryData.RemoveItem(itemIndex, itemQuantity);
                inventoryUI.ResetSelection();
                audioSource.PlayOneShot(dropClip);
            }
            else
            {
                Debug.LogWarning("Attempted to drop an item that is either null or empty.");
            }
        }
        //Functie utilitara care calculeaza pozitia unde obiectul va fi creat
        private Vector3 CalculateDropPosition()
        {
            // Example: Place the item 1.5 meters in front of the player
            Transform playerTransform = this.transform; // Assuming the script is attached to the player
            Vector3 dropPosition = playerTransform.position + playerTransform.forward * 1.5f;
            return dropPosition;
        }

        //Functia care descrie felul in care draggingul este implementat

        private void HandleDragging(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(itemIndex);
            if(inventoryItem.Item.ItemImage == null)
            {
                Debug.LogError("sprite is null.");
            }
            if(inventoryItem.isEmpty)
            {
                return;
            }
            inventoryUI.CreateDraggedItem(inventoryItem.Item.ItemImage, inventoryItem.itemQuantity);
        }

        private void HandleSwapItems(int itemIndex1, int itemIndex2)
        {
            inventoryData.SwapItems(itemIndex1, itemIndex2);
        }
        //Functie care verifica daca jucatorul doreste sa vad inventarul
        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (inventoryUI.isActiveAndEnabled == false)
                {
                    inventoryUI.Show();
                    foreach (var item in inventoryData.GetCurrentInventoryState())
                    {
                        // Add the items from the data structure in the UI
                        inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.itemQuantity);
                    }
                }
                else
                {
                    inventoryUI.Hide();
                }
            }
        }
    }
}
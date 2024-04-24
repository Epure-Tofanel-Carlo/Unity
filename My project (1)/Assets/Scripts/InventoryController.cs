using InventoryUI;
using Inventory.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using static UnityEditor.Progress; // am pus comment ca dadea fail la build
using System.Text;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField]
        private InventoryPageScript inventoryUI; // the inventory page

        [SerializeField]
        private InventorySO inventoryData; // the data structure that contains the "objects"

        [SerializeField]
        private AudioSource audioSource;
        [SerializeField]
        private AudioClip dropClip;
        [SerializeField]
        private GameObject itemPrefab;

        public List<InventoryItem> initialItems = new List<InventoryItem>();

        private void Start()
        {
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;
            foreach (InventoryItem item in initialItems)
            {
                if(item.isEmpty)
                {
                    continue;
                }
                inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();
            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(item.Key, item.Value.Item.ItemImage, item.Value.itemQuantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);
            // Add the events for the inventory UI
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnHoverDescription += RequestHoverDescription;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
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

        public void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemByIndex(itemIndex);
            if (inventoryItem.isEmpty)
            {
                return;
            }

            IItemAction itemAction = inventoryItem.Item as IItemAction;
            if (itemAction != null)
            {
                inventoryUI.ShowItemAction(itemIndex);
                inventoryUI.AddAction(itemAction.actionName, () => PerformAction(itemIndex));
            }
            IDestroyableItem destroyableItem = inventoryItem.Item as IDestroyableItem;
            if (destroyableItem != null)
            {
                inventoryUI.AddAction("Drop", () => DropItem(itemIndex, inventoryItem.itemQuantity));
            }
        }

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

        private Vector3 CalculateDropPosition()
        {
            // Example: Place the item 1.5 meters in front of the player
            Transform playerTransform = this.transform; // Assuming the script is attached to the player
            Vector3 dropPosition = playerTransform.position + playerTransform.forward * 1.5f;
            return dropPosition;
        }



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
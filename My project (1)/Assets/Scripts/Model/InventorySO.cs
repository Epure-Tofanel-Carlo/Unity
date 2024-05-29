using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        public List<InventoryItem> inventoryItems;
        [field: SerializeField]
        public int Size { get; private set; } = 10;
        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();
            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }
        public int AddItem(ItemSO item, int quantity,List<ItemParameter> itemstate)
        {
            if(item.isStackable == false)
            {
                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    while(quantity > 0 && IsInventoryFull() == false) // add every unstackable item, example 5 swords
                    {
                        quantity -= AddItemtoFirstFreeSlot(item, 1, itemstate); // we can only add just 1 item at a time

                    }
                    InformAboutChange();
                    return quantity;
                }
            }
            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemtoFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemstate = null)
        {
            InventoryItem newItem = new InventoryItem
            {
                Item = item,
                itemQuantity = quantity,
                itemState = new List<ItemParameter>(itemstate == null ? item.DefaultParametersList : itemstate),
            };
            // search for an empty space
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }
            return 0;
        }
        //function that search if does not exists anymore any empty slot
        private bool IsInventoryFull() => inventoryItems.Where(item => item.isEmpty).Any() == false;

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    continue;
                }
                if (inventoryItems[i].Item.ID == item.ID)
                {
                    int amountPossibleToTake = inventoryItems[i].Item.MaxStackSize - inventoryItems[i].itemQuantity; // we need to see how many items we can stack on the current item in the inventory
                    if(quantity > amountPossibleToTake)
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].Item.MaxStackSize); // maxed out the capacity of the slot
                        quantity -= amountPossibleToTake;
                    }
                    else
                    {
                        inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].itemQuantity + quantity);
                        InformAboutChange();
                        return 0;
                    }
                }
            }
            while(quantity > 0 && IsInventoryFull() == false)
            {
                int newQuantity = Math.Clamp(quantity,0, item.MaxStackSize);
                quantity -= newQuantity;
                AddItemtoFirstFreeSlot(item, newQuantity);
            }
            return quantity;
        }

        // to update the inventory UI with the items that exists
        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> returnvalue = new Dictionary<int, InventoryItem>();
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].isEmpty)
                {
                    continue;
                }
                returnvalue[i] = inventoryItems[i];
            }
            return returnvalue;
        }
        public InventoryItem GetItemByIndex(int itemIndex)
        {
            return inventoryItems[itemIndex];
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.Item, item.itemQuantity,item.itemState);
        }

        public void SwapItems(int itemIndex1, int itemIndex2)
        {
            InventoryItem item1 = inventoryItems[itemIndex1];
            inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
            inventoryItems[itemIndex2] = item1;
            InformAboutChange();
        }
        public void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if(inventoryItems.Count > itemIndex)
            {
                if (inventoryItems[itemIndex].isEmpty)
                {
                    return;
                }
                int reminder = inventoryItems[itemIndex].itemQuantity - amount;
                if(reminder <= 0)
                {
                    inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
                }
                else
                {
                    inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(reminder);
                }
                InformAboutChange();
            }
        }
    }
    // struct so the change is more secure of an inventoryItem from the list, because to change a value is necesarry to use the Inventory ref from the stack
    [Serializable]
    public struct InventoryItem
    {
        public int itemQuantity;
        public ItemSO Item;
        public List<ItemParameter> itemState;
        private int quantityToSplit;

        public InventoryItem(ItemSO item, int quantityToSplit) : this()
        {
            Item = item;
            this.quantityToSplit = quantityToSplit;
        }

        public bool isEmpty => Item == null;

        public InventoryItem ChangeQuantity(int newItemQuantity)
        {
            return new InventoryItem
            {
                Item = this.Item,
                itemQuantity = newItemQuantity,
                itemState = new List<ItemParameter>(this.itemState),
            };
        }
        public static InventoryItem GetEmptyItem() => new InventoryItem
        {
            Item = null,
            itemQuantity = 0,
            itemState = new List<ItemParameter>(),
        };
    }
}

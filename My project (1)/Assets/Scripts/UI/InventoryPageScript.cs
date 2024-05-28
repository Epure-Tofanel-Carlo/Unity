using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InventoryUI
{
    public class InventoryPageScript : MonoBehaviour
    {
        [SerializeField]
        private UIInventoryItem itemPrefab;
        [SerializeField]
        private UIInventoryDescription itemDescription;
        [SerializeField]
        private RectTransform contentpanel;
        [SerializeField]
        private MouseFollower mouseFollower;
        [SerializeField]
        private ItemActionPanel actionPanel;

        List<UIInventoryItem> items = new List<UIInventoryItem>();


        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging, OnHoverDescription, OnItemSplitRequested;
        public event Action<int, int> OnSwapItems;

        public void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }
        public void InitializeInventoryUI(int size_of_inventory)
        {
            for (int i = 0; i < size_of_inventory; i++)
            {
                UIInventoryItem item = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(contentpanel);
                items.Add(item);
                item.OnItemClicked += HandleItemSelection;
                item.OnItemBeginDrag += HandleBeginDrag;
                item.OnItemDroppedOn += HandleSwap;
                item.OnItemEndDrag += HandleEndDrag;
                item.OnRightMouseButtonClick += HandleShowItemActions;
                item.OnItemHoverEnter += HandleItemHoverEnter;
                item.OnItemHoverExit += HandleItemHoverExit;           
            }
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            int index = items.IndexOf(item);
            if (index == -1)
            {
                return;
            }
            OnDescriptionRequested?.Invoke(index);
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            int index = items.IndexOf(item);
            // the index is in the items list
            if (index == -1)
            {
                return;
            }
            currentlyDraggedItemIndex = index;
            HandleItemSelection(item);
            OnStartDragging?.Invoke(index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            // activate the mouse follower with the item's image and quantity
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        private void HandleSwap(UIInventoryItem item)
        {
            int index = items.IndexOf(item);
            // the index is in the items list
            if (currentlyDraggedItemIndex == -1)
            {
                return;
            }
            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
            HandleItemSelection(item);
        }

        private void ResetDraggedItem()
        {
            // reset the dragging logic
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        public void UpdateData(int itemIndex, Sprite itemImage, int itemQuantity)
        {
            if (items.Count > itemIndex)
            {
                items[itemIndex].SetData(itemImage, itemQuantity);
            }
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = items.IndexOf(item);
            if (index != -1)
            {
                OnHoverDescription?.Invoke(index);
            }
            OnItemActionRequested?.Invoke(index);
        }

        public void DisableDescription()
        {
            itemDescription.enabled = false;
        }
        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }
        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeSelectAllItems();
        }

        public void AddAction(string actionName, Action performAction)
        {
            actionPanel.AddButton(actionName, performAction); // add different action options to the panel
        }

        public void ShowItemAction(int itemIndex)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = items[itemIndex].transform.position; // the action panel to be on the item we want to perform an action
        }

        private void DeSelectAllItems()
        {
            foreach (UIInventoryItem item in items)
            {
                item.DeSelect();
            }
            actionPanel.Toggle(false);
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            itemDescription.Hide();
            ResetSelection();
        }

        internal void UpdateDescription(int itemIndex, string name, string description)
        {
            itemDescription.SetDescription(name, description);
            DeSelectAllItems();
            items[itemIndex].Select();
            itemDescription.Show();
        }
        private void HandleItemHoverEnter(UIInventoryItem item)
        {
            int index = items.IndexOf(item);
            if (index != -1)
            {
                OnHoverDescription?.Invoke(index);
            }
        }

        private void HandleItemHoverExit(UIInventoryItem item)
        {
            itemDescription.Hide();
           // itemDescription.ResetDescription();
        }

        public void ResetAllItems()
        { 
            foreach (var item in items)
            {
                //Reset and Unselect the item
                item.ResetData();
                item.DeSelect();
            }
        }
    }
}
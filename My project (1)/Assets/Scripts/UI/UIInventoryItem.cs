using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace InventoryUI
{
    public class UIInventoryItem : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler, IDropHandler, IDragHandler, IPointerEnterHandler
    {
        [SerializeField]
        private Image itemImage;
        [SerializeField]
        private TMP_Text quantityText;
        [SerializeField]
        private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked, OnItemDroppedOn, OnItemBeginDrag, OnItemEndDrag, OnRightMouseButtonClick, OnItemHoverEnter, OnItemHoverExit; // events for left click, right click, and dragging the item with the mouse
        private bool isEmpty = true;

        public void Awake()
        {
            ResetData();
            DeSelect();
        }

        public void DeSelect()
        {
            borderImage.enabled = false;
        }

        public void ResetData()
        {
            itemImage.gameObject.SetActive(false); //Empty Slot
            quantityText.text = string.Empty;
            isEmpty = true;
        }
        public void SetData(Sprite sprite, int quantity) // Adding an item to the slot
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityText.text = quantity + "";
            isEmpty = false;
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnItemHoverEnter?.Invoke(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnItemHoverExit?.Invoke(this);
        }

        public void OnPointerClick(PointerEventData pointerEvent)
        {
            if (pointerEvent.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseButtonClick?.Invoke(this);
            }
            else
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (isEmpty)
            {
                return;
            }
            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}
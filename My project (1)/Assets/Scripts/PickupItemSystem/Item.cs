using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [field: SerializeField]
    public ItemSO InventoryItem {  get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private float duration = 0.3F;
    [SerializeField]
    public List<ItemParameter> parameters = new List<ItemParameter>();

    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = InventoryItem.ItemImage;
    }
    internal void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }
    private IEnumerator AnimateItemPickup()
    {
        audioSource.Play();
        Vector3 startScale = transform.localScale;
        Vector3 endScale = transform.localScale;
        float currenttime = 0;
        while (currenttime < duration) 
        {
            currenttime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale, endScale, currenttime / duration);
            yield return null;
        }
        transform.localScale = startScale;
        Destroy(gameObject);
    }
    public void SetItemDetails(ItemSO inventoryItem, int itemQuantity)
    {
        InventoryItem = inventoryItem;
        Quantity = itemQuantity;
        parameters = inventoryItem.DefaultParametersList;
    }
}

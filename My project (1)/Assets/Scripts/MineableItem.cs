using Inventory.Model;
using System.Collections;
using UnityEngine;

public class MineableItem : MonoBehaviour
{
    [field: SerializeField]
    public MineableItemSO MineableItemData { get; private set; }

    [SerializeField]
    private GameObject droppedItemPrefab;

    [SerializeField]
    private AudioSource audioSource;

    private bool isMining = false;

    private void OnMouseDown()
    {
        Debug.Log("A inceput minarea");
        if (!isMining)
        {
            StartCoroutine(MineItem());
        }
    }

    private IEnumerator MineItem()
    {
        isMining = true;
        yield return new WaitForSeconds(MineableItemData.MineTime);

        // Drop the item
        DropItem();

        // Optionally play a mining sound
        if (audioSource != null)
        {
            audioSource.Play();
        }

        Destroy(gameObject); // Destroy the mined object
    }

    private void DropItem()
    {
        GameObject droppedItem = Instantiate(droppedItemPrefab, transform.position, Quaternion.identity);
        Item itemComponent = droppedItem.GetComponent<Item>();
        if (itemComponent != null)
        {
            itemComponent.SetItemDetails(MineableItemData, 1);
        }
    }
}

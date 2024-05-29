using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class FurnaceItemSO : CraftingItemRecipeSO
{ 

    [field: SerializeField]
    public int bakingTime { get; set; } = 1; // Timpul necesar pentru a topi acest item
    public interface IDestroyableItem
    {

    }
}


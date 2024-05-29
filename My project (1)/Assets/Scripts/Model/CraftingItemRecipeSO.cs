using UnityEngine;
using System.Collections.Generic;
using Inventory.Model;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "Crafting/Crafting Recipe")]
public class CraftingItemRecipeSO : ScriptableObject
{
    public List<InventoryItem> ingredients;
    public InventoryItem result;
}

using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class CraftingController : MonoBehaviour
{
    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private CraftingPageScript craftingView;
    [SerializeField] private List<CraftingItemRecipeSO> craftingRecipes;

    private void Start()
    {
        if (craftingView == null)
        {
            Debug.LogError("CraftingView is not assigned in the inspector.");
            return;
        }

        if (craftingRecipes == null || craftingRecipes.Count == 0)
        {
            Debug.LogError("Crafting recipes are not assigned or empty.");
            return;
        }

        craftingView.InitializeCraftingUI(craftingRecipes);
        foreach (var slot in craftingView.GetRecipeSlots())
        {
            slot.OnRecipeClicked += HandleRecipeClicked;
        }
    }

    private void HandleRecipeClicked(UICraftingRecipe recipeUI)
    {
        CraftingItemRecipeSO recipe = recipeUI.GetRecipe();
        Debug.Log("Crafting");
        CraftItem(recipe);
    }

    public bool HasIngredients(CraftingItemRecipeSO recipe)
    {
        foreach (var ingredient in recipe.ingredients)
        {
            if (!HasItem(ingredient.Item, ingredient.itemQuantity))
            {
                return false;
            }
        }
        return true;
    }

    private bool HasItem(ItemSO item, int quantity)
    {
        int count = 0;
        foreach (var inventoryItem in inventoryData.inventoryItems)
        {
            if (inventoryItem.Item == item)
            {
                count += inventoryItem.itemQuantity;
                if (count >= quantity)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void CraftItem(CraftingItemRecipeSO recipe)
    {
        if (!HasIngredients(recipe))
        {
            Debug.Log("Nu exista ingredientele pentru aceasta reteta");
            return;
        }

        foreach (var ingredient in recipe.ingredients)
        {
            RemoveItem(ingredient.Item, ingredient.itemQuantity);
        }

        AddItem(recipe.result.Item, recipe.result.itemQuantity, recipe.result.itemState);
    }

    private void RemoveItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryData.inventoryItems.Count; i++)
        {
            if (inventoryData.inventoryItems[i].Item == item)
            {
                if (inventoryData.inventoryItems[i].itemQuantity > quantity)
                {
                    inventoryData.inventoryItems[i] = inventoryData.inventoryItems[i].ChangeQuantity(inventoryData.inventoryItems[i].itemQuantity - quantity);
                    break;
                }
                else
                {
                    quantity -= inventoryData.inventoryItems[i].itemQuantity;
                    inventoryData.inventoryItems[i] = InventoryItem.GetEmptyItem();
                }
            }
        }
        inventoryData.InformAboutChange();
    }

    private void AddItem(ItemSO item, int quantity, List<ItemParameter> itemState)
    {
        inventoryData.AddItem(item, quantity, itemState);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (craftingView.isActiveAndEnabled == false)
            {
                craftingView.Show();
            }
            else
            {
                craftingView.Hide();
            }
        }
    }
}

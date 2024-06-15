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
        //Functii care trateaza posibile erori
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
        //Daca nu s-a intamplat nicio eroare se initializeaza UI ul sistemului de craft si adauga fiecarei retete modalitatea in care se va tratata selectarea acesteia
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
    //Functie care verifica daca jucatorul are toate ingredientele necesare ca sa poate crea acel obiect
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
    //Functie ajutatoare pentru HasIngredients
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
    //Functia care trateaza cum jucatorul va crea acel obiect
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
    //Eliminarea obiectelor care au fost folosite
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
    //Adaugarea obiectului creat in inventar
    private void AddItem(ItemSO item, int quantity, List<ItemParameter> itemState)
    {
        inventoryData.AddItem(item, quantity, itemState);
    }
    //Functie care verifica daca playerul a deschis pagina de craft
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

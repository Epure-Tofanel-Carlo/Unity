using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class CraftingPageScript : MonoBehaviour
{
    [SerializeField] 
    private UICraftingRecipe recipePrefab;
    [SerializeField] 
    private RectTransform contentPanel;
    [SerializeField] 
    private UIInventoryDescription itemDescription;

    private List<UICraftingRecipe> recipeSlots = new List<UICraftingRecipe>();

    //Functie care initializeaza pagina de crafting si aduaga modalitatea in care vor fi tratate evenimentele jucatorului
    public void InitializeCraftingUI(List<CraftingItemRecipeSO> recipes)
    {
        foreach (var recipe in recipes)
        {
            UICraftingRecipe recipeSlot = Instantiate(recipePrefab, Vector3.zero, Quaternion.identity);
            recipeSlot.transform.SetParent(contentPanel);
            recipeSlots.Add(recipeSlot);
            recipeSlot.SetRecipe(recipe);
            recipeSlot.OnRecipeHoverEnter += HandleRecipeHoverEnter;
            recipeSlot.OnRecipeHoverExit += HandleRecipeHoverExit;
        }
    }

    //Functie care trateaza evenimentul de hover asupra unei retete de catre jucator
    private void HandleRecipeHoverEnter(UICraftingRecipe recipeUI)
    {
        CraftingItemRecipeSO recipe = recipeUI.GetRecipe();
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Required Items:");
        foreach (var ingredient in recipe.ingredients)
        {
            sb.AppendLine($"{ingredient.itemQuantity} x {ingredient.Item.Name}");
        }
        itemDescription.SetDescription(recipe.result.Item.Name, sb.ToString());
        itemDescription.Show();
    }

    public List<UICraftingRecipe> GetRecipeSlots()
    {
        return recipeSlots;
    }

    private void HandleRecipeHoverExit(UICraftingRecipe recipeUI)
    {
        itemDescription.Hide();
    }
    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        itemDescription.Hide();
    }
}

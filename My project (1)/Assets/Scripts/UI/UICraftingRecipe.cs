using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICraftingRecipe : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Image itemImage;

    private CraftingItemRecipeSO recipe;
    public event System.Action<UICraftingRecipe> OnRecipeHoverEnter, OnRecipeHoverExit, OnRecipeClicked;

    public void SetRecipe(CraftingItemRecipeSO newRecipe)
    {
        recipe = newRecipe;

        if (recipe == null)
        {
            Debug.LogError("Recipe is null");
            return;
        }

        if (recipe.result.isEmpty)
        {
            Debug.LogError("Recipe result is null");
            return;
        }

        if (recipe.result.Item.ItemImage == null)
        {
            Debug.LogError("Recipe result's itemImage is null");
            return;
        }

        itemImage.sprite = recipe.result.Item.ItemImage;
    }

    public CraftingItemRecipeSO GetRecipe()
    {
        return recipe;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Recipe clicked: " + recipe.result.Item.Name);
        OnRecipeClicked?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnRecipeHoverEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnRecipeHoverExit?.Invoke(this);
    }
}

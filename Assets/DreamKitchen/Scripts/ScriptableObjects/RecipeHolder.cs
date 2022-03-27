using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeHolder : MonoBehaviour
{
    [SerializeField] private Recipe scriptableRecipe;
    
    [SerializeField] private List<IngredientHolder> liveRecipe = new List<IngredientHolder>();
        
    private IngredientHolder tempIngredientStruct;
    
    void Start()
    {
        PopulateLiveRecipe();
    }

    void PopulateLiveRecipe()
    {
        liveRecipe.Clear();
        
        for (int i = 0; i < scriptableRecipe.ingredientList.Count; i++)
        {
            liveRecipe.Add(new IngredientHolder());
            tempIngredientStruct = liveRecipe[i];
            tempIngredientStruct.scriptableIngredient = scriptableRecipe.ingredientList[i];
            liveRecipe[i] = tempIngredientStruct;
        }
    }

    public Recipe GetCurrentRecipe()
    {
        return scriptableRecipe;
    }

    //Sets the current recipe and populates the list with the ingredient information
    public void SetCurrentRecipe(Recipe recipe)
    {
        scriptableRecipe = recipe;
        PopulateLiveRecipe();
    }
}

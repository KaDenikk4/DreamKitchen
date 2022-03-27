using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject
{
    public string recipeName;
    public string recipeDescription;
    public Sprite recipeIcon;
    public int    recipePrice; 
    public List<Ingredient> ingredientList = new List<Ingredient>();
}

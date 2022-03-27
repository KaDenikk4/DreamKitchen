using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "ScriptableObjects/Ingredient", order = 1)]
public class Ingredient : ScriptableObject
{
    public string   ingredientName;
    public Sprite   ingredientIcon;
    public Sprite   ingredientAsset;
    public string   ingredientWorkstation;
}

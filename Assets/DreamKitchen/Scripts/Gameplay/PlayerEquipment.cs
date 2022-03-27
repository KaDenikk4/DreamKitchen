using System;
using System.Collections.Generic;


[Serializable]
public struct PlayerEquipment
{
    public KitchenProduct equippedKnife;
    public KitchenProduct equippedOven;
    public List<Recipe> unlockedRecipes;
}

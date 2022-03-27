using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public List<KitchenProductSaveData> productDatabaseToSave = new List<KitchenProductSaveData>();
    //public List<RecipeData> recipeDatabaseToSave = new List<RecipeData>();

    public int standardCurrency;
    public int premiumCurrency;

    public string equippedKnife;
    public string equippedOven;
    public string equippedWallDecor;
    public string equippedFridge;
    public string equippedWindows;
    public string equippedCupboards;

}

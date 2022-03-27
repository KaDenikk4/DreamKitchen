using System;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "KitchenProduct", menuName = "ScriptableObjects/Kitchen Product", order = 1)]
public class KitchenProduct : ScriptableObject
{
    public string   productName;
    public Sprite   productSprite;
    public int      productPrice;
    public string   productDescription;
    public Sprite   productAsset;
}

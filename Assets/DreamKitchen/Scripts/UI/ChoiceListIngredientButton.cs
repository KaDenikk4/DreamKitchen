using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChoiceListIngredientButton : MonoBehaviour
{
    [SerializeField]
    public IngredientToWorkstationData thisButtonData;

    public void OpenCorrectWorkstationMinigame()
    {

        Guid orderID = thisButtonData.orderID;
        int ingredientIndex = thisButtonData.ingredientNumber;
        string requiredWorkstation = thisButtonData.requiredWorkstation;

        Order[] activeOrders = FindObjectsOfType<Order>();

        switch (requiredWorkstation)
        {
            case "CuttingBoard":
                FindObjectOfType<CuttingMinigame>().ProgressCooking();
                FindObjectOfType<CuttingMinigame>().ToggleMinigame();
                FindObjectOfType<CuttingMinigame>().SetIngredientNumberOnList(ingredientIndex);
                FindObjectOfType<CuttingMinigame>().SetIngredientOrderId(orderID.ToString());

                for (int i = 0; i < activeOrders.Length; i++)
                {
                    if (activeOrders[i].GetOrderId() == orderID.ToString())
                    {
                        activeOrders[i].ToggleOrderUI();
                    }
                }
                break;

            case "Hob":
                FindObjectOfType<TimedCooking>().ToggleMinigame();
                FindObjectOfType<TimedCooking>().SetIngredientNumberOnList(ingredientIndex);
                FindObjectOfType<TimedCooking>().SetIngredientsOrderId(orderID.ToString());
                for (int i = 0; i < activeOrders.Length; i++)
                {
                    if (activeOrders[i].GetOrderId() == orderID.ToString())
                    {
                        activeOrders[i].ToggleOrderUI();
                    }
                }
                break;

            case "Serving":

                break;

            default:
                break;

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]public struct IngredientToWorkstationData
{
    public Guid orderID;
    public int ingredientNumber;
    public string requiredWorkstation;
}
public class ModularWorkstationsForAllOrders : MonoBehaviour
{
    [SerializeField] private Order[] currentOrders;
    [SerializeField] private Button[] ChoiceList;
    [SerializeField] private ChoiceListIngredientButton[] ChoiceListData;

    int index = 0;

    private void Start()
    {
        currentOrders = FindObjectsOfType<Order>(); // orders array
    }

    private void Update()
    {
        //SetupChoiceOption();
    }

    public void FIndAllHobIngredients() // finding all the hob ingredients
    {
        currentOrders = FindObjectsOfType<Order>();
        IngredientToWorkstationData tempStruct = new IngredientToWorkstationData();
        ResetChoiceList();
        for(int i = 0; i < currentOrders.Length; i++) // going through orders
        {
            for(int j = 0; j < currentOrders[i].GetIngredients().Length; j++) // going through ingredients
            {
                if(currentOrders[i].GetIngredients()[j].GetThisButtonPrepMinigame() == "Hob") // checking which has hob preparation way
                {
                    int tempIngredientNumber = j;
                    ChoiceList[index].image.sprite = currentOrders[i].GetIngredients()[tempIngredientNumber].GetIngredientImage(); //set ingredient images

                    tempStruct.orderID = currentOrders[i].GetOrderGuid(); // setting id
                    tempStruct.ingredientNumber = currentOrders[i].GetIngredients()[tempIngredientNumber].getIngredientNumber(); // ingredients number
                    tempStruct.requiredWorkstation = currentOrders[i].GetIngredients()[tempIngredientNumber].GetThisButtonPrepMinigame(); // required workstation 

                    ChoiceListData[index].thisButtonData = tempStruct;

                    if (currentOrders[i].GetIngredients()[tempIngredientNumber].getGraded() == false) // checking wether can interact with the list
                        ChoiceList[index].interactable = true;
                    else
                        ChoiceList[index].interactable = false;
                    if (index < 11)
                    index++;
                }
            }
        }
        for(int i = 0; i<ChoiceList.Length; i++)
        {
            if(ChoiceList[i].image.sprite == null)
            {
                ChoiceList[i].gameObject.SetActive(false);
            }
        }
    }

    public void FIndAllCuttingBoardIngredients()
    {
        currentOrders = FindObjectsOfType<Order>();
        IngredientToWorkstationData tempStruct = new IngredientToWorkstationData();
        ResetChoiceList();
        for (int i = 0; i < currentOrders.Length; i++)
        {
            for (int j = 0; j < currentOrders[i].GetIngredients().Length; j++)
            {
                if (currentOrders[i].GetIngredients()[j].GetThisButtonPrepMinigame() == "CuttingBoard")
                {
                    int tempIngredientNumber = j;
                    ChoiceList[index].image.sprite = currentOrders[i].GetIngredients()[tempIngredientNumber].GetIngredientImage();

                    tempStruct.orderID = currentOrders[i].GetOrderGuid();
                    tempStruct.ingredientNumber = currentOrders[i].GetIngredients()[tempIngredientNumber].getIngredientNumber();
                    tempStruct.requiredWorkstation = currentOrders[i].GetIngredients()[tempIngredientNumber].GetThisButtonPrepMinigame();

                    ChoiceListData[index].thisButtonData = tempStruct;

                    if (currentOrders[i].GetIngredients()[tempIngredientNumber].getGraded() == false)
                        ChoiceList[index].interactable = true;
                    else
                        ChoiceList[index].interactable = false;
                    if (index < 11)
                        index++;
                }
            }
        }
        for (int i = 0; i < ChoiceList.Length; i++)
        {
            if (ChoiceList[i].image.sprite == null)
            {
                ChoiceList[i].gameObject.SetActive(false);
            }
        }
    }

    public void ResetChoiceList()//reset
    {
        IngredientToWorkstationData tempStruct = new IngredientToWorkstationData();
        index = 0;
        for (int i = 0; i < ChoiceList.Length; i++)
        {
            ChoiceList[i].image.sprite = null;
            ChoiceListData[i].thisButtonData = tempStruct;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class IngredientListElement : MonoBehaviour
{
    public enum eCookingStates { NotPrepared, InProgress, Prepared}

    [SerializeField] private Ingredient thisButtonIngredient;
    
    [SerializeField]
    Image ingredientImage;

    [SerializeField]
    Image preparationWay;

    [SerializeField]
    Image markImage;

    [SerializeField]
    TextMeshProUGUI markText;

    [SerializeField]
    TextMeshProUGUI ingredientName;

    [SerializeField]
    string tPreparationWay;

    [SerializeField]
    int iIngredientNumber;

    int ingredientGrade;

    int iCookingStage;

    bool bIsPicked;

    bool bIsCooked;

    bool bGraded;

    private TimedCooking cookingMingameScript;

    private string ingredientOrderId;

    public string GetIngredientsOrderId()
    {
        return ingredientOrderId;
    }

    public void SetIngredientsOrderId(string orderId)
    {
        ingredientOrderId = orderId;
    }

    //ToDO: set up an ingredient types here
    //TODO: set up the preparation way based on the ingredient type
    //TODO: split the script into two different deriving from each other, to store most common ingredient variables inside of the struct.

    [SerializeField] private GameObject goCookingMinigame;
    [SerializeField] private GameObject goCuttingMinigame;
    [SerializeField] private GameObject goPlacingMinigame;

    [SerializeField] private GameObject goPrepButton;
    [SerializeField] private GameObject goMarkStamp;

    private string requiredWorkstation;

    private void OnEnable()
    {
        goCookingMinigame = GameObject.FindGameObjectWithTag("CookingMinigame");
        goCuttingMinigame = GameObject.FindGameObjectWithTag("CuttingMinigame");
    }

    /*
     * This section is created for functional part of the Ingredient object.
     */
    private void Start()
    {
        goCookingMinigame = GameObject.FindGameObjectWithTag("CookingMinigame");
        goCuttingMinigame = GameObject.FindGameObjectWithTag("CuttingMinigame");
        SetCookingStage(0);
    }

    public void SetCookingStage(int CookingStage) //on click button function doesn't work with the enums
    {
        iCookingStage = CookingStage;
        switch ((eCookingStates)CookingStage)
        {
            case eCookingStates.NotPrepared:
            
                bIsPicked = false;
                Debug.Log("Ingredient is not prepared prepared!");
                break;            

            case eCookingStates.InProgress:
            
                bIsPicked = true;
                Debug.Log("Ingredient is being prepared!");
                break;
            

            case eCookingStates.Prepared:
                bIsPicked = false;
                Debug.Log("Ingredient is prepared!");
                break;

            default:
                Debug.LogError("using wrong type of cooking stage!");
                break;
        }
    }

    //TODO: create a similar function inside of the minigame or the workstation script it will be deriving from - so you can transfer back to the ingredient selection view and also transfer the information from the minigame(e.g. cutting - amount of good or bad cuts or just a mark)

    public void TransferToTheWorkstation(string workStation) // double check if I need to put the txt variable inside of the function
    {
        //checking the preparation method of the ingredient here
        //going through the list of the workstations comparing them with the variant current Ingreient has
        //making the transfer of the canvas/scenes to the workstation (mini game starts)
        //if (tPreparationWay == workStation)
        SceneManager.LoadScene(workStation);
    }

    /* 
     This section is done for getters and setters of the ingredient object      
     */

    public int getCookingStage()
    {
        return iCookingStage;
    }

    public int getIngredientNumber()
    {
        return iIngredientNumber;
    }

    public void setIsPicked(bool picked)
    {
        bIsPicked = picked;
    }

    public void setMarkText(string markTxt)
    {
        markText.text = markTxt;
        Debug.Log(markTxt + " mark was set");
    }

    public Image getMarkImage()
    {
        return markImage;
    }
    public int getIngredientMark()
    {
        if (markText.text == "Bad")
            ingredientGrade = 1;
        else if (markText.text == "Good")
            ingredientGrade = 2;
        else if (markText.text == "Perfect")
            ingredientGrade = 3;

        return ingredientGrade;
    }

    public bool getIsCooked()
    {
        return bIsCooked;
    }
    public void setIsCooked(bool b)
    {
        bIsCooked = b;
    }

    public string getPreparationWay()
    {
        return tPreparationWay;
    }

    public void setPreparationWay(string txt)
    {
        tPreparationWay = txt;
    }

    public bool getGraded()
    {
        return bGraded;
    }

    public void SetIngredientImage(Sprite ingredientIcon)
    {
        ingredientImage.sprite = ingredientIcon;
    }

    public Sprite GetIngredientImage()
    {
        return ingredientImage.sprite;
    }
    public void SetIngredientName(string name)
    {
        ingredientName.text = name;
    }

    public void SetThisButtonIngredient(Ingredient ingredient)
    {
        thisButtonIngredient = ingredient;
        requiredWorkstation = thisButtonIngredient.ingredientWorkstation;
    }

    public string GetThisButtonPrepMinigame()
    {
        return requiredWorkstation;
    }
    
    public void OpenCorrectWorkstationMinigame()
    {
        Order[] activeOrders = FindObjectsOfType<Order>();
        
        switch (requiredWorkstation)
        {
            case "CuttingBoard":
                FindObjectOfType<CuttingMinigame>().ProgressCooking();
                FindObjectOfType<CuttingMinigame>().ToggleMinigame();
                FindObjectOfType<CuttingMinigame>().SetIngredientNumberOnList(iIngredientNumber);
                FindObjectOfType<CuttingMinigame>().SetIngredientOrderId(ingredientOrderId);
                
                for (int i = 0; i < activeOrders.Length; i++)
                {
                    if (activeOrders[i].GetOrderId() == ingredientOrderId)
                    {
                        activeOrders[i].ToggleOrderUI();
                    }
                }
                
                break;
            
            case "Hob":
                FindObjectOfType<TimedCooking>().ToggleMinigame();
                FindObjectOfType<TimedCooking>().SetIngredientNumberOnList(iIngredientNumber);
                FindObjectOfType<TimedCooking>().SetIngredientsOrderId(ingredientOrderId);
                
                for (int i = 0; i < activeOrders.Length; i++)
                {
                    if (activeOrders[i].GetOrderId() == ingredientOrderId)
                    {
                        activeOrders[i].ToggleOrderUI();
                    }
                }
                
                
                //GameObject.FindObjectOfType<Order>().ToggleOrderUI();
                break;
            
            case "Serving":
                
                break;
            
            default:
                break;
            
        }
    }

    public void OpenCorrectWorkstationMinigame(int ingredientIndex)
    {
        switch (requiredWorkstation)
        {
            case "CuttingBoard":
                FindObjectOfType<CuttingMinigame>().ProgressCooking();
                FindObjectOfType<CuttingMinigame>().ToggleMinigame();
                FindObjectOfType<CuttingMinigame>().SetIngredientNumberOnList(iIngredientNumber);
                FindObjectOfType<CuttingMinigame>().SetIngredientOrderId(ingredientOrderId);
                GameObject.FindObjectOfType<Order>().ToggleOrderUI();
                break;

            case "Hob":
                FindObjectOfType<TimedCooking>().ToggleMinigame();
                FindObjectOfType<TimedCooking>().SetIngredientNumberOnList(iIngredientNumber);
                FindObjectOfType<TimedCooking>().SetIngredientsOrderId(ingredientOrderId);
                GameObject.FindObjectOfType<Order>().ToggleOrderUI();
                break;

            case "Serving":

                break;

            default:
                break;

        }
    }

    public int GetIngredientNumber()
    {
        return iIngredientNumber;
    }

    public void SetIngredientGrade(int grade)
    {
        ingredientGrade = grade;
        goPrepButton.gameObject.SetActive(false);
        goMarkStamp.gameObject.SetActive(true);
        bGraded = true;
    }

    public void ResetIngredient()
    {
        ingredientGrade = 0;
        iCookingStage = 0;
        goPrepButton.gameObject.SetActive(true);
        goMarkStamp.gameObject.SetActive(false);
        bIsCooked = false;
        bGraded = false;
    }
}

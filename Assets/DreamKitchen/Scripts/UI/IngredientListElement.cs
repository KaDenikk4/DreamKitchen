using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class IngredientListElement : MonoBehaviour
{
    public enum eCookingStates { NotPrepared, InProgress, Prepared }

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
    [SerializeField] Sprite[] markSprites;

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
        goCookingMinigame = GameObject.FindGameObjectWithTag("CookingMinigame"); // finding correct cooking minigame   
        goCuttingMinigame = GameObject.FindGameObjectWithTag("CuttingMinigame"); // finding the correct cutting mingame
    }

    /*
     * This section is created for functional part of the Ingredient object.
     */
    private void Start()
    {
        goCookingMinigame = GameObject.FindGameObjectWithTag("CookingMinigame"); // finding correct cooking minigame   
        goCuttingMinigame = GameObject.FindGameObjectWithTag("CuttingMinigame"); // finding the correct cutting mingame
        SetCookingStage(0);
    }

    public void SetCookingStage(int CookingStage) //on click button function doesn't work with the enums
    {
        //cooking stages of the ingredient
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

    public string GetIngredientName()
    {
        return ingredientName.text;
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
    
    public void OpenCorrectWorkstationMinigame() // to open corrent minigame for the ingredient
    {
        Order[] activeOrders = FindObjectsOfType<Order>(); // all orders
        CuttingMinigameManager cuttingMinigame = FindObjectOfType<CuttingMinigameManager>(); // cutting minigame
        
        switch (requiredWorkstation)
        {
            case "CuttingBoard":   // cutting minigame stage
                cuttingMinigame.transform.GetChild(1).gameObject.SetActive(true);  
                cuttingMinigame.ProgressCount = 0; 
                cuttingMinigame.ProgressCooking(); // start the minigame
                cuttingMinigame.IngredientNumberFromTheList = iIngredientNumber; // set ingredient number
                cuttingMinigame.IngredientOrderId = ingredientOrderId; // set ingredient ID
                cuttingMinigame.RequiredName = ingredientImage.sprite.name; // set required ingredient name
                cuttingMinigame.RequiredIngredientImage.sprite = ingredientImage.sprite; // set required ingredient sprite

                for (int i = 0; i < activeOrders.Length; i++)
                {
                    if (activeOrders[i].GetOrderId() == ingredientOrderId)
                    {
                        activeOrders[i].ToggleOrderUI(); // switch UI
                        activeOrders[i].HideOrderIcons(); // hide order icons
                    }
                }
                
                break;
            
            case "Hob":
                BackgroundCookingHolder listForIndicators = FindObjectOfType<BackgroundCookingHolder>();
                
                for (int i = 0; i < listForIndicators.listOfBackgroundCookingIndicators.Count; i++)
                {
                    if (!listForIndicators.listOfBackgroundCookingIndicators[i].GetIsMinigameRunning())
                    {
                        listForIndicators.listOfBackgroundCookingIndicators[i].gameObject.SetActive(true);
                        listForIndicators.listOfBackgroundCookingIndicators[i]
                            .SetRunning(0.0f, ingredientOrderId, iIngredientNumber);
                        listForIndicators.listOfBackgroundCookingIndicators[i].ToggleBackgroundCooking();
                       
                        goPrepButton.SetActive(false);
                       
                        GameObject.Find("WhiteSmoke").GetComponent<ParticleSystem>().Play();
                        GameObject.Find("WhiteSmoke").GetComponent<AudioSource>().Play();
                
                        return;
                    }
                }
                
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
    { // calculating the grade
        ingredientGrade = grade;
        goPrepButton.gameObject.SetActive(false);
        goMarkStamp.gameObject.SetActive(true);

        switch (ingredientGrade) // depending on the grade setting up the image
        {
            case 1:
                //goMarkStamp.GetComponent<Image>().color = Color.red;
                goMarkStamp.GetComponent<Image>().sprite = markSprites[2];
                break;
            case 2:
                //goMarkStamp.GetComponent<Image>().color = Color.yellow;
                goMarkStamp.GetComponent<Image>().sprite = markSprites[1];
                break;
            case 3:
                //goMarkStamp.GetComponent<Image>().color = Color.green;
                goMarkStamp.GetComponent<Image>().sprite = markSprites[0];
                break;
        }
        bGraded = true;
    }

    public void ResetIngredient()
    {
        //reset
        ingredientGrade = 0;
        iCookingStage = 0;
        goPrepButton.gameObject.SetActive(true);
        goMarkStamp.gameObject.SetActive(false);
        bIsCooked = false;
        bGraded = false;
    }
}

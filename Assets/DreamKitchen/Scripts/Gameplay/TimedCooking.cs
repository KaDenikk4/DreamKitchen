using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class TimedCooking : MonoBehaviour
{
    private enum cookingStages
    {
        PreCooking = 0,
        Cooking = 1,
        PostCooking = 2
    }

    cookingStages currentCookingStage;

    [SerializeField] 
    private float fCookingSpeed;

    [SerializeField]
    private Image cookingGaugeBarImage;

    private RectTransform cookingBarTransform;

    private Vector3 cookingGaugeBarPosition;
    private Vector3 cookingGaugeBarStartingPosition;

    private int cookClickCount;

    private bool resultObtained;

    private float gradePosition;

    [SerializeField]
    private GameObject goGradeDisplay;
    private TextMeshProUGUI tmpGrade;

    private int ingredientGrade;

    [SerializeField] private TextMeshProUGUI cookServeButtonText;

    private string currentPrepStage;
    
    private IngredientGradeStruct ingredientNumberAndGrade;

    private bool minigameActive = false;

    [SerializeField] private GameObject cookingBar;

    [SerializeField] private GameObject WorkStations;

    private int ingredientNumberFromTheList;
    private string ingredientOrderId;

    private float progressPercentage = 0.0f;

    public void SetIngredientsOrderId(string orderId)
    {
        ingredientOrderId = orderId;
    }

    private string GetIngredientsOrderId()
    {
        return ingredientOrderId;
    }

    // Start is called before the first frame update
    void Start()
    {
        cookingBarTransform = cookingBar.GetComponent<RectTransform>();
        cookingGaugeBarPosition = cookingGaugeBarImage.rectTransform.anchoredPosition;
        tmpGrade = goGradeDisplay.GetComponent<TextMeshProUGUI>();

        ingredientGrade = 0;

        if (cookClickCount == 0)
        {
            currentCookingStage = cookingStages.PreCooking;
        }

        resultObtained = false;
    }
    
    

    // Update is called once per frame
    void Update()
    {
        Cook();
    }

    void Cook()
    {
        switch (currentCookingStage)
        {
            //TODO: Choose ingredient to put in the cooking station for precooking
            case cookingStages.PreCooking:
                //Debug.Log("TimedCooking.cs: Player hasn't started cooking yet!");
                currentPrepStage = "PreCooking";
                break;
            
            case cookingStages.Cooking:

                currentPrepStage = "Cooking";
                if (cookingGaugeBarPosition.y < cookingBarTransform.rect.height - cookingGaugeBarImage.GetComponent<RectTransform>().rect.height)
                {
                    cookingGaugeBarPosition.y += fCookingSpeed * Time.deltaTime;

                    progressPercentage = cookingGaugeBarPosition.y / cookingBarTransform.rect.height;
                    
                    cookingGaugeBarImage.rectTransform.anchoredPosition = cookingGaugeBarPosition;
                }

                break;
            
            case cookingStages.PostCooking:

                currentPrepStage = "PostCooking";
                GetResultOfCooking();
                ingredientNumberAndGrade.ingredientNumber = ingredientNumberFromTheList;
                ingredientNumberAndGrade.ingredientGrade = ingredientGrade;
                break;
            
            default:
                Debug.LogWarning("You're not using cookingStages correctly");
                break;
        }
        
    }

    public void ProgressCooking()
    {
        cookClickCount++;

        if (cookClickCount == 1)
        {
            currentCookingStage = cookingStages.Cooking;
            cookServeButtonText.text = "Finish";
        }

        if (cookClickCount == 2)
        {
            currentCookingStage = cookingStages.PostCooking;
            cookServeButtonText.text = "Serve";
        }

        if (cookClickCount == 3)
        {
            FinishCookingMinigame();
        }
    }

    void GetResultOfCooking()
    {
        //TODO: Cache ranges on the cooking bar
        if (!resultObtained)
        {
            if (cookingGaugeBarPosition.y < 100)
            {
                tmpGrade.text = "BAD";
                ingredientGrade = 1;
            }

            if (cookingGaugeBarPosition.y > 100 && cookingGaugeBarPosition.y < 200)
            {
                tmpGrade.text = "GOOD";
                ingredientGrade = 2;
            }

            if (cookingGaugeBarPosition.y > 200 && cookingGaugeBarPosition.y < 300)
            {
                tmpGrade.text = "PERFECT";
                ingredientGrade = 3;
            }

            if (cookingGaugeBarPosition.y > 300)
            {
                tmpGrade.text = "BAD";
                ingredientGrade = 1;
            }
        }
        
        resultObtained = true;
    }

    public int GetIngredientGrade()
    {
        return ingredientGrade;
    }

    void ResetCookingMinigame()
    {
        cookingGaugeBarPosition = new Vector3(0, 0, 0);
        cookingGaugeBarImage.rectTransform.anchoredPosition = cookingGaugeBarPosition;
        currentCookingStage = cookingStages.PreCooking;
        cookClickCount = 0;
        tmpGrade.text = "-------------";
        resultObtained = false;
        cookServeButtonText.text = "Cook";
        progressPercentage = 0.0f;
    }

    void FinishCookingMinigame()
    {
        Order[] activeOrders = FindObjectsOfType<Order>();
        
        for (int i = 0; i < activeOrders.Length; i++)
        {
            if (activeOrders[i].GetOrderId() == ingredientOrderId)
            {
                activeOrders[i].SetIngredientMark(ingredientNumberAndGrade);
                activeOrders[i].ToggleOrderUI();
            }
        }
        //WorkStations.SetActive(true);
        ResetCookingMinigame();
        ToggleMinigame();
    }

    public void SetIngredientNumberOnList(int ingredientListElementNumber)
    {
        ingredientNumberFromTheList = ingredientListElementNumber;
    }
    
    public void ToggleMinigame()
    {
        if (!minigameActive)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }

            minigameActive = true;
        }
        
        else if (minigameActive)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
            }

            minigameActive = false;
        }
    }

    public void SetRunning()
    {
        cookClickCount = 1;
        currentCookingStage = cookingStages.Cooking;
        cookingGaugeBarPosition.y = progressPercentage * cookingBarTransform.rect.height;
        cookServeButtonText.text = "Finish";
    }

    public float GetCookingMinigameProgress()
    {
        return progressPercentage;
    }
    
    public void SetProgress(float progressRatio)
    {
        progressPercentage = progressRatio;
    }

    public void StartBackgroundCooking()
    {
        BackgroundCookingHolder listForIndicators = FindObjectOfType<BackgroundCookingHolder>();
        Order[] activeOrders = FindObjectsOfType<Order>();
        
        for (int i = 0; i < listForIndicators.listOfBackgroundCookingIndicators.Count; i++)
        {
            if (!listForIndicators.listOfBackgroundCookingIndicators[i].GetIsMinigameRunning())
            {
                listForIndicators.listOfBackgroundCookingIndicators[i].gameObject.SetActive(true);
                listForIndicators.listOfBackgroundCookingIndicators[i]
                    .SetRunning(progressPercentage, ingredientOrderId, ingredientNumberFromTheList);
                listForIndicators.listOfBackgroundCookingIndicators[i].ToggleBackgroundCooking();
                ToggleMinigame();
                
                for (int j = 0; j < activeOrders.Length; j++)
                {
                    if (activeOrders[j].GetOrderId() == ingredientOrderId)
                    {
                        activeOrders[j].ToggleOrderUI();
                    }
                }
                
                return;
            }
        }
    }

}

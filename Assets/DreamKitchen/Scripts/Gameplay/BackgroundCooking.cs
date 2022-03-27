using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundCooking : MonoBehaviour
{
    private string orderId;

    private int ingredientNumber;

    private IngredientGradeStruct ingredientNumberAndGrade;
    
    private float currentProgress;

    [SerializeField] private Image progressBar;

    private TimedCooking minigameReference;

    private bool isMinigameRunning = false;

    public bool GetIsMinigameRunning()
    {
        return isMinigameRunning;
    }

    public void SetIsMinigameRunning(bool isItRunning)
    {
        isItRunning = isMinigameRunning;
    }

    public void Start()
    {
        minigameReference = GameObject.FindObjectOfType<TimedCooking>();
    }

    public void Update()
    {
        if (isMinigameRunning)
        {
            progressBar.fillAmount += 0.05f * Time.deltaTime;
            currentProgress = progressBar.fillAmount;
        }
    }
    
    public void OpenMinigameWithProgress()
    {
        //make active
        minigameReference.ToggleMinigame();
        
        //pass Order id
        minigameReference.SetIngredientsOrderId(orderId);
        
        //pass ingredient number
        minigameReference.SetIngredientNumberOnList(ingredientNumber);
        
        isMinigameRunning = false;
        //pass current progress - set the bar position
        minigameReference.SetProgress(currentProgress);

        //set cooking in progress
        minigameReference.SetRunning();
        
        currentProgress = 0.0f;
        
        ToggleBackgroundCooking();
    }

    public void FinishCooking()
    {
        if (progressBar.fillAmount < 0.25f || progressBar.fillAmount > 0.75f)
        {
            ingredientNumberAndGrade.ingredientGrade = 1;
        }

        if (progressBar.fillAmount > 0.25f && progressBar.fillAmount < 0.5f)
        {
            ingredientNumberAndGrade.ingredientGrade = 2;            
        }
        
        if (progressBar.fillAmount > 0.5f && progressBar.fillAmount < 0.75f)
        {
            ingredientNumberAndGrade.ingredientGrade = 3;            
        }


        Order[] activeOrders = FindObjectsOfType<Order>();
        
        for (int i = 0; i < activeOrders.Length; i++)
        {
            //ToDo:: Set ingredient number and grade.
            if (activeOrders[i].GetOrderId() == orderId)
            {
                activeOrders[i].SetIngredientMark(ingredientNumberAndGrade);
                //activeOrders[i].ToggleOrderUI();
            }
        }

        isMinigameRunning = false;
        ToggleBackgroundCooking();

    }

    public void SetRunning(float progress, string ingredientOrderId, int ingredientNumberOnOrder)
    {
        progressBar.fillAmount   = progress;
        ingredientNumber         = ingredientNumberOnOrder;
        ingredientNumberAndGrade.ingredientNumber         = ingredientNumberOnOrder;
        orderId                  = ingredientOrderId;
        isMinigameRunning = true;
    }

    public void SetDataAndRun(string ingredientOrderId, int ingredientNumberOnOrder)
    {
        ingredientNumberAndGrade.ingredientNumber         = ingredientNumberOnOrder;
        orderId                                           = ingredientOrderId;
        isMinigameRunning = true;
    }

    public void ToggleBackgroundCooking()
    {
        if (isMinigameRunning)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(true);
            }    
        }
        if (!isMinigameRunning)
        {
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                gameObject.transform.GetChild(i).gameObject.SetActive(false);
                currentProgress = 0.0f;
            }    
        }
        
    }

}

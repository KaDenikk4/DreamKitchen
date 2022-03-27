using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CuttingMinigame : MonoBehaviour
{
    private enum gameStages
    {
        PreCooking = 0,
        Cooking = 1,
        PostCooking = 2
    }

    private gameStages currentStage;

    [SerializeField]
    private float fSpeed;
    [SerializeField]
    private Vector3 vStartPosition;
    [SerializeField]
    private Vector3 vDestination;
    [SerializeField]
    private Vector3 vCurrentPosition;    
    [SerializeField]
    private Image sGreenArea;
    [SerializeField]
    private Image sBar;
    private int iCutAmount;
    private int iGoodMarks;
    private int iBadMarks;
    private static string sFinalMark;
    private int iFinalMark;
    bool bCheck;
    bool bReachedPoint = false;
    List<string> lsMarks = new List<string>();
    private IngredientGradeStruct ingredientNumberAndGrade;
    private int ingredientNumberFromTheList;
    [SerializeField] private Button MinigameFinished;
    [SerializeField] private GameObject goCuttingMinigame;
    private string ingredientOrderId;
    private bool minigameActive;
    [SerializeField] private GameObject movingSight;
    private int progressCount;
    //game manager
    private GameManager gm;
    [SerializeField] private GameObject goMouseFeedbackGood;
    [SerializeField] private GameObject goMouseFeedbackBad;

    private void Start()
    {        
        SetMovingSightImage(); //setting sight image
        vStartPosition = movingSight.GetComponent<RectTransform>().anchoredPosition; //setting the start position of the sight
        vDestination.x = vStartPosition.x + sBar.rectTransform.rect.width; // setting destination position
        currentStage = gameStages.PreCooking; // setting the stage
        ResetCuttingMinigame(); //reset
    }
    // Update is called once per frame
    void Update()
    {
        switch (currentStage) // setting the switch for current stages
        {
            case gameStages.PreCooking:
                break;

            case gameStages.Cooking:
                moveSight();
                performCut();
                break;

            case gameStages.PostCooking:
                break;

            default:
                break;
        }

    }

    void moveSight() 
    {
        vCurrentPosition = movingSight.GetComponent<RectTransform>().anchoredPosition; //setting the current position
        vCurrentPosition.x += (fSpeed * Time.deltaTime); // setting the speed
        movingSight.GetComponent<RectTransform>().anchoredPosition = vCurrentPosition; // setting the current position to the anchor

        //setting the ability to move from one side to another (infinitely) 
        if (vCurrentPosition.x >= vDestination.x && bReachedPoint == false) 
        {
            fSpeed = -fSpeed;
            bReachedPoint = true;
        }
        if (vCurrentPosition.x <= vStartPosition.x && bReachedPoint == true)
        {
            fSpeed = -fSpeed;
            bReachedPoint = false;
        }
    }

    void performCut() 
    {
        float fGreenAreaWidth = sGreenArea.rectTransform.rect.width;
        Debug.Log(fGreenAreaWidth);
        float fBarWidth = sBar.rectTransform.rect.width;
        Debug.Log(fBarWidth);
        
        //checking what happens if player cuts at green area
        if (Input.GetMouseButtonDown(0) && iCutAmount < 3 && (vCurrentPosition.x > fBarWidth / 2 - fGreenAreaWidth / 2 + vStartPosition.x) && (vCurrentPosition.x < fBarWidth / 2 + fGreenAreaWidth / 2 + vStartPosition.x))
        {
            GetComponent<AudioSource>().Play();
            lsMarks.Add("Good");
            iCutAmount++;
            Debug.Log("Good cut");
            ProvideFeedbackOnCutGood();
        }

        //checking what happens if player cuts on red area
        else if ((Input.GetMouseButtonDown(0) && iCutAmount < 3 && (vCurrentPosition.x < fBarWidth / 2 - fGreenAreaWidth / 2 + vStartPosition.x) && (vCurrentPosition.x > vStartPosition.x)) || (Input.GetMouseButtonDown(0) && iCutAmount < 3 && (vCurrentPosition.x > fBarWidth / 2 + fGreenAreaWidth / 2 + vStartPosition.x)) && (vCurrentPosition.x < fBarWidth + vStartPosition.x))
        {
            GetComponent<AudioSource>().Play();
            lsMarks.Add("Bad");
            iCutAmount++;
            Debug.Log("Bad cut");
            ProvideFeedbackOnCutBad();
        }


        //calculating the final result
        if (bCheck == false && iCutAmount == 3)
        {
            GetComponent<AudioSource>().Play();
            Evaluation();
            bCheck = true;
            currentStage = gameStages.PostCooking;
        }
    }

    void Evaluation()
    {
        //setting the evaluatio method
        if (iCutAmount == 3)
        {
            for (int i = 0; i < lsMarks.Count; i++)
            {
                if (lsMarks[i] == "Bad")
                    iBadMarks++;
                else
                    iGoodMarks++;
            }

            if (iGoodMarks == 3)
            {
                sFinalMark = "Perfect";
                iFinalMark = 3;
            }
            else if (iGoodMarks > iBadMarks && iGoodMarks < 3)
            {
                sFinalMark = "Good";
                iFinalMark = 2;
            }
            else
            {
                sFinalMark = "Bad";
                iFinalMark = 1;
            }

            //setting the ingredient mark by the number on the list
            ingredientNumberAndGrade.ingredientNumber = ingredientNumberFromTheList;
            ingredientNumberAndGrade.ingredientGrade = iFinalMark;
            
            Order[] activeOrders = FindObjectsOfType<Order>();
        
            for (int i = 0; i < activeOrders.Length; i++)
            {
                if (activeOrders[i].GetOrderId() == ingredientOrderId)
                {
                    activeOrders[i].SetIngredientMark(ingredientNumberAndGrade);;
                }
            }
            
            MinigameFinished.gameObject.SetActive(true);
        }

        Debug.Log(sFinalMark);
    }

    public void ProgressCooking()
    {
        //chenging stages of the minigame
        progressCount++;

        if (progressCount == 1)
        {
            currentStage = gameStages.Cooking;
        }

        if (progressCount == 2)
        {
            currentStage = gameStages.PostCooking;
        }
    }

    public void FinishMinigame()
    {
        //minigame ends
        Order[] activeOrders = FindObjectsOfType<Order>();
        
        for (int i = 0; i < activeOrders.Length; i++)
        {
            if (activeOrders[i].GetOrderId() == ingredientOrderId)
            {
                activeOrders[i].ToggleOrderUI();
                activeOrders[i].ToggleOrderUI();
            }
        }
        
        ResetCuttingMinigame();
        ToggleMinigame();

    }


    public void SetMovingSightImage()
    {
        // setting the moving sight image here
        gm = FindObjectOfType<GameManager>();
        if (gm.playerEquipment.equippedKnife)
            movingSight.GetComponent<Image>().sprite = gm.playerEquipment.equippedKnife.productAsset;
    }
    public string getFinalMark()
    {
        return sFinalMark;
    }
    public void ResetCuttingMinigame()
    {
        //minigame reset
        lsMarks.Clear();
        bCheck = false;
        iCutAmount = 0;
        iGoodMarks = 0;
        iBadMarks = 0;
        sFinalMark = null;
        currentStage = gameStages.PreCooking;
        progressCount = 0;
    }


    public void SetIngredientNumberOnList(int ingredientListElementNumber)
    {
        ingredientNumberFromTheList = ingredientListElementNumber;
    }

    public void ToggleMinigame()
    {
        //swapping images to be invisible of visible depending on the stage of the minigame
        if (!minigameActive)
        {
            for (int i = 0; i < gameObject.transform.childCount - 1; i++)
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

    public void ProvideFeedbackOnCutGood()
    {
        //instantiating visual effects

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;       // we want 2m away from the camera position
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        Instantiate(goMouseFeedbackGood, objectPos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    public void ProvideFeedbackOnCutBad()
    {
        //instantiating visual effects

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10.0f;       // we want 2m away from the camera position
        Vector3 objectPos = Camera.main.ScreenToWorldPoint(mousePos);
        Instantiate(goMouseFeedbackBad, objectPos, Quaternion.identity, GameObject.FindGameObjectWithTag("Canvas").transform);
    }

    public string GetIngredientOrderId()
    {
        return ingredientOrderId;
    }

    public void SetIngredientOrderId(string orderId)
    {
        ingredientOrderId = orderId;
    }

}
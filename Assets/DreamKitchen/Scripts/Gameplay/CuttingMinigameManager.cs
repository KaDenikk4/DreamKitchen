using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CuttingMinigameManager : MonoBehaviour
{
    [SerializeField] private string requiredName;
    [SerializeField] private Image requiredIngredientImage;
    [SerializeField] private TextMeshProUGUI scoreText;
    private int scoreCount;
    [SerializeField] private int maxCutAmount;
    private int progressCount;
    private string ingredientOrderId;
    [SerializeField] private Image backgroundImage;
    private int ingredientNumberFromTheList;
    private bool check = true;

    [SerializeField] private GameObject SpawnPoint;

    private int goodMarks;
    private int badMarks;
    private int finalMark;

    public Image BackgroundImage { get => backgroundImage; set => backgroundImage = value; }
    public string RequiredName { get => requiredName; set => requiredName = value; }
    public string IngredientOrderId { get => ingredientOrderId; set => ingredientOrderId = value; }
    public int ScoreCount { get => scoreCount; set => scoreCount = value; }
    public int GoodMarks { get => goodMarks; set => goodMarks = value; }
    public int BadMarks { get => badMarks; set => badMarks = value; }
    public int IngredientNumberFromTheList { get => ingredientNumberFromTheList; set => ingredientNumberFromTheList = value; }
    public int ProgressCount { get => progressCount; set => progressCount = value; }
    public Image RequiredIngredientImage { get => requiredIngredientImage; set => requiredIngredientImage = value; }
    public int Index { get => index; set => index = value; }
    public int OtherIndex { get => otherIndex; set => otherIndex = value; }

    private enum gameStages
    {
        PreCooking = 0,
        Cooking = 1,
        PostCooking = 2
    }

    private gameStages currentStage;

    private IngredientGradeStruct ingredientNumberAndGrade;

    //cutting elements spawner variables
    [SerializeField] private GameObject cuttingElementPrefab;
    [SerializeField] private CuttingElement _cuttingElementPrefab;
    private System.Random rnd1 = new System.Random();
    private System.Random rnd2 = new System.Random();
    int index;
    int otherIndex;
    [SerializeField] private float fRespawnTime;
    private Vector2 screenBounds;


    private void Start()
    {
        //setting up the area for cutting minigame
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));      
    }

    private void Update()
    {
        //cutting minigame stages
        switch (currentStage)
        {
            case gameStages.PreCooking:
                break;

            case gameStages.Cooking:

                if (check)
                {
                    ResetMinigame(); // reset
                    index = UnityEngine.Random.Range(0, _cuttingElementPrefab.ingredientSprites.Length); // random ingredient from the list
                    otherIndex = UnityEngine.Random.Range(0, _cuttingElementPrefab.ingredientSprites.Length); // random ingredient from the list
                    
                    StartCoroutine(CuttingElementsWave()); // start minigame
                    check = false;
                }

                if (scoreCount == maxCutAmount)
                {
                    ProgressCooking(); //next stage
                }
                break;

            case gameStages.PostCooking:
                if (!check)
                {                    
                    StopAllCoroutines(); // finishing minigame
                    this.transform.GetChild(2).gameObject.SetActive(true);
                    check = true;
                }
                break;

            default:
                break;
        }
        scoreText.text = scoreCount.ToString(); // setting the score
    }


    public void ProgressCooking()
    {
        //stages of the game progression
        ProgressCount++;
        if (ProgressCount == 0)
        {
            currentStage = gameStages.PreCooking;
        }

        if (ProgressCount == 1)
        {
            currentStage = gameStages.Cooking;
        }

        if (ProgressCount == 2)
        {
            currentStage = gameStages.PostCooking;
        }

    }


    public void EvaluateGame()
    {
        //evaluating method
        if(scoreCount >= maxCutAmount)
        {
            if (goodMarks == scoreCount)
            {
                finalMark = 3;
            }
            else if(goodMarks > BadMarks && goodMarks < scoreCount)
            {
                finalMark = 2;
            }
            else
            {
                finalMark = 1;
            }
            //setting mark by the ingredient number
            ingredientNumberAndGrade.ingredientNumber = IngredientNumberFromTheList;
            ingredientNumberAndGrade.ingredientGrade = finalMark;

            Order[] activeOrders = FindObjectsOfType<Order>();

            //checking id of the ingredient
            for (int i = 0; i < activeOrders.Length; i++)
            {
                if (activeOrders[i].GetOrderId() == ingredientOrderId)
                {
                    activeOrders[i].SetIngredientMark(ingredientNumberAndGrade); // setting mark

                    activeOrders[i].ToggleOrderUI(); // switching ui
                }
            }
            this.transform.GetChild(1).gameObject.SetActive(false);
        }
    }
    
    public void Finish()
    {
        EvaluateGame();
    }

    public void ResetMinigame()
    {
        //minigame reset
        StopAllCoroutines();
        scoreText.text = "0";
        finalMark = 0;
        goodMarks = 0;
        badMarks = 0;        
        scoreCount = 0;
        check = true;
        Input.ResetInputAxes();
    }


    //Cutting elements spawner here

    private void SpawnCuttingElement()
    {
        // function for instantiation of the cutting elements
        GameObject go = Instantiate(cuttingElementPrefab) as GameObject;
        go.transform.parent = backgroundImage.transform;
        go.transform.position = new Vector3(SpawnPoint.transform.position.x, UnityEngine.Random.Range(-backgroundImage.rectTransform.rect.height/2, backgroundImage.rectTransform.rect.height/2), -1);  
    }

    //creating a coroutine to make function to be called once in few seconds
    IEnumerator CuttingElementsWave()
    {
        while (true)
        {
            yield return new WaitForSeconds(fRespawnTime);
            SpawnCuttingElement();
        }
    }
}

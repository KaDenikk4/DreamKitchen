using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Order : MonoBehaviour
{
    // TODO:  change the structure 
    [SerializeField]
    Image orderBackground;

    [SerializeField]
    TextMeshProUGUI orderName;

    [SerializeField]
    int iIngredientAmount;

    const int iMaxIngredientAmount = 4;

    int iDishFinalMark;

    System.Random rnd = new System.Random();

    [SerializeField]
    IngredientListElement[] ingredients = new IngredientListElement[4];

    //setting up the correct order of the ingredients(how to place them)
    [SerializeField]
    IngredientListElement[] ingredientsCorrectOrder;

    [SerializeField] private RecipeHolder currentRecipe;

    [SerializeField] private Image orderIcon;
    [SerializeField] private Image servingPicture;

    [SerializeField] private GameObject orderIconGameObject;
    [SerializeField] private GameObject orderUiGameObject;
    [SerializeField] private GameObject goBadMarkStamp;
    [SerializeField] private GameObject goGoodMarkStamp;
    [SerializeField] private GameObject goPerfectMarkStamp;

    [SerializeField] private GameObject goListIngredientPrefab;
    
    [SerializeField] private CountdownTimer timer;

    [SerializeField] private Score score;

    GameObject[] orderIcons;
    
    private bool bOrderExpanded;

    private string orderId;
    
    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        //transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        currentRecipe = GetComponentInChildren<RecipeHolder>();
        iIngredientAmount = currentRecipe.GetCurrentRecipe().ingredientList.Count;
        bOrderExpanded = false;

        orderIcons = GameObject.FindGameObjectsWithTag("OrderIcon");
        
        //FindIngredientButtons();
        
        if (iMaxIngredientAmount>iIngredientAmount)
        {
            for(int i = iIngredientAmount; i < iMaxIngredientAmount; i++)
            {
                //ingredients[i].gameObject.SetActive(false);
                Destroy(ingredients[i].gameObject);
            }
        }
        //ToDo: Instantiate correct amount of ingredients.
        
        

        orderIcon.sprite = currentRecipe.GetCurrentRecipe().recipeIcon;
        servingPicture.sprite = currentRecipe.GetCurrentRecipe().recipeIcon;
        PopulateRecipeListUI();
        //GameObject.FindGameObjectWithTag("Order").SetActive(false);
    }

    public void orderFinalMark()
    {
        for (int i = 0; i < iIngredientAmount; i++)
        {
            if (ingredients[i].getMarkImage().gameObject.activeSelf)
            {
                if (ingredients[i].getIsCooked() == false)
                {
                    iDishFinalMark += ingredients[i].getIngredientMark();
                    ingredients[i].setIsCooked(true);
                }
                bAllCooked();
            }
            else
                Debug.Log("some ingredients are not graded yet");
        }

        if(bAllCooked())
        {
            Debug.Log(iDishFinalMark);
            if(iDishFinalMark/iIngredientAmount == 3)
            {
                Debug.Log("PERFECT MARK");
                goPerfectMarkStamp.gameObject.SetActive(true);
                timer.AddTime(timer.getPerfectTime());
            }
            else if ((float)iDishFinalMark / (float)iIngredientAmount < 3 && (float)iDishFinalMark / (float)iIngredientAmount > 1.5)
            {
                Debug.Log("GOOD MARK");
                goGoodMarkStamp.gameObject.SetActive(true);
                timer.AddTime(timer.getGoodTime());
            }
            else if((float)iDishFinalMark / (float)iIngredientAmount <= 1.5)
            {
                Debug.Log("BAD MARK");
                goBadMarkStamp.gameObject.SetActive(true);
                timer.AddTime(timer.getBadTime());
            }
            orderPreview();
        }
    }

    public void PopulateRecipeListUI()
    {
        for (int i = 0; i < currentRecipe.GetCurrentRecipe().ingredientList.Count; i++)
        {
            orderName.text = currentRecipe.GetCurrentRecipe().recipeName;
            ingredients[i].SetThisButtonIngredient(currentRecipe.GetCurrentRecipe().ingredientList[i]);
            ingredients[i].SetIngredientImage(currentRecipe.GetCurrentRecipe().ingredientList[i].ingredientIcon);
            ingredients[i].SetIngredientName(currentRecipe.GetCurrentRecipe().ingredientList[i].ingredientName);
            ingredients[i].SetIngredientsOrderId(orderId);
        }
    }
    

    bool bAllCooked()
    {
        for (int j = 0; j < iIngredientAmount; j++)
        {
            if (ingredients[j].getIsCooked() == false)
            {
                return false;
            }
        }
        return true;
    }

    public void orderPreview()
    {
        // calling animation to preview the order 
       
        //getting rid of the order and ingredients
        //showing up the dish, which was created
        servingPicture.gameObject.SetActive(true);
        servingPicture.transform.GetChild(0).gameObject.SetActive(true);


    }

    public void orderServing()
    {
        // calling animation to serv the order to the customer
        Debug.Log("Order is served");

        score.AddToTheTotalMark((float)iDishFinalMark/(float)iIngredientAmount);
        score.AddToTheOrderAmount(1);
    }

    public void ingredientPlacing()
    {
        // pressing ingredients in correct order like in the recipe - from bottom to the top(e.g. bread, butter, bacon)
    }

    public void SetIngredientMark(IngredientGradeStruct ingredientNumberAndGrade)
    {
        ingredients[ingredientNumberAndGrade.ingredientNumber].SetIngredientGrade(ingredientNumberAndGrade.ingredientGrade);
        Debug.Log(ingredientNumberAndGrade.ingredientGrade);
    }

    public void ToggleOrderUI()
    {
        if (!bOrderExpanded)
        {
            orderIconGameObject.SetActive(false);
            orderUiGameObject.SetActive(true);
            HideOrderIcons();
            bOrderExpanded = true;
        }
        else if (bOrderExpanded)
        {
            orderIconGameObject.SetActive(true);
            orderUiGameObject.SetActive(false);
            ShowOrderIcons();
            bOrderExpanded = false;
        }
    }

    public void SetOrderId(Guid customOrderId)
    {
        orderId = customOrderId.ToString();
    }

    public string GetOrderId()
    {
        return orderId;
    }
    
    public void FindIngredientButtons()
    {
        for (int i = 0; i < currentRecipe.GetCurrentRecipe().ingredientList.Count; i++)
        {
            ingredients[i] = GameObject.FindGameObjectWithTag("Ingredients").transform.GetChild(i).gameObject
                .GetComponent<IngredientListElement>();
        }
    }

    public void ResetOrder()
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].ResetIngredient();
        }
        iDishFinalMark = 0;
    }

    public IngredientListElement GetIngredientFromList(int ingredient)
    {
     //   if (ingredient < iIngredientAmount && ingredient >= 0 )
            return ingredients[ingredient];
       // else 
          //  return null;
    }

    public void HideOrderIcons()
    {
        for (int i = 0; i < orderIcons.Length; i++)
        {
            orderIcons[i].SetActive(false);
        }
    }

    public void ShowOrderIcons()
    {
        for (int i = 0; i < orderIcons.Length; i++)
        {
            orderIcons[i].SetActive(true);
        }
    }

}

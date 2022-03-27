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

    private Vector2 order2Pos = new Vector2(-560.0f, 120.0f);
    private Vector2 order3Pos = new Vector2(-560.0f, -40.0f);
    private RollingOrders helperRollingScript;
    private Order[] allOrdersArray;
    
    GameObject[] orderIcons;
    private bool bOrderExpanded;

    private string orderId;

    private Guid orderGuid;
    
    private void Start()
    {
        Initialise();
    }

    public void Initialise()
    {
        helperRollingScript = GameObject.FindObjectOfType<RollingOrders>();
        allOrdersArray = helperRollingScript.GetOrdersArray();
        GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
        currentRecipe = GetComponentInChildren<RecipeHolder>();
        iIngredientAmount = currentRecipe.GetCurrentRecipe().ingredientList.Count;
        bOrderExpanded = false;
        orderIcons = GameObject.FindGameObjectsWithTag("OrderIcon");
        
        if (iMaxIngredientAmount>iIngredientAmount)
        {
            for(int i = iIngredientAmount; i < iMaxIngredientAmount; i++)
            {
                Destroy(ingredients[i].gameObject);
            }
        }
        //ToDo: Instantiate correct amount of ingredients.
        
        

        orderIcon.sprite = currentRecipe.GetCurrentRecipe().recipeIcon;
        servingPicture.sprite = currentRecipe.GetCurrentRecipe().recipeIcon;
        PopulateRecipeListUI();
    }

    public void orderFinalMark() // setting order final mark
    {
        for (int i = 0; i < iIngredientAmount; i++)
        {
            if (ingredients[i].getMarkImage().gameObject.activeSelf)
            {
                if (ingredients[i].getIsCooked() == false)
                {
                    iDishFinalMark += ingredients[i].getIngredientMark(); // collecting all marks from ingredients
                    ingredients[i].setIsCooked(true);
                }
                bAllCooked();
            }
            else
                Debug.Log("some ingredients are not graded yet");
        }

        if(bAllCooked())
        {
            // checking order final mark 
            Debug.Log(iDishFinalMark);
            if(iDishFinalMark/iIngredientAmount == 3) // perfect
            {
                Debug.Log("PERFECT MARK");
                goPerfectMarkStamp.gameObject.SetActive(true);
                timer.AddTime(timer.getPerfectTime());
            }
            else if ((float)iDishFinalMark / (float)iIngredientAmount < 3 && (float)iDishFinalMark / (float)iIngredientAmount > 1.5) // good
            {
                Debug.Log("GOOD MARK");
                goGoodMarkStamp.gameObject.SetActive(true);
                timer.AddTime(timer.getGoodTime());
            }
            else if((float)iDishFinalMark / (float)iIngredientAmount <= 1.5) // bad
            {
                Debug.Log("BAD MARK");
                goBadMarkStamp.gameObject.SetActive(true);
                timer.AddTime(timer.getBadTime());
            }
            orderPreview();
        }
    }

    public void PopulateRecipeListUI() // populating all the recipe list ui
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
    

    bool bAllCooked() // checking boolean 
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

    public void orderPreview() // previewing the order
    {
        // calling animation to preview the order 
       
        //getting rid of the order and ingredients
        //showing up the dish, which was created
        servingPicture.gameObject.SetActive(true);
        
        //TODO: Cache values of Serve button and frames
        servingPicture.transform.GetChild(1).gameObject.SetActive(true);
        servingPicture.transform.GetChild(4).gameObject.SetActive(true);
        servingPicture.transform.GetChild(5).gameObject.SetActive(true);

    }

    public void orderServing() // serving
    {
        // calling animation to serv the order to the customer
        Debug.Log("Order is served");

        //setting up the score
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

    public void ToggleOrderUI() // switching UI method
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
        orderGuid = customOrderId;
    }

    public string GetOrderId()
    {
        return orderId;
    }

    public Guid GetOrderGuid()
    {
        return orderGuid;
    }

    public void FindIngredientButtons() 
    {
        for (int i = 0; i < currentRecipe.GetCurrentRecipe().ingredientList.Count; i++)
        {
            ingredients[i] = GameObject.FindGameObjectWithTag("Ingredients").transform.GetChild(i).gameObject.GetComponent<IngredientListElement>();
        }
    }

    public void ResetOrder() // reset
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i].ResetIngredient();
        }
        iDishFinalMark = 0;
    }

    public IngredientListElement GetIngredientFromList(int ingredient)
    {
            return ingredients[ingredient];
    }

    public IngredientListElement[] GetIngredients()
    {
	return ingredients;
    }

    public void HideOrderIcons() // hiding order UI
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

    public void IconSlideInAnimation()
    {
        orderIcon.GetComponent<Animation>().Play("SlideIn");
     
    }

    public void IconSlideOutAnimation()
    {

        orderIcon.GetComponent<Animation>().Play("SlideOut");
    }

    public void IconRollAnimation()
    {
        orderIcon.GetComponent<Animation>().Play("Roll");
    }

    private const float animationDuration = 0.5f;
    public IEnumerator ShiftIconToTheLeft()
    {
        float startingPosition = orderIcon.gameObject.GetComponent<RectTransform>().anchoredPosition.x;
        float targetPosition = -160.0f;
        Vector2 tempVector = orderIcon.gameObject.GetComponent<RectTransform>().anchoredPosition;
        
        float elapsedTime = 0;
        float progress = 0;

        targetPosition = startingPosition + targetPosition;
        
        while (progress <= 1.0f)
        {
            tempVector.x = Mathf.Lerp(startingPosition, targetPosition, progress);
            elapsedTime += Time.unscaledDeltaTime;
            progress = elapsedTime / animationDuration;
            orderIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = tempVector;
            yield return null;
        }

        targetPosition = -160.0f;
        
        tempVector.x = startingPosition + targetPosition;
        
        orderIcon.gameObject.GetComponent<RectTransform>().anchoredPosition = tempVector;

    }

}

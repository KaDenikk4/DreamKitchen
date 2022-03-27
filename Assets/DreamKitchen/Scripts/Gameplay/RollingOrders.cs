using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class RollingOrders : MonoBehaviour
{
    private int numberOfActiveOrders = 0;

    private Guid customOrderId;

    [SerializeField] private GameObject orderGameObject;

    [SerializeField] private List<Recipe> listOfAllAvailableRecipes = new List<Recipe>();

    [SerializeField] private List<GameObject> listOfOrderIcons = new List<GameObject>();
    
    private Random randomRecipeIndex = new Random();
    Order[] activeOrders;
    
    float timeSpent;

    private bool twoActive = false;
    private bool threeActive = false;

    public int GetNumOfActiveOrders()
    {
        if (twoActive && threeActive)
        {
            return 2;
        }
        else if(twoActive && !threeActive)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public Order[] GetOrdersArray()
    {
        return activeOrders;
    }

    [Header("Time Thresholds")] 
    [SerializeField] private float twoOrderThreshold;
    [SerializeField] private float threeOrderThreshold;

    private Score score;
    // Start is called before the first frame update
    void Start()
    {
        activeOrders = FindObjectsOfType<Order>();
        InitialiseOrderIcons();
        RollFirstOrder();
        score = FindObjectOfType<Score>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!score.IsGameOver())
        ManageActiveOrders();
    }

    public void RollFirstOrder()
    {
        if (numberOfActiveOrders == 0)
        {
            for (int i = 0; i < activeOrders.Length; i++)
            {
                customOrderId = Guid.NewGuid();
                //activeOrders[i].IconSlideInAnimation();
                activeOrders[i].ResetOrder();
                activeOrders[i].SetOrderId(customOrderId);
                activeOrders[i].Initialise();
                activeOrders[i].GetComponentInChildren<RecipeHolder>().SetCurrentRecipe(listOfAllAvailableRecipes[randomRecipeIndex.Next(listOfAllAvailableRecipes.Count)]);
            }
        }
        
        listOfOrderIcons[0].transform.parent.gameObject.GetComponent<Order>().IconSlideInAnimation();
    }
    
    public void RollNewOrder(string previousOrderId = "")
    {
        Order[] activeOrders = FindObjectsOfType<Order>();
        int newOrder = -1;

        int randomRecipe = randomRecipeIndex.Next(listOfAllAvailableRecipes.Count);
        
        for (int i = 0; i < activeOrders.Length; i++)
        {
            if (activeOrders[i].GetOrderId() == previousOrderId)
            {
                customOrderId = Guid.NewGuid();
                newOrder = i;
                activeOrders[i].ResetOrder();
                activeOrders[i].SetOrderId(customOrderId);
                activeOrders[i].Initialise();
                activeOrders[i].GetComponentInChildren<RecipeHolder>().SetCurrentRecipe(listOfAllAvailableRecipes[randomRecipe]);
                if (listOfAllAvailableRecipes[randomRecipe].ingredientList.Count < activeOrders[i].GetIngredients().Length)
                {
                    activeOrders[i].GetIngredients()[listOfAllAvailableRecipes[randomRecipe].ingredientList.Count].gameObject.SetActive(false);
                }
            }
        }
    }

    public void InitialiseOrderIcons()
    {
        //leave the first one active
        for (int i = 1; i < listOfOrderIcons.Count; i++)
        {
            Vector2 tempV2 = listOfOrderIcons[i].GetComponent<RectTransform>().anchoredPosition;
            tempV2.x = -720.0f;
            
            listOfOrderIcons[i].GetComponent<RectTransform>().anchoredPosition = tempV2;
        }
        
        listOfOrderIcons[0].transform.parent.gameObject.GetComponent<Order>().IconSlideInAnimation();
    }

    public void ManageActiveOrders()
    {
        timeSpent = GameObject.FindObjectOfType<CountdownTimer>().GetTimeSpent();

        if (listOfOrderIcons[0].activeSelf)
        {
            if (timeSpent > twoOrderThreshold)
            {
                if (!twoActive)
                {
                    listOfOrderIcons[1].transform.parent.gameObject.GetComponent<Order>().IconSlideInAnimation();

                    twoActive = true;
                }
            }

            if (timeSpent > threeOrderThreshold)
            {
                if (!threeActive)
                {
                    listOfOrderIcons[2].transform.parent.gameObject.GetComponent<Order>().IconSlideInAnimation();

                    threeActive = true;
                }
            }
        }
    }

}

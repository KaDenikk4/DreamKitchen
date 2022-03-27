using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // should consist from the game data:
    // 1) score of the game and player current currency
    // 2) stage of the game - menu, gameplay, end game
    // 3) items, which player can have and equip
    
    [Header("Unlocks databases")]
    [SerializeField] private List<ProductHolder> liveDatabaseOfKitchenProducts    = new List<ProductHolder>();
    [SerializeField] private List<RecipeData>    liveDatabaseOfRecipes            = new List<RecipeData>();

    private Dictionary<string, KitchenProduct> kitchenProductsNameDictionary = new Dictionary<string, KitchenProduct>();
    private Dictionary<string, Recipe> recipesDictionary = new Dictionary<string, Recipe>();
    public enum eGameStages { Menu, Gameplay, Endgame }

    public bool playedTutorial = false;
    
    private int iPlayerCurrency;

    //Time spent on the game? score of the game?

    //list of the items? how to switch between them?
    
    [Header("Player Equipment")]
    public PlayerEquipment playerEquipment;

    [SerializeField] private int standardCurrency;
    [SerializeField] private int premiumCurrency;
    
    
    [SerializeField] private int iGameStage;

    
    private void Awake()
    {
        MakeSingleton();
        PopulateDictionaryOfProducts();
    }


    private void MakeSingleton()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }


   // public void SetGameStage(int gameStage) //on click button function doesn't work with the enums so setting up the integer
    //{
        //iGameStage = gameStage;
        //switch ((eGameStages)gameStage)
        //{
        //    case eGameStages.Menu:
        //        SceneManager.LoadScene("MainMenu");
        //        Debug.Log("Menu game stage");
        //        break;

        //    case eGameStages.Gameplay:
        //        SceneManager.LoadScene("Game");
        //        Debug.Log("Gameplay game stage");
        //        break;

        //    case eGameStages.Endgame:
        //        Debug.Log("Endgame game stage");
        //        //get the data from the game player just played and put all of into the endgame card
        //        //take data from the score script
        //        break;

        //    default:
        //        Debug.LogError("using wrong type of game stage!");
        //        break;
        //}
    //}


    public int getGameStage()
    {
        return iGameStage;
    }

    public List<ProductHolder> GetProductDatabase()
    {
        return liveDatabaseOfKitchenProducts;
    }

    public List<RecipeData> GetRecipeDatabase()
    {
        return liveDatabaseOfRecipes;
    }

    public void SetProductDatabase(List<ProductHolder> savedList)
    {
        liveDatabaseOfKitchenProducts.Clear();
        for (int i = 0; i < savedList.Count; i++)
        {
            liveDatabaseOfKitchenProducts.Add(savedList[i]);
        }
    }

    public void SetRecipeDatabase(List<RecipeData> savedRecipeList)
    {
        liveDatabaseOfRecipes.Clear();
        for (int i = 0; i < savedRecipeList.Count; i++)
        {
            liveDatabaseOfRecipes.Add(savedRecipeList[i]);
        }
    }

    public void PopulateDictionaryOfProducts()
    {
        for (int i = 0; i < liveDatabaseOfKitchenProducts.Count; i++)
        {
            kitchenProductsNameDictionary.Add(liveDatabaseOfKitchenProducts[i].product.name, liveDatabaseOfKitchenProducts[i].product);    
        }

        for (int i = 0; i < liveDatabaseOfRecipes.Count; i++)
        {
            recipesDictionary.Add(liveDatabaseOfRecipes[i].recipe.name, liveDatabaseOfRecipes[i].recipe);
        }
        //foreach (KeyValuePair<string, KitchenProduct> kvp in kitchenProductsNameDictionary)
        //{
        //    Debug.Log("GameManager.cs: Dictionary check: " + kvp.Key + " " + kvp.Value.ToString());
        //}
    }

    public Dictionary<string, KitchenProduct> GetKitchenProductDictionary()
    {
        return kitchenProductsNameDictionary;
    }

    public int GetStandardCurrency()
    {
        return standardCurrency;
    }

    public int GetPremiumCurrency()
    {
        return premiumCurrency;
    }

    public void SetStandardCurrency(int amount)
    {
        standardCurrency = amount;
    }

    public void SetPremiumCurrency(int amount)
    {
        premiumCurrency = amount;
    }
    
    public void AddStandardCurrency(int amountToAdd)
    {
        standardCurrency += amountToAdd;
    }

    public void SpendStandardCurrency(int amountToSubtract)
    {
        if (amountToSubtract < standardCurrency)
        {
            standardCurrency -= amountToSubtract;    
        }
        
        //ToDo: Show player they don't have enough money.
        //ToDO: Offer watching an ad for extra 'bit' of currency (????)
        //Debug.Log("GameManager.cs: Player has insufficient amount of standard currency.");
    }
    
    public void AddPremiumCurrency(int amountToAdd)
    {
        standardCurrency += amountToAdd;
    }

    public void SpendPremiumCurrency(int amountToSubtract)
    {
        if ((premiumCurrency -= amountToSubtract) > 0)
        {
            premiumCurrency -= amountToSubtract;
        }
        else
        {
            //ToDo: Show player they don't have enough money.
            //ToDO: Offer buying premium currency (????)
            Debug.Log("GameManager.cs: Player has insufficient amount of standard currency.");
        }
    }

}

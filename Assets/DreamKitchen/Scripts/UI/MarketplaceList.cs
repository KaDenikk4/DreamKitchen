using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;

public class MarketplaceList : MonoBehaviour
{
    //TODO: Make a database with Dictionaries for unlocked and purchased items.
    
    //List using scriptable objects
    [SerializeField] private List<KitchenProduct> listOfAvailableProducts = new List<KitchenProduct>();
    [SerializeField] private List<Recipe> listOfAvailableRecipes = new List<Recipe>();
    
    
    private KitchenProduct currentlyFocusedProduct;

    //List of created buttons
    [SerializeField] private List<GameObject> listOfButtons = new List<GameObject>();

    //Product database that allows for modifying the objects in it;
    [SerializeField] private List<ProductHolder> liveProductDatabase;

    [SerializeField] private List<MarketplaceRecipeStruct> liveRecipesDatabase;
    
    private ProductHolder tempProductStruct;

    [SerializeField] private GameObject goListParent;
    [SerializeField] private GameObject goListElementPrefab;
    [SerializeField] private Scrollbar scrollBar;
    [SerializeField] private GameObject goBuyButtonPricePanel;
        
    [SerializeField] private Rect contentBoxForButtons;

    private ProductHolder tempProductHolder;

    private GameManager gm;
    
    private enum productStatus
    {
        NotPurchased  = 0,
        Purchased     = 1,
        Equipped      = 2
    }

    private productStatus focusedProductStatus;
    
    private void OnValidate()
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        gm = FindObjectOfType<GameManager>();
        liveProductDatabase = FindObjectOfType<GameManager>().GetProductDatabase();
        contentBoxForButtons = goListParent.GetComponent<RectTransform>().rect;
        PrepareListContent();
        PopulateListOfButtons();
        PopulateListElementsWithProducts();
        
        //Setting first element on the list to focus.
        listOfButtons[0].GetComponent<MarketplaceListing>().SetProductListing(listOfAvailableProducts[0]);
        SetCurrentlyFocusedProduct(listOfAvailableProducts[0]);
        scrollBar.value = 1.0f;

        liveProductDatabase = FindObjectOfType<GameManager>().GetProductDatabase();
        
        if (listOfAvailableProducts.Count != liveProductDatabase.Count)
        {
            liveProductDatabase.Clear();
            
            tempProductStruct = new ProductHolder();
            
            for (int i = 0; i < listOfAvailableProducts.Count; i++)
            {
                tempProductStruct.product = listOfAvailableProducts[i];
                liveProductDatabase.Add(tempProductStruct);
            }
        }
    }

    void PrepareListContent()
    {
        //Adjust the height of content box for the amount of buttons.        
        contentBoxForButtons.height = goListElementPrefab.GetComponent<RectTransform>().rect.height *
                                      listOfAvailableProducts.Count;

        goListParent.GetComponent<RectTransform>().sizeDelta = new Vector2(contentBoxForButtons.width, contentBoxForButtons.height);
     

        //Instantiating buttons.
        //ToDo: FOR PERFORMANCE - replace with preloaded buttons and recycle given few instead of instantiating all of them.
        for (int i = 0; i < listOfAvailableProducts.Count; i++)
        {
            Instantiate(goListElementPrefab, goListParent.transform);
        }
    }

    void PopulateListOfButtons()
    {
        for (int i = 0; i < goListParent.transform.childCount; i++)
        {
            listOfButtons.Add(goListParent.transform.GetChild(i).gameObject);
        }
    }

    void PopulateListElementsWithProducts()
    {
        for (int i = 0; i < listOfButtons.Count; i++)
        {
            listOfButtons[i].GetComponent<MarketplaceListing>().SetProductListing(listOfAvailableProducts[i]);
        }
    }

    void PopulateListElementsWithRecipes()
    {
        for (int i = 0; i < listOfButtons.Count; i++)
        {
            listOfButtons[i].GetComponent<MarketplaceListing>().SetProductListing(listOfAvailableRecipes[i]);
        }
    }

    public List<ProductHolder> GetDatabaseOfProducts()
    {
        return liveProductDatabase;
    }

    public KitchenProduct GetCurrentProduct()
    {
        return currentlyFocusedProduct;
    }

    public void SetCurrentlyFocusedProduct(KitchenProduct product)
    {
        currentlyFocusedProduct = product;
    }
    
    public void BuyProduct()
    {
        for (int i = 0; i < liveProductDatabase.Count; i++)
        {
            if (liveProductDatabase[i].product == currentlyFocusedProduct)
            {
                tempProductStruct = liveProductDatabase[i];
                tempProductStruct.productPurchased = true;
                liveProductDatabase[i] = tempProductStruct;
                return;
            }
        }
    }

    public void CheckProductStatus()
    {
        liveProductDatabase = FindObjectOfType<GameManager>().GetProductDatabase();
        
        for (int i = 0; i < liveProductDatabase.Count; i++)
        {
            if (currentlyFocusedProduct == liveProductDatabase[i].product)
            {
                tempProductHolder = liveProductDatabase[i];

                if (liveProductDatabase[i].productPurchased)
                {
                    focusedProductStatus = productStatus.Purchased;
                }

                if (liveProductDatabase[i].productEquipped)
                {
                    focusedProductStatus = productStatus.Equipped;
                }

                switch (focusedProductStatus)
                {
                    case productStatus.NotPurchased:
                        if (gm.GetStandardCurrency() > currentlyFocusedProduct.productPrice)
                        {
                            FindObjectOfType<GameManager>().SpendStandardCurrency(currentlyFocusedProduct.productPrice);
                            tempProductHolder.productPurchased = true;
                            FindObjectOfType<GameManager>().GetProductDatabase()[i] = tempProductHolder;
                            focusedProductStatus = productStatus.Purchased;
                            goBuyButtonPricePanel.GetComponent<TextMeshProUGUI>().text = "Equip";
                        }
                        
                        break;

                    case productStatus.Purchased:
                        tempProductHolder.productEquipped = true;
                        FindObjectOfType<GameManager>().GetProductDatabase()[i] = tempProductHolder;
                        FindObjectOfType<GameManager>().playerEquipment.equippedKnife = currentlyFocusedProduct;
                        focusedProductStatus = productStatus.Equipped;
                        goBuyButtonPricePanel.GetComponent<TextMeshProUGUI>().text = "Equipped";
                        break;

                    case productStatus.Equipped:
                        break;

                    default:
                        break;
                }
            }
        }
    }

    private bool recipesDisplayed = false;
    
    
    public void SwitchToRecipes()
    {
        if (!recipesDisplayed)
        {
            PopulateListElementsWithRecipes();
            
            recipesDisplayed = true;
        }
    }

    public void SwitchToBackgrounds()
    {
        if (recipesDisplayed)
        {
            PopulateListElementsWithProducts();
            SetCurrentlyFocusedProduct(listOfAvailableProducts[0]);
            recipesDisplayed = false;
        }
    }
}

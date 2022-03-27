using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MarketplaceListing : MonoBehaviour
{
    [SerializeField]
    private KitchenProduct  productToDisplay;
    private Recipe          recipeToDisplay;
    private Sprite          productIconToDisplay;
    private Sprite          productIconOnList;
    private string          productNameToDisplay;
    private string          productDescriptionToDisplay;
    private int             productPriceToDisplay;

    private bool displayRecipes = false;
    
    //On Available Products panel
    [SerializeField] private GameObject goListIcon;
    [SerializeField] private GameObject goListName;
    
    //On description panel
    [SerializeField] private GameObject goIconPanel;
    [SerializeField] private GameObject goDescriptionPanel;
    [SerializeField] private GameObject goPricePanel;

    [SerializeField] private MarketplaceList marketplaceDatabaseScript;

    private void Start()
    {
        marketplaceDatabaseScript = GameObject.FindObjectOfType<MarketplaceList>();
    }

    public void SetProductListing(KitchenProduct productToSet)
   {
       productToDisplay = productToSet;

       goIconPanel          = GameObject.Find("MPProductIcon");
       goDescriptionPanel   = GameObject.Find("MPDescription");
       goPricePanel         = GameObject.Find("MPPrice");
       
       goListIcon.GetComponent<Image>().sprite                  = productToDisplay.productSprite;
       goListName.GetComponent<TextMeshProUGUI>().text          = productToDisplay.productName;
       goIconPanel.GetComponent<Image>().sprite                 = productToDisplay.productSprite;
       goDescriptionPanel.GetComponent<TextMeshProUGUI>().text  = productToDisplay.productDescription;
       goPricePanel.GetComponent<TextMeshProUGUI>().text        = productToDisplay.productPrice.ToString();

       displayRecipes = false;
   }
   
    public void SetProductListing(Recipe recipeToSet)
    {
        recipeToDisplay = recipeToSet;

        goIconPanel          = GameObject.Find("MPProductIcon");
        goDescriptionPanel   = GameObject.Find("MPDescription");
        goPricePanel         = GameObject.Find("MPPrice");
       
        goListIcon.GetComponent<Image>().sprite                  = recipeToDisplay.recipeIcon;
        goListName.GetComponent<TextMeshProUGUI>().text          = recipeToDisplay.recipeName;
        goIconPanel.GetComponent<Image>().sprite                 = recipeToDisplay.recipeIcon;
        goDescriptionPanel.GetComponent<TextMeshProUGUI>().text  = recipeToDisplay.recipeDescription;
        goPricePanel.GetComponent<TextMeshProUGUI>().text        = recipeToDisplay.recipePrice.ToString();

        displayRecipes = true;
    }
    
   public void FocusThisProductListing()
   {
       if (!displayRecipes)
       {
          goListIcon.GetComponent<Image>().sprite                  = productToDisplay.productSprite;
          goListName.GetComponent<TextMeshProUGUI>().text          = productToDisplay.productName;
          goIconPanel.GetComponent<Image>().sprite                 = productToDisplay.productSprite;
          goDescriptionPanel.GetComponent<TextMeshProUGUI>().text  = productToDisplay.productDescription;
          goPricePanel.GetComponent<TextMeshProUGUI>().text        = productToDisplay.productPrice.ToString(); 
       }

       if (displayRecipes)
       {
           goListIcon.GetComponent<Image>().sprite                  = recipeToDisplay.recipeIcon;
           goListName.GetComponent<TextMeshProUGUI>().text          = recipeToDisplay.recipeName;
           goIconPanel.GetComponent<Image>().sprite                 = recipeToDisplay.recipeIcon;
           goDescriptionPanel.GetComponent<TextMeshProUGUI>().text  = recipeToDisplay.recipeDescription;
           goPricePanel.GetComponent<TextMeshProUGUI>().text        = recipeToDisplay.recipePrice.ToString();
       }
       
       
       marketplaceDatabaseScript.SetCurrentlyFocusedProduct(productToDisplay);

       for (int i = 0; i < marketplaceDatabaseScript.GetDatabaseOfProducts().Count; i++)
       {
           if (productToDisplay == marketplaceDatabaseScript.GetDatabaseOfProducts()[i].product)
           {
               if (marketplaceDatabaseScript.GetDatabaseOfProducts()[i].productPurchased)
               {
                   goPricePanel.GetComponent<TextMeshProUGUI>().text = "Sold";
               }
           }
       }
       
   }
}

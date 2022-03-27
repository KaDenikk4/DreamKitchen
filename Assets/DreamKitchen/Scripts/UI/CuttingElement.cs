using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class CuttingElement : MonoBehaviour
{
    //setting up variables
    [SerializeField] private float fSpeed;
    Rigidbody2D rb;
    private Vector2 screenBounds;
    private string elementName;

    private Sprite currentSprite;
    private string currentName;
    private CuttingMinigameManager cuttingMinigameManager;
   [SerializeField] private SpriteRenderer spriteRenderer;

    private System.Random rnd = new System.Random();

    [SerializeField] public Sprite[] ingredientSprites; // to make it look random (that's why we going to gett all ingredients in the current orders)
    [SerializeField] private string[] ingredientNames;
    [SerializeField] private Order[] currentOrders;
    [SerializeField] private IngredientListElement[] currentIngredients;
    [SerializeField] private Sprite[] currentIngredientSprites;

    private int index;
    private int otherIndex;

    public int Index { get => index; set => index = value; }
    public int OtherIndex { get => otherIndex; set => otherIndex = value; }

    void Start()
    {
        cuttingMinigameManager = FindObjectOfType<CuttingMinigameManager>(); // setting the cutting mingame
        rb = this.GetComponent<Rigidbody2D>(); // rigidbody
        rb.velocity = new Vector2(-fSpeed, 0); // speed of the element
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z)); // area of the minigame


        for (int i = 0; i < ingredientSprites.Length; i++)
        {
            if(cuttingMinigameManager.RequiredName == ingredientSprites[i].name)
            {
                currentIngredientSprites[0] = ingredientSprites[i]; // setting one of the sprites to always be correct
            }
        }       


        currentIngredientSprites[1] = ingredientSprites[cuttingMinigameManager.Index]; // setting random sprite
        currentIngredientSprites[2] = ingredientSprites[cuttingMinigameManager.OtherIndex]; // setting random sprite

        int currentIndex = rnd.Next(0, currentIngredientSprites.Length); 
        spriteRenderer.sprite = currentIngredientSprites[currentIndex]; // calling random sprite out of 3 existing sprites

        this.transform.localScale = new Vector3(.75f, .75f, .75f); // scale
    }

    void Update()
    {
        if (transform.position.x < screenBounds.x)
        {
            Destroy(this.gameObject, 2f); // deleting sprites after certain time
        }        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //what happens when sprte collides with the blade
        if(collision.tag == "Blade")
        {
            Debug.Log("Object was hit");
            rb.gravityScale = 2; // scaling up the gravity to make them fall
            //adding a score to the minigame
            GetComponent<AudioSource>().Play(); // audio
            //setting up a score of the mingame
            cuttingMinigameManager.ScoreCount++; 
            if (cuttingMinigameManager.RequiredName == spriteRenderer.sprite.name)
            {
                cuttingMinigameManager.GoodMarks++;
            }
            else
            {
                cuttingMinigameManager.BadMarks++;
            }
            
        }
    }

    
}

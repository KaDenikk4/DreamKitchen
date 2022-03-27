 using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievemntManager : MonoBehaviour
{

    [SerializeField] private GameObject goAchievementPrefab;

    [SerializeField] private Sprite[] AchievementsSprites;

    private AchievementButton activeButton;

    [SerializeField] private ScrollRect scrollRect;

    [SerializeField] private GameObject achievementMenu;

    [SerializeField] private GameObject visualAchievement;

    [SerializeField] private Dictionary<string, Achievement> achievements = new Dictionary<string, Achievement>();

    private static AchievemntManager instance;

    public static AchievemntManager Instance
    {
        get // making an instance of the current class
        {
            if(instance == null)
            {
                instance = GameObject.FindObjectOfType<AchievemntManager>();
            }
            return AchievemntManager.instance;
        }
    }

    private void Awake()
    {
        MakeSingleton();
    }
    void Start()
    {
        activeButton = GameObject.Find("GeneralCategoryButton").GetComponent<AchievementButton>(); 

        CreateAchievement("GeneralCategory", "Press W", "You have to press w!", 0); // making an achievement here

        foreach(GameObject achievementList in GameObject.FindGameObjectsWithTag("AchievementList")) // making all achievements active
        {
            achievementList.SetActive(false); 
        }

        achievementMenu.SetActive(false);
        activeButton.CLick();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) // to call the achievement menu
        {
            achievementMenu.SetActive(!achievementMenu.activeSelf);
        }
        if(Input.GetKeyDown(KeyCode.W)) // to earn first achievement
        {
            EarnAchievement("Press W"); 
        }
    }


    private void MakeSingleton() // checking that there is the only one achievement system
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void EarnAchievement(string title) // setting achievement to be earned
    {
        if(achievements[title].EarnAchievement())
        {
            GameObject achievement = (GameObject)Instantiate(visualAchievement); // calling the achievement visual

            SetAchievementInfo("EarnAchievementCanvas", achievement, title);
            StartCoroutine(HideAchievement(achievement)); // hiding achievement visual
        }
    }

    public IEnumerator HideAchievement(GameObject achievement) //destroying achivement visual
    {
        yield return new WaitForSeconds(3);
        Destroy(achievement);
    }

    public void CreateAchievement(string parent, string title, string description, int spriteIndex) // achivement creation method
    {
        GameObject achievement = (GameObject)Instantiate(goAchievementPrefab); // making new achievement

        Achievement newAchievement = new Achievement(title, description, spriteIndex, achievement); //calling achievement class to make it

        achievements.Add(title, newAchievement);

        SetAchievementInfo(parent, achievement, title);
    }

    public void SetAchievementInfo(string parent, GameObject achievement, string title) // setting achievement information
    {
        achievement.transform.SetParent(GameObject.Find(parent).transform);
        achievement.transform.localScale = new Vector3(1, 1, 1); //scale
        achievement.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = title; //title
        achievement.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = achievements[title].Description; //description
        achievement.transform.GetChild(2).GetComponent<Image>().sprite = AchievementsSprites[achievements[title].SpriteIndex]; // image
    }

    public void ChangeCategory(GameObject button) // setting up achievement category
    {
        AchievementButton achievementButton = button.GetComponent<AchievementButton>(); // setting specific button for the specific achievement

        scrollRect.content = achievementButton.GetAchievementList().GetComponent<RectTransform>();

        activeButton.CLick();
        achievementButton.CLick();

        activeButton = achievementButton;
    }
}

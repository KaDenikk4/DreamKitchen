using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementButton : MonoBehaviour
{
    [SerializeField] private GameObject achievementList;

    [SerializeField] private Sprite neutral, highlited; // those things are going to be used to track which tab player is using right now
    private bool clicked;

    public GameObject GetAchievementList()
    {
        return achievementList;
    }

    public void CLick()
    {
        //stting achievemnt list UI
        clicked = !clicked;

        if(clicked)
        {
            achievementList.SetActive(true);
        }
        else
        {
            achievementList.SetActive(false);
        }

    }
}

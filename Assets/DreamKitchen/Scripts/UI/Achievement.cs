using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Achievement
{
    private string name;

    private string description;

    private bool unlocked;

    private int spriteIndex;

    private GameObject achievementReference;

    public Achievement(string name, string description, int spriteIndex, GameObject achievementReference)
    {
        //constructor
        this.name = name;
        this.description = description;
        this.spriteIndex = spriteIndex;
        this.unlocked = false;
        this.achievementReference = achievementReference;
    }

    public string Name { get => name; set => name = value; }
    public string Description { get => description; set => description = value; }
    public bool Unlocked { get => unlocked; set => unlocked = value; }
    public int SpriteIndex { get => spriteIndex; set => spriteIndex = value; }
    public GameObject AchievementReference { get => achievementReference; set => achievementReference = value; }

    public bool EarnAchievement()
    {
        // check if achievement is unlocked
        if(!unlocked)
        {
            unlocked = true;
            return true;
        }
        return false;
    }
}

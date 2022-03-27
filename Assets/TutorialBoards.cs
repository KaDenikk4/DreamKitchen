using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialBoards : MonoBehaviour
{
    public List<GameObject> listOfTutorialTextBoxes = new List<GameObject>();
    public int tutorialStage = 1;

    private void Start()
    {
        GameObject.FindObjectOfType<GameManager>().playedTutorial = true;
    }
    
    public void SwitchBoards()
    {
        tutorialStage++;

        switch (tutorialStage)
        {
            case 2:
                listOfTutorialTextBoxes[0].SetActive(false);
                listOfTutorialTextBoxes[1].SetActive(true);
                break;

            case 3:
                listOfTutorialTextBoxes[1].SetActive(false);
                listOfTutorialTextBoxes[2].SetActive(true);
                break;

            case 4:
                listOfTutorialTextBoxes[2].SetActive(false);
                listOfTutorialTextBoxes[3].SetActive(true);
                break;

            case 5:
                listOfTutorialTextBoxes[3].SetActive(false);
                listOfTutorialTextBoxes[4].SetActive(true);
                break;

            case 6:
                listOfTutorialTextBoxes[4].SetActive(false);
                listOfTutorialTextBoxes[5].SetActive(true);
                break;
        }
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

}

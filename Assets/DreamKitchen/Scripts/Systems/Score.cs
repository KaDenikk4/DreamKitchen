using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private int iEarnedCurrency;
    private int iNumberOfOrders;

    private float iTotalMark;
    private float fAverageMark;

    [SerializeField] private CountdownTimer timer;

    [SerializeField] private TextMeshProUGUI TimeSpent;
    [SerializeField] private TextMeshProUGUI OrdersCompleted;
    [SerializeField] private Image AverageMark;
    
    [SerializeField] private List<GameObject> listOfUiToDisable = new List<GameObject>();
    private bool gameOver = false;
    
    private void Start()
    {
        //reset score
        gameOver = false;
        ResetScore();
    }

    public void SetTheEndGameCard()
    {
        //setting the UI
        TimeSpent.text = ("Time spent: " + timer.GetTimeSpent().ToString("0"));
        OrdersCompleted.text = ("Orders completed: " + iNumberOfOrders.ToString("0"));

        if (GetAverageMark() == 3)
        {
            AverageMark.GetComponent<Image>().color = Color.green;
        }
        else if(GetAverageMark() >= 1.5 && GetAverageMark() < 3)
        {
            AverageMark.GetComponent<Image>().color = Color.yellow;
        }
        else if(GetAverageMark() < 1.5)
        {
            AverageMark.GetComponent<Image>().color = Color.red;
        }
    }

    public int GetEarnedCurrency()
    {
        return iEarnedCurrency;
    }

    public int GetNumberOfOrders()
    {
        return iNumberOfOrders;
    }
    public void SetEarnedCurrency(int iEarned)
    {
        iEarnedCurrency = iEarned;
    }

    public void SetNumberOfOrders(int iNumber)
    {
        iNumberOfOrders = iNumber;
    }

    public void AddToTheTotalMark(float mark)
    {
        iTotalMark += mark;
    }

    public void AddToTheOrderAmount(int order)
    {
        iNumberOfOrders += order;
    }

    public float GetAverageMark()
    {
        //calculating average mark
        fAverageMark = (float)iTotalMark / (float)iNumberOfOrders;
        return fAverageMark;
    }

    public void ResetScore()
    {
        //Score reset
        fAverageMark = 0;
        iTotalMark = 0;
        iNumberOfOrders = 0;
    }

    public void DisableUiOnGameOver()
    {
        //UI disabling on the game over
        gameOver = true;
        for (int i = 0; i < listOfUiToDisable.Count; i++)
        {
            listOfUiToDisable[i].SetActive(false);
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CountdownTimer : MonoBehaviour
{

    float fCurrentTime;

    [SerializeField]
    private float fStartTime;

    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private float fBad;

    [SerializeField]
    private float fGood;

    [SerializeField]
    private float fPerfect;

    [SerializeField]
    private float fAwesome;

    private bool bShouldWork = true;

    private float iTimeSpent;


    [SerializeField] private Score score;
    
    void Start()
    {
        fCurrentTime = fStartTime;
        iTimeSpent = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ZenTimer();
    }

    void ZenTimer()
    {
        if (bShouldWork)
        {
            fCurrentTime -= 1 * Time.deltaTime; // counting down every second

            iTimeSpent += 1 * Time.deltaTime; 

            timerText.text = fCurrentTime.ToString("0"); // changing float to string
            ReachedZero();
        }
    }

    public void AddTime(float fAdditionalTime)
    {
        fCurrentTime += fAdditionalTime;
    }

    public void ReachedZero()
    {
        if (fCurrentTime <= 0)
        {
            fCurrentTime = 0; // checking end of the timer
           
            score.transform.GetChild(0).gameObject.SetActive(true);
            score.SetTheEndGameCard();
            score.DisableUiOnGameOver();
           
            bShouldWork = false;
        }
    }


    public float GetTimeSpent()
    {
        return iTimeSpent;
    }
    public void StopTheTimer()
    {
        bShouldWork = false;
    }
    public void StartTheTimer()
    {
        bShouldWork = true;
    }

    public float getBadTime()
    {
        return fBad;
    }

    public float getGoodTime()
    {
        return fGood;
    }
    public float getPerfectTime()
    {
        return fGood;
    }
}

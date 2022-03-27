using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeDisplay : MonoBehaviour
{

    [SerializeField]    private GameObject displayTime;
    [SerializeField]    private GameObject displayCurrency;
    

    void Start()
    {
        displayCurrency.GetComponent<TextMeshProUGUI>().text = FindObjectOfType<GameManager>().GetStandardCurrency().ToString();
    }

    // Update is called once per frame
    void Update()
    {
        displayTime.GetComponent<TextMeshProUGUI>().text = System.DateTime.Now.ToString("HH:mm");
    }
}

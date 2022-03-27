using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeButton : MonoBehaviour
{
    private Order thisOrder;

    private RollingOrders rollingOrdersScript;
    
    private List<GameObject> arrayOfServeScreenChildren = new List<GameObject>();
    
    private string servedOrderId;
    
    
    // Start is called before the first frame update
    void Start()
    {
        thisOrder = gameObject.transform.parent.gameObject.GetComponent<Order>();

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            arrayOfServeScreenChildren.Add(gameObject.transform.GetChild(i).gameObject);
        }

        servedOrderId = thisOrder.GetOrderId();
        
    }

    public void EnableServingUI()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            arrayOfServeScreenChildren[i].SetActive(true);
        }
    }

    public void Serve()
    {
        thisOrder.orderServing();

        thisOrder.ToggleOrderUI();
        
        rollingOrdersScript = FindObjectOfType<RollingOrders>();
        
        thisOrder.IconRollAnimation();
        
        GetComponent<AudioSource>().Play();

        StartCoroutine("DelayReroll", 0.75f);
        
    }

    private IEnumerator DelayReroll(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            arrayOfServeScreenChildren[i].SetActive(false);
        }
        
        rollingOrdersScript.RollNewOrder(thisOrder.GetOrderId());
        gameObject.SetActive(false);
    }

}

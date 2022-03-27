using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{

    [SerializeField] private UnityEngine.UI.Button mainMenuButton;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        mainMenuButton = gameObject.GetComponent<UnityEngine.UI.Button>();
        mainMenuButton.onClick.AddListener(LoadMenus);
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    // Update is called once per frame
    void LoadMenus()
    {
        //if(mainMenuButton)
        //{
        //    gm.SetGameStage(0);
        //}
    }
}

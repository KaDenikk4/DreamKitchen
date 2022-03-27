using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{

    public AudioMixer myAudioMixer;
    private bool SFXon = true;
    private bool BGMon = true;

    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        //mainMenuButton = gameObject.GetComponent<UnityEngine.UI.Button>();
        //mainMenuButton.onClick.AddListener(LoadMenus);
        gm = (GameManager)FindObjectOfType(typeof(GameManager));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadGame()
    {
        //    if(playButton)
        //    gm.SetGameStage(1);

        if (!gm.playedTutorial)
        {
            SceneManager.LoadScene("TutorialTest");    
        }
        else
        {
            SceneManager.LoadScene("Game");
        }

    }

    public void LoadMenus()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadMarketplace()
    {
        SceneManager.LoadScene("Marketplace");
    }

    public void ExitApp()
    {
        Application.Quit();
    }

    public void ToggleSFX()
    {
        if (SFXon == true)
        {
            myAudioMixer.SetFloat("SFX", -80.0f);
            SFXon = false;
        }
        else
        {
            myAudioMixer.SetFloat("SFX", 0.0f);
            SFXon = true;
        }
        
    }

    public void ToggleBGM()
    {
        if (BGMon == true)
        {
            GameObject.FindObjectOfType<AudioManager>().ToggleBGMTrack();
            myAudioMixer.SetFloat("BGM", -80.0f);
            BGMon = false;
        }
        else
        {
            GameObject.FindObjectOfType<AudioManager>().ToggleBGMTrack();
            myAudioMixer.SetFloat("BGM", 0.0f);
            BGMon = true;
        }
    }
}

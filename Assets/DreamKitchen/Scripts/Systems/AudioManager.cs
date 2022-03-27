using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    AudioMixer mySFXGroup;

    private static bool created = false;

    private static AudioManager instance;

    [SerializeField] private AudioSource bgmAudioSource;

    private bool bgmEnabled;
    
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        bgmEnabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleBGMTrack()
    {
        if (bgmEnabled)
        {
            bgmAudioSource.Stop();
            bgmEnabled = false;
        }
        else if (!bgmEnabled)
        {
            bgmAudioSource.Play();
            bgmEnabled = true;
        }
        
    }

}

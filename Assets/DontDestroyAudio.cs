using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyAudio : MonoBehaviour {

    static bool AudioBegin = false;
    AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        
    }

     void Update()
    {
        if(Application.loadedLevelName == "MainMenu")
        {
            audioSource.Stop();
            AudioBegin = false;
        }
    }
}

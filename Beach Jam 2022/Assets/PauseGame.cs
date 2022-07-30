using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance;
    public bool pauseActive;
    public GameObject pauseScreen;
    public PlayerController pController;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseActive)
            {
                pauseActive = true;
                pController.inputDisabled = true;
                StartPausing();
            }
            else
            {
                pauseActive = false;
                pController.inputDisabled = false;
                StopPausing();
            }

        }
    }

    void StartPausing()
    {
        Time.timeScale = 0;
        pauseScreen.SetActive(true);
    }

    void StopPausing()
    {
        Time.timeScale = 1;
        pauseScreen.SetActive(false);
    }
}

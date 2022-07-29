using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseGame : MonoBehaviour
{
    public static PauseGame Instance;
    public bool pauseActive;
    public GameObject pauseScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pauseActive)
            {
                pauseActive = true;
                StartPausing();
            }
            else
            {
                pauseActive = false;
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

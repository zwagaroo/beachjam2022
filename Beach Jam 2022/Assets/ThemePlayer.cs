using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemePlayer : MonoBehaviour
{
    public AudioSource theme;
    // Start is called before the first frame update
    void Start(){   

        theme.Play();
    }

    private void OnDestroy() {
        theme.Stop();
    }
}

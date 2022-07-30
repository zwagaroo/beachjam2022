using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MusicPlayer : MonoBehaviour
{

    public List<string> BGMList;
    public string nextUp;
    public List<string> alreadyPlayed;
    public AudioManager am;
    // Start is called before the first frame update
    void Start()
    {

        am = FindObjectOfType<AudioManager>();
        nextUp = "Onwards";
        StartCoroutine(CheckBGM());
    }

    // Update is called once per frame

        IEnumerator CheckBGM()
        {
            for (; ; )
            {
                PlayBGM();
                yield return new WaitForSeconds(1f);
            }
        }

        void PlayBGM()
        {
            foreach (Sound s in am.currentlyPlaying)
            {
                foreach (string name in BGMList)
                {
                    if (s.name == name && s.source.isPlaying == true) { return; }
                }
            }

            print("works");

            am.GetComponent<AudioManager>().Play(nextUp);
            alreadyPlayed.Add(nextUp);
            nextUp = null;

            foreach (string name in BGMList)
            {
                if (alreadyPlayed.FirstOrDefault(x => x == name) == null)
                {
                    nextUp = name;
                    break;
                }
            }

            if (nextUp == null)
            {
                alreadyPlayed.Clear();
                nextUp = "Onwards";
            }

    }
}

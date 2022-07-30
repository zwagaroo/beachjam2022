using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;


public class MusicPlayer : MonoBehaviour
{
    
    public List<string> BGMList;
    public string nextUp;
    public List<string> alreadyPlayed;
    public AudioManager am;
    public bool BossLevel;
    public bool endSeq;
    // Start is called before the first frame update
    
    void Start()
    {

        am = FindObjectOfType<AudioManager>();
        if(BossLevel)
        {
            foreach(Sound s in am.sounds){
                am.Stop(s.name);
            }
            nextUp = "TheFireAndTheSea";
        }
        else if(endSeq){
            foreach(Sound s in am.sounds){
                am.Stop(s.name);
            }
            nextUp = "Theme";
        }
        else {

            var rand = UnityEngine.Random.Range(0,BGMList.Count);
            nextUp = BGMList[rand];
        }

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
            foreach (string name in BGMList)
            {
                if (Array.Find(am.GetComponent<AudioManager>().sounds, sound => sound.name == name).source.isPlaying == true) { print("hello"); return; }
            }

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
                nextUp = BGMList[0];
            }

    }


    
}

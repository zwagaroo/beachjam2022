using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static float globalVolume;
    public Sound[] sounds;
    public static AudioManager instance;

    void Awake(){

        DontDestroyOnLoad(gameObject);

        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
            return;
        }

        foreach(Sound s in sounds){
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name){
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
    }
}

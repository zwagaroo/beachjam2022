using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class AudioManager : MonoBehaviour
{

    public static float globalVolume;
    public Sound[] sounds;
    public static AudioManager instance;

    public List<Sound> currentlyPlaying = new List<Sound>();
    public List<Sound> currentlyPaused = new List<Sound>();

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

    IEnumerator Clean(string name){
            yield return new WaitForSeconds(Array.Find(sounds, sound => sound.name == name).clip.length + 1);
            foreach(Sound s in currentlyPlaying.ToList()){
                if(s.source.isPlaying == false){
                    currentlyPlaying.Remove(s);
                }
            }
    }

    public void Play(string name){
        Array.Find(sounds, sound => sound.name == name).source.Play();
        currentlyPlaying.Add(Array.Find(sounds, sound => sound.name == name));
        StartCoroutine(Clean(name));

/*         Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Play();
        currentlyPlaying.Add(s);
        StartCoroutine(Clean(name)); */
    }

    public void Stop(string name){
        Array.Find(sounds, sound => sound.name == name).source.Stop();
        if(currentlyPlaying.FirstOrDefault(x => x.name == name) != null)
        {
            currentlyPlaying.Remove(Array.Find(sounds, sound => sound.name == name));
        }
/*         currentlyPlaying.Find(s => s.name == name).source.Stop();
        currentlyPlaying.Remove(currentlyPlaying.Find(s => s.name == name)); */
    }

    public void Pause(string name){
        Array.Find(sounds, sound => sound.name == name).source.Pause();
/*         currentlyPlaying.Find(s => s.name == name).source.Pause();
        currentlyPaused.Add(currentlyPlaying.Find(s => s.name == name));
        currentlyPlaying.Remove(currentlyPlaying.Find(s => s.name == name)); */
    }

    public void UnPause(string name){
        Array.Find(sounds, sound => sound.name == name).source.UnPause();
/*         currentlyPaused.Find(s => s.name == name).source.UnPause();
        currentlyPlaying.Add(currentlyPlaying.Find(s => s.name == name));
        currentlyPaused.Remove(currentlyPlaying.Find(s => s.name == name)); */
 
    }

}

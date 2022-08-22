using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sound {
    
    public AudioClip clip;
    public AudioSource src;
    public float volume = 0.1f;

    public void PlayOnce(){
        src.volume = volume;
        src.loop = false;
        src.clip = clip; 
        src.Play();
    }
    public void PlayLoop(){
        src.volume = volume;
        src.loop = true;
        src.clip = clip; 
        src.Play();
    }

    public void StopSource(){
        src.Stop();
    }


    
}

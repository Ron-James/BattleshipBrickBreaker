using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sound {
    
    public AudioClip clip;
    public AudioSource src;
    public float volume = 0.1f;

    public void PlayOnce(){
        if(clip == null || src == null){
            Debug.Log("no source or clip");
            return;
        }
        src.volume = volume;
        src.loop = false;
        src.clip = clip; 
        src.Play();
    }
    public void PlayLoop(){
        if(clip == null || src == null){
            Debug.Log("no source or clip");
            return;
        }
        src.volume = volume;
        src.loop = true;
        src.clip = clip; 
        src.Play();
    }

    public void StopSource(){
        src.Stop();
    }


    
}

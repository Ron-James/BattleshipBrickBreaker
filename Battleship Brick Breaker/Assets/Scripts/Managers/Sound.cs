using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class Sound {
    public enum SoundCategory{
        sfx = 0,
        music = 1
    }
    public AudioClip clip;
    public AudioSource src;
    public float volume = 0.1f;
    public SoundCategory soundCategory = SoundCategory.sfx;

    public void PlayOnce(){
        if(clip == null || src == null){
            Debug.Log("no source or clip");
            return;
        }
        float volumeMod = 1f;
        switch((int) soundCategory){
            case 0:
                volumeMod = SettingsManager.sfxVolume;
            break;
            case 1:
                volumeMod = SettingsManager.musicVolume;
            break;
        }
        src.volume = volume * volumeMod * SettingsManager.globalVolume;
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

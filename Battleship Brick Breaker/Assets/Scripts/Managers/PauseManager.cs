using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : Singleton<PauseManager>
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject settingsMenu;
    public static bool isPaused;

    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseGameplay(){
        isPaused = true;
        Time.timeScale = 0;
        
    }
    

    public void ResumeGameplay(){
        Time.timeScale = 1;
        isPaused = false;
        
        
    }

    public void OpenPauseMenu(){
        PauseGameplay();
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void CloseMenus(){
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(false);
        ResumeGameplay();
        Debug.Log("SHould close here");
    }

    public void OpenSettingsMenu(){
        pauseMenu.SetActive(false);
        settingsMenu.SetActive(true);
    }


}

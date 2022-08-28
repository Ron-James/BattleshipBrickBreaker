using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PauseGameplay(){
        isPaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGameplay(){
        isPaused = false;
        Time.timeScale = 1;
    }
}

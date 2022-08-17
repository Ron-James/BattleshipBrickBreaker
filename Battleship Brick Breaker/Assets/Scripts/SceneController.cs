using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : Singleton<SceneController>
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLocalCoop(){
        SceneManager.LoadScene(1);
    }
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }
    public void QuitGame(){
        Application.Quit();
    }
}

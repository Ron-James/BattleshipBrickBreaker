using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] GameObject loadingScreen;
    [SerializeField] Image loadingBar;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadLocalCoop(){
        LoadScene(2);
    }
    public void RestartCurrentScene(){
        Scene scene = SceneManager.GetActiveScene(); 
        SceneManager.LoadScene(scene.name);
    }
    public void LoadTutorial(){
        SceneManager.LoadScene(1);
    }
    public void LoadMenu(){
        SceneManager.LoadScene(0);
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void LoadScene(int sceneId){
        StartCoroutine(LoadSceneAsync(sceneId));
    }
    IEnumerator LoadSceneAsync(int sceneId){

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        loadingScreen.SetActive(true);

        while(!operation.isDone){
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);
            loadingBar.fillAmount = progressValue;
            yield return null;
        }

    }
}

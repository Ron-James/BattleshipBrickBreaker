using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    [SerializeField] GameObject[] prompts = new GameObject[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenPrompt(bool player1){
        if(player1){
            prompts[0].SetActive(true);
        }
        else{
            prompts[1].SetActive(true);
        }
    }

    public void ClosePrompt(bool player1) { 
        if(player1){
            prompts[0].SetActive(false);
        }
        else{
            prompts[1].SetActive(false);
        }
    }

    public void EnableBoth(){
        OpenPrompt(true);
        OpenPrompt(false);
    }

    public void DisableBoth(){
        ClosePrompt(true);
        ClosePrompt(false);
    }

    public bool IsOpen(bool player1){
        if(player1){
            return prompts[0].activeInHierarchy;
        }
        else{
            return prompts[1].activeInHierarchy;
        }
    }
    
}

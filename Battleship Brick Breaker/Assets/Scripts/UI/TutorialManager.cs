using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public TutorialPrompt ballLaunch;
    public TutorialPrompt cannonLaunch;
    public TutorialPrompt bombPowerUp;
    public TutorialPrompt bombThrow;

    public TutorialPrompt moveTut;


    public TutorialPrompt[] tutorialPrompts;
    [SerializeField] GameObject brickHolder;
    [SerializeField] GameObject clearMenu;
    [SerializeField] ButtonPrompt p1CannonPrompt;
    [SerializeField] ButtonPrompt p2CannonPrompt;
    [SerializeField] ButtonPrompt p1CannonPromptLeft;
    [SerializeField] ButtonPrompt p2CannonPromptLeft;
    public BrickHealth[] bricks;

    public bool isTutorial;
    // Start is called before the first frame update

    
    void Start()
    {
        
        if (isTutorial)
        {
            //clearMenu.SetActive(false);
            bricks = brickHolder.GetComponentsInChildren<BrickHealth>();
            tutorialPrompts = GetComponentsInChildren<TutorialPrompt>();
            bombThrow.DisableBoth();
            bombPowerUp.DisableBoth();
            cannonLaunch.DisableBoth();
            moveTut.EnableBoth();
            ballLaunch.EnableBoth();
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableCannonPrompt(bool player1){
        if(player1){
            if(SettingsManager.p1Right){
                p1CannonPrompt.SwitchButtonPrompt(false);
                Debug.Log("Should prompt here");
            }
            else{
                p1CannonPromptLeft.SwitchButtonPrompt(false);
            }
            
            
        }
        else{
            if(SettingsManager.p2Right){
                
                p2CannonPrompt.SwitchButtonPrompt(false);
            }
            else{
                p2CannonPromptLeft.SwitchButtonPrompt(false);
            }
            
            
        }
    }
    public void DisableCannonPrompt(bool player1){
        if(player1){
            if(SettingsManager.p1Right){
                Debug.Log("Should prompt here");
                p1CannonPrompt.SwitchButtonPrompt(true);
            }
            else{
                p1CannonPromptLeft.SwitchButtonPrompt(true);
            }
            
            
        }
        else{
            if(SettingsManager.p2Right){
                p2CannonPrompt.SwitchButtonPrompt(true);
            }
            else{
                p2CannonPromptLeft.SwitchButtonPrompt(true);
            }
            
            
        }
    }
    public void DisableAllPrompts()
    {
        DisableCannonPrompt(true);
        DisableCannonPrompt(false);
        foreach (TutorialPrompt prompt in tutorialPrompts)
        {
            prompt.DisableBoth();
        }
    }


    public void CloseMovePrompt(bool player1)
    {
        moveTut.ClosePrompt(player1);
    }

    public bool RemainingBricks()
    {
        foreach (BrickHealth brick in bricks)
        {
            if (brick.isBroken)
            {
                continue;
            }
            else
            {
                return true;
            }
        }
        return false;
    }

    public void ClearTutorial()
    {
        PauseManager.instance.PauseGameplay();
        clearMenu.SetActive(true);
    }

    public void ToggleTutorial(){
        DisableAllPrompts();
    }
}

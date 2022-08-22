using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public TutorialPrompt ballLaunch;
    public TutorialPrompt cannonLaunch;
    public TutorialPrompt bombPowerUp;
    public TutorialPrompt bombThrow;
    public TutorialPrompt ammoBrick;
    public TutorialPrompt objBrick;
    public TutorialPrompt powerUpBrick;
    public TutorialPrompt moveTut;
    public TutorialPrompt powerUpCollect;

    public TutorialPrompt[] tutorialPrompts;
    [SerializeField] GameObject brickHolder;
    [SerializeField] GameObject clearMenu;
    public BrickHealth[] bricks;
    // Start is called before the first frame update
    void Start()
    {
        clearMenu.SetActive(false);
        bricks = brickHolder.GetComponentsInChildren<BrickHealth>();
        tutorialPrompts = GetComponentsInChildren<TutorialPrompt>();
        DisableAllPrompts();
        moveTut.EnableBoth();
        ballLaunch.EnableBoth();
        ammoBrick.EnableBoth();
        objBrick.EnableBoth();
        powerUpBrick.EnableBoth();
    }

    // Update is called once per frame
    void Update()
    {
        if(!RemainingBricks()){
            ClearTutorial();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            ClearTutorial();
        }
    }

    public void DisableAllPrompts(){
        foreach(TutorialPrompt prompt in tutorialPrompts){
            prompt.DisableBoth();
        }
    }


    public void CloseMovePrompt(bool player1){
        moveTut.ClosePrompt(player1);
    }

    public bool RemainingBricks(){
        foreach(BrickHealth brick in bricks){
            if(brick.isBroken){
                continue;
            }
            else{
                return true;
            }
        }
        return false;
    }

    public void ClearTutorial(){
        PauseManager.PauseGameplay();
        clearMenu.SetActive(true);
    }
}

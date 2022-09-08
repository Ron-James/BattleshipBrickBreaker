using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : Singleton<TutorialManager>
{
    public enum LaunchTutorial
    {
        None = 0,
        BombHold = 2,
        BombRelease = 4,
        Cannon = 8
    }

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
    public LaunchTutorial launchTutorialP1 = LaunchTutorial.None;
    public LaunchTutorial launchTutorialP2 = LaunchTutorial.None;

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
    public LaunchTutorial GetLaunchTutorialCurrentState(bool player1)
    {
        if (player1)
        {
            return launchTutorialP1;
        }
        else
        {
            return launchTutorialP2;
        }
    }

    public void SetGetLaunchTutorialCurrentState(LaunchTutorial tut, bool player1)
    {
        if (player1)
        {
            launchTutorialP1 = tut;
        }
        else
        {
            launchTutorialP2 = tut;
        }
    }
    public void EnableCannonPrompt(bool player1)
    {
        if (player1)
        {
            if (SettingsManager.p1Right)
            {
                if (p1CannonPrompt.gameObject.activeSelf)
                {
                    p1CannonPrompt.SwitchButtonPrompt(false);
                    Debug.Log("Should prompt here");
                }

            }
            else
            {
                if (p1CannonPromptLeft.gameObject.activeSelf)
                {
                    p1CannonPrompt.SwitchButtonPrompt(false);
                    Debug.Log("Should prompt here");
                }
            }


        }
        else
        {
            if (SettingsManager.p2Right)
            {
                if(p2CannonPrompt.gameObject.activeSelf){
                    p2CannonPrompt.SwitchButtonPrompt(false);
                }
                
            }
            else
            {
                if(p2CannonPromptLeft.gameObject.activeSelf){
                    p2CannonPromptLeft.SwitchButtonPrompt(false);
                }
                
            }


        }
    }
    public void DisableCannonPrompt(bool player1)
    {
        if (player1)
        {
            if (SettingsManager.p1Right)
            {
                Debug.Log("Should prompt here");
                p1CannonPrompt.SwitchButtonPrompt(true);
            }
            else
            {
                p1CannonPromptLeft.SwitchButtonPrompt(true);
            }


        }
        else
        {
            if (SettingsManager.p2Right)
            {
                p2CannonPrompt.SwitchButtonPrompt(true);
            }
            else
            {
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
    public void DisableAllPrompts(bool player1)
    {

        foreach (TutorialPrompt prompt in tutorialPrompts)
        {
            prompt.ClosePrompt(player1);
        }

    }
    public void DisableAllLaunchPrompts(bool player1)
    {
        cannonLaunch.ClosePrompt(player1);
        bombThrow.ClosePrompt(player1);
        bombPowerUp.ClosePrompt(player1);
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

    public void ToggleTutorial()
    {
        DisableAllPrompts();
    }

    public void SwitchLauchTut(LaunchTutorial tut, bool player1)
    {
        if (player1)
        {
            launchTutorialP1 = tut;
        }
        else
        {
            launchTutorialP2 = tut;
        }
        switch ((int)tut)
        {
            case 0://none
                DisableAllLaunchPrompts(player1);
                break;
            case 2://bomb hold
                DisableAllLaunchPrompts(player1);
                bombPowerUp.OpenPrompt(player1);



                break;
            case 4://bomb release
                DisableAllLaunchPrompts(player1);
                bombThrow.OpenPrompt(player1);


                break;
            case 8://cannon
                DisableAllLaunchPrompts(player1);
                cannonLaunch.OpenPrompt(player1);



                break;
        }
    }
}

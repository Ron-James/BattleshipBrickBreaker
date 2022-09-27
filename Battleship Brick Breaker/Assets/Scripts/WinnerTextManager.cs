using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinnerTextManager : MonoBehaviour
{

    [SerializeField] TextFlash textFlash;
    [SerializeField] Color p1Color;
    [SerializeField] Color p2Color;
    [SerializeField] Transform p1Transform;
    [SerializeField] Transform p2Transform;
    [SerializeField] GameObject winMenu;
 
    // Start is called before the first frame update
    void Start()
    {
        winMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetWinMenuActive(){
        winMenu.SetActive(true);
    }

    IEnumerator DelayWinMenuActive(float delay){
        yield return new WaitForSeconds(delay);
        Debug.Log(delay + " Delay");
        winMenu.SetActive(true);
        PauseManager.instance.PauseGameplay();
        
    }


    public void StartWinTextSequence(bool player1){
        if(player1){
            textFlash.transform.position = p1Transform.position;
            textFlash.transform.rotation = p1Transform.rotation;
            textFlash.FlashColor = p1Color;
            textFlash.FlashText();
            StartCoroutine(DelayWinMenuActive(textFlash.FlashDuration));
            
        }
        else{
            textFlash.transform.position = p2Transform.position;
            textFlash.transform.rotation = p2Transform.rotation;
            textFlash.FlashColor = p2Color;
            textFlash.FlashText();
            StartCoroutine(DelayWinMenuActive(textFlash.FlashDuration));
        }
        
    }


}

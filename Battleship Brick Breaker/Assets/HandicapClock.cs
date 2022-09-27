using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandicapClock : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] float timeRemaining = 0;
    [SerializeField] float lastStartTime = 0;
    [SerializeField] GameObject clockHolder;

    Coroutine clockRoutine;

    // Start is called before the first frame update
    void Start()
    {
        timeRemaining = 0;
        lastStartTime = 0;
        clockHolder.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartClockCount(float time){
        timeRemaining += time;
        lastStartTime = timeRemaining;
        if(timeRemaining > 0){
            if(clockRoutine == null){
                clockRoutine = StartCoroutine(StartClock());
            }
        }
    }

    public void StopClock(){
        timeRemaining = 0;
        lastStartTime = 0;
    }

    IEnumerator StartClock(){
        clockHolder.SetActive(true);
        while(true){
            timeRemaining -= Time.deltaTime;
            float ratio = timeRemaining/lastStartTime;
            if(timeRemaining <= 0){
                clockRoutine = null;
                clockHolder.SetActive(false);
                StopClock();
                break;
            }
            else{
                background.fillAmount = ratio;
                yield return null;
            }
        }
    }
}

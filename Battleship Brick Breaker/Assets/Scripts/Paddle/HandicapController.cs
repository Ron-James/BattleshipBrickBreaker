using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandicapController : MonoBehaviour
{
    public bool isHandicapped;
    [SerializeField] float handicapTime = 0;
    [SerializeField] float maxHandicapTime = 15f;
    Coroutine handicapRoutine;
    PaddleController paddleController;
    AimArrow aimArrow;
    HandicapClock handicapClock;

    public Coroutine HandicapRoutine { get => HandicapRoutine; set => HandicapRoutine = value; }


    // Start is called before the first frame update
    void Start()
    {
        aimArrow = GetComponentInChildren<AimArrow>();
        paddleController = GetComponent<PaddleController>();
        handicapClock = GetComponentInChildren<HandicapClock>();
        isHandicapped = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator HandicapPeriod(bool sink = true)
    {
        isHandicapped = true;
        if (sink)
        {
            GetComponentInChildren<BoatMotion>().SinkShip(handicapTime);
        }

        GetComponent<Artillery>().CanFire = false;
        aimArrow.IgnoreBalls(true);
        GetComponentInChildren<AimArrow>().CanLaunch = false;
        paddleController.IsStopped = true;

        while (true)
        {
            if (handicapTime <= 0)
            {
                isHandicapped = false;
                handicapTime = 0;
                handicapRoutine = null;
                aimArrow.IgnoreBalls(false);
                aimArrow.CanLaunch = true;
                GetComponent<Artillery>().CanFire = true;
                paddleController.IsStopped = false;
                paddleController.Slider.interactable = true;

                if(paddleController.Ball.IsBoundToPaddle){
                    aimArrow.StartOscillation();
                }


                break;
            }
            else
            {
                handicapTime -= Time.deltaTime;
                yield return null;
            }
        }
    }

    public void AddHandicapTime(float time, bool sink)
    {
        handicapTime += time;
        Mathf.Clamp(handicapTime, 0, maxHandicapTime);
        if (handicapTime > 0)
        {
            handicapClock.StartClockCount(handicapTime);
            if(handicapRoutine == null){
                handicapRoutine = StartCoroutine(HandicapPeriod(sink));
            }   
            

        }
    }

    public void OnBallOut(){
        AddHandicapTime(GameManager.instance.InitialOutPenalty, false);
    }


}

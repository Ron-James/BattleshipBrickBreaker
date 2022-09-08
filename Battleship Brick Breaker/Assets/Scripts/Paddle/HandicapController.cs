using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandicapController : MonoBehaviour
{
    public bool isHandicapped;
    [SerializeField] float handicapTime = 0;
    Coroutine handicapRoutine;
    PaddleController paddleController;
    AimArrow aimArrow;

    public Coroutine HandicapRoutine { get => HandicapRoutine; set => HandicapRoutine = value; }


    // Start is called before the first frame update
    void Start()
    {
        aimArrow = GetComponentInChildren<AimArrow>();
        paddleController = GetComponent<PaddleController>();
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
            GetComponentInChildren<BoatMotion>().SinkShip();
        }

        GetComponent<Artillery>().CanFire = false;
        aimArrow.IgnoreBalls(true);
        GetComponentInChildren<AimArrow>().CanLaunch = false;
        paddleController.IsStopped = true;
        paddleController.Slider.interactable = false;
        while (true)
        {
            if (handicapTime <= 0)
            {
                handicapTime = 0;
                isHandicapped = false;
                handicapRoutine = null;
                aimArrow.IgnoreBalls(false);
                aimArrow.CanLaunch = true;
                GetComponent<Artillery>().CanFire = true;
                paddleController.IsStopped = false;
                paddleController.Slider.interactable = true;



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
        if (handicapTime > 0 && handicapRoutine == null)
        {
            handicapRoutine = StartCoroutine(HandicapPeriod(sink));
        }
    }

    public void OnBallOut(){
        AddHandicapTime(GameManager.instance.InitialOutPenalty, false);
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HandicapController : MonoBehaviour
{
    public bool isHandicapped;
    [SerializeField] float handicapTime = 0;
    [SerializeField] Collider paddleCollider;
    Coroutine handicapRoutine;
    PaddleController paddleController;

    public Coroutine HandicapRoutine { get => HandicapRoutine; set => HandicapRoutine = value; }
   

    // Start is called before the first frame update
    void Start()
    {
        paddleController = GetComponent<PaddleController>();
        isHandicapped = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator HandicapPeriod(){
        isHandicapped = true;
        GetComponentInChildren<BoatMotion>().SinkShip();
        GetComponent<Artillery>().CanFire = false;
        paddleCollider.enabled = false;
        GetComponentInChildren<AimArrow>().CanLaunch = false;
        paddleController.IsStopped = true;
        paddleController.Slider.interactable = false;
        while(true){
            if(handicapTime <= 0){
                isHandicapped = false;
                handicapRoutine = null;
                if(GetComponentInChildren<AimArrow>().LaunchPenalty1 == null){
                    GetComponentInChildren<AimArrow>().CanLaunch = true;
                    GetComponent<Artillery>().CanFire = true;
                    paddleController.IsStopped = false;
                    paddleController.Slider.interactable = true;
                }
                
                
                break;
            }
            else{
                handicapTime -= Time.deltaTime;
                yield return null;
            }
        }
    }

    public void AddHandicapTime(float time){
        handicapTime += time;
        if(handicapTime > 0 && handicapRoutine == null){
            handicapRoutine = StartCoroutine(HandicapPeriod());
        }
    }

    
}

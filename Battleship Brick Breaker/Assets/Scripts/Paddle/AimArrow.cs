using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AimArrow : MonoBehaviour
{
    [SerializeField] bool player1;
    [SerializeField] Transform ballPos;
    [SerializeField] Transform aimPoint;
    
    [SerializeField] Collider paddleCollider;

    [Header("Aiming angles and speed")]
    [SerializeField] float maxAngle = 85f;
    [SerializeField] float aimPeriod = 0.8f;
    [SerializeField] float maxTapDistance = 10f;

    [SerializeField] GameObject ball;

    [Header("Out Penalty Time")]
    [SerializeField] float outPenaltyTime = 1f;
    [SerializeField] bool aiming;
    [SerializeField] bool canLaunch;
    [SerializeField] bool canHit;
    int sign;
    public Vector3 lastAimDirection;
    Coroutine oscillator;
    Coroutine launchPenalty;
    Touch lastTouchInField;
    Vector3 touchPoint;
    Image arrow;
    BallPhysics ballPhysics;
    HandicapController handicapController;
    PaddleController paddleController;

    public bool Aiming { get => aiming; set => aiming = value; }
    public bool CanLaunch { get => canLaunch; set => canLaunch = value; }
    public bool CanHit { get => canHit; set => canHit = value; }
    public Coroutine LaunchPenalty1 { get => launchPenalty; set => launchPenalty = value; }


    

    private void OnDisable() {
        StopAllCoroutines();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        handicapController = GetComponentInParent<HandicapController>();
        ballPhysics = ball.GetComponent<BallPhysics>();
        paddleController = GetComponentInParent<PaddleController>();
        outPenaltyTime = GameManager.instance.InitialOutPenalty;
        lastAimDirection = Vector3.zero;
        CanHit = false;
        arrow = GetComponent<Image>();
        if(player1){
            sign = -1;
        }
        else{
            sign = 1;
        }
        aiming = true;
        canLaunch = true;
        ResetRotation();
        transform.position = ballPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(canLaunch && aiming && oscillator == null){
            //oscillator = StartCoroutine(Oscillate(aimPeriod, CanHit));
        }

        if(ballPhysics.IsBoundToPaddle){
            if(!handicapController.isHandicapped){
                if(oscillator == null){
                    oscillator = StartCoroutine(Oscillate(aimPeriod, CanHit));
                }
            }
        }

        
    }

    public void ResetRotation(){
        float straight = 180;
        if(player1) {
            straight = 0;
        }
        else{
            straight = 180;
        }
        transform.eulerAngles = new Vector3(90, -90, straight);
    }
    public bool ClickInField()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;
            if (player1)
            {
                if (Mathf.Abs(hitPos.x) < Mathf.Abs(transform.position.x) && hitPos.x >= (ball.transform.position.x - (sign * maxTapDistance)))
                {
                    //testCube.transform.position = new Vector3(hitPos.x, 1, hitPos.z);
                    //Debug.Log(testCube.transform.position);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (Mathf.Abs(hitPos.x) < Mathf.Abs(transform.position.x) && hitPos.x <= (ball.transform.position.x - (sign * maxTapDistance)))
                {
                    //testCube.transform.position = new Vector3(hitPos.x, 1, hitPos.z);
                    //Debug.Log(testCube.transform.position);
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }
        else
        {
            return false;
        }
    }

    public void IgnoreBalls(bool ignore){
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls){
            Physics.IgnoreCollision(paddleCollider, ball.GetComponent<SphereCollider>(), ignore);
        }
        
    }
    IEnumerator Oscillate(float period, bool canHit){
        
        arrow.enabled = true;
        transform.position = GetComponentInParent<PaddleController>().Ball.transform.position;
        float time = 0;
        float w = (1/period) * 2 * Mathf.PI;
        float straight = 180;
        int touchIndex;
        Vector3 touchPos;
        GetComponentInParent<PaddleController>().Slider.interactable = false;
        GetComponentInParent<BombLauncher>().canLaunch = false;
        Vector3 direction = (aimPoint.position - GetComponent<RectTransform>().position).normalized;
        if(player1) {
            straight = 0;
        }
        else{
            straight = 180;
        }
        if(canHit){
            IgnoreBalls(true);
        }
        else{
            IgnoreBalls(false);
        }
        while(true){
            if(GetComponentInParent<HandicapController>().isHandicapped){
                oscillator = null;
                break;
            }
            if((ClickInField() && Input.GetMouseButtonDown(0)) || GameManager.instance.TouchInField(out touchIndex, out touchPos, player1)){
                if(paddleController.controlScheme == PaddleController.ControlScheme.slider){
                    paddleController.Slider.interactable = true;
                }
                if(TutorialManager.instance.isTutorial){
                    TutorialManager.instance.ballLaunch.ClosePrompt(player1);
                }
                IgnoreBalls(false);
                direction = (aimPoint.position - GetComponent<RectTransform>().position).normalized;
                
                StartCoroutine(BombLauncherDelay(0.2f));
                arrow.enabled = false;
                aiming = false;
                oscillator = null;
                CanHit = true;
                ballPhysics.Launch(GameManager.instance.InitialVelocity, sign * direction);
        
                yield return new WaitForFixedUpdate();
                break;
            }
            else{
                float zRot = straight - (maxAngle * Mathf.Sin(w * time));
                Vector3 angles = new Vector3(90, -90, zRot);
                angles.z = zRot;
                transform.eulerAngles = angles;
                direction = (aimPoint.position - GetComponent<RectTransform>().position).normalized;
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    IEnumerator BombLauncherDelay(float time){
        yield return new WaitForSeconds(time);
        GetComponentInParent<BombLauncher>().canLaunch = true;
        
    }

    public void OnBallOut(){
        aiming = true;
        canLaunch = false;
        canHit = false;
        
        

    }

}

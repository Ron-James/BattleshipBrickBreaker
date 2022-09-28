using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleButtonController : MonoBehaviour
{

    Coroutine move;
    Coroutine slowDown;

    [SerializeField] AnimationCurve speedUpCurve;
    [SerializeField] AnimationCurve slowDownCurve;

    [SerializeField] Vector3 maxPoint;
    [SerializeField] Vector3 minPoint;
    [SerializeField] float speedUpDuration = 1f;
    [SerializeField] float slowDownDuration = 1f;
    [SerializeField] float initialSpeed = 1f;
    [SerializeField] float maxSpeed = 1f;
    [SerializeField] float currentSpeed;
    [SerializeField] BoxCollider paddleCollider;
    [SerializeField] float speedModifier = 1f;

    public float snowSlowDownTime;
    Vector3 defPos;
    [SerializeField] float currentDirection = 1;
    [SerializeField] BoxCollider[] barriers = new BoxCollider[2];
    [SerializeField] GameObject bigIce;
    [SerializeField] GameObject regularIce;



    Rigidbody rb;

    [SerializeField] bool upButtonDown;
    [SerializeField] bool downButtonDown;



    PaddleController paddleController;
    PowerUpManager powerUpManager;
    Coroutine snowSlower;

    public float SpeedModifier { get => speedModifier; set => speedModifier = value; }

    public void UpdateButtonInput()
    {

    }
    public void SetUpButton(bool down)
    {
        upButtonDown = down;
    }
    public void SetDownButton(bool down)
    {
        downButtonDown = down;
    }
    // Start is called before the first frame update
    void Start()
    {
        powerUpManager = GetComponent<PowerUpManager>();
        rb = GetComponent<Rigidbody>();
        CalculateMaximumValues();
        paddleController = GetComponent<PaddleController>();
        upButtonDown = false;
        downButtonDown = false;
        slowDown = null;
        move = null;
        DisableSlowEffect();
        //rb = GetComponent<Rigidbody>();

    }




    // Update is called once per frame
    void Update()
    {



        if (!paddleController.IsStopped)
        {
            ApplyButtonInput();
        }


    }

    public void AddSnowSlowTime(float duration){
        snowSlowDownTime += duration;
        if(snowSlowDownTime > 0){
            if(snowSlower == null){
                snowSlower = StartCoroutine(SnowSlowDown());
            }
        }

    }

    public void EnableSlowEffect(){
        if(powerUpManager.IsLarge()){
            bigIce.SetActive(true);
            regularIce.SetActive(false);
        }
        else{
            bigIce.SetActive(false);
            regularIce.SetActive(true);
        }
    }

    public void DisableSlowEffect(){
        bigIce.SetActive(false);
        regularIce.SetActive(false);
    }

    IEnumerator SnowSlowDown(){
        speedModifier = GameManager.instance.SnowSlowDownMultiplier;
        EnableSlowEffect();
        while(true){
            snowSlowDownTime -= Time.deltaTime;
            if(snowSlowDownTime <= 0){
                speedModifier = 1;
                snowSlower = null;
                DisableSlowEffect();
                break;
            }
            else{
                yield return null;
            }
        }
    }

    public void ApplyButtonInput()
    {
        if (upButtonDown && !downButtonDown)
        {
            TutorialManager.instance.moveTut.SetAcknowledge(true, paddleController.Player1);
            currentDirection = 1;
            if (move == null)
            {
                if (currentSpeed == 0)
                {
                    move = StartCoroutine(MovePaddle(initialSpeed, maxSpeed, speedUpDuration));
                }
                else
                {
                    move = StartCoroutine(MovePaddle(currentSpeed, maxSpeed, speedUpDuration));
                }

            }
        }
        else if (!upButtonDown && downButtonDown)
        {
            TutorialManager.instance.moveTut.SetAcknowledge(true, paddleController.Player1);
            currentDirection = -1;
            if (move == null)
            {
                if (currentSpeed == 0)
                {
                    move = StartCoroutine(MovePaddle(initialSpeed, maxSpeed, speedUpDuration));
                }
                else
                {
                    move = StartCoroutine(MovePaddle(currentSpeed, maxSpeed, speedUpDuration));
                }
            }
        }
        else if (!downButtonDown && !upButtonDown)
        {
            if (slowDown == null)
            {
                slowDown = StartCoroutine(SlowDown(slowDownDuration));
            }
        }
        else if (downButtonDown && upButtonDown)
        {

            if (slowDown == null)
            {
                slowDown = StartCoroutine(SlowDown(slowDownDuration));
            }
        }
    }





    IEnumerator SlowDown(float duration)
    {
        float time = 0;
        float speed = currentSpeed;
        float iniSpeed = speed;
        currentSpeed = speed;
        Vector3 target = transform.position;
        float rate = speed / (duration / Time.deltaTime);
        while (true)
        {
            time += Time.fixedDeltaTime;
            float ratio = time / duration;
            speed = slowDownCurve.Evaluate(ratio) * iniSpeed;
            currentSpeed = speed;
            if (speed <= 0 || paddleController.IsStopped)
            {
                slowDown = null;
                currentSpeed = 0;
                break;
            }
            else if (upButtonDown || downButtonDown)
            {
                slowDown = null;
                //move = StartCoroutine(MovePaddle(currentSpeed, maxSpeed, speedUpDuration));
                break;
            }
            else
            {


                if (currentDirection == -1)
                {
                    target = minPoint;
                }
                else if (currentDirection == 1)
                {
                    target = maxPoint;
                }
                Vector3 position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
                rb.MovePosition(position);
                
                yield return new WaitForFixedUpdate();
            }
        }
    }
    IEnumerator MovePaddle(float iniSpeed, float maxSpeed, float duration)
    {

        float time = 0;
        float speed = iniSpeed;
        currentSpeed = speed;
        Vector3 target = transform.position;
        while (true)
        {
            time += Time.fixedDeltaTime;
            float ratio = time / duration;
            speed = speedUpCurve.Evaluate(ratio) * maxSpeed * speedModifier;
            currentSpeed = speed;
            if (!upButtonDown && !downButtonDown)
            {
                slowDown = StartCoroutine(SlowDown(slowDownDuration));
                move = null;
                break;
            }
            else if (paddleController.IsStopped)
            {
                currentSpeed = 0;
                move = null;
                break;
            }
            else
            {

                if (currentDirection == -1)
                {
                    target = minPoint;
                }
                else if (currentDirection == 1)
                {
                    target = maxPoint;
                }

                if (speed >= maxSpeed * speedModifier)
                {
                    speed = maxSpeed * speedModifier;
                    currentSpeed = speed;
                    if (currentDirection == -1)
                    {
                        target = minPoint;
                    }
                    else if (currentDirection == 1)
                    {
                        target = maxPoint;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
                }
                else
                {

                    Vector3 position = Vector3.MoveTowards(transform.position, target, speed * Time.fixedDeltaTime);
                    rb.MovePosition(position);
                    currentSpeed = speed;
                }
                //yield return null;
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void CalculateMaximumValues()
    {
        Transform topPiece = barriers[0].transform;
        Transform bottomPiece = barriers[1].transform;

        float barrierWidth = barriers[0].bounds.size.z / 2;
        float paddleWidth = paddleCollider.bounds.size.z / 2;


        maxPoint = topPiece.position;
        maxPoint.z -= barrierWidth;
        maxPoint.z -= paddleWidth;
        maxPoint.x = transform.position.x;

        minPoint = bottomPiece.position;
        minPoint.z += barrierWidth;
        minPoint.z += paddleWidth;
        minPoint.x = transform.position.x;
    }
}

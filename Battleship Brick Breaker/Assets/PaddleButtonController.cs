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
    Vector3 defPos;
    [SerializeField] float currentDirection = 1;
    [SerializeField] BoxCollider[] barriers = new BoxCollider[2];


    Rigidbody rb;

    [SerializeField] bool upButtonDown;
    [SerializeField] bool downButtonDown;


    PaddleController paddleController;
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
        CalculateMaximumValues();
        paddleController = GetComponent<PaddleController>();
        upButtonDown = false;
        downButtonDown = false;
        slowDown = null;
        move = null;
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





    public void ApplyButtonInput()
    {
        if (upButtonDown && !downButtonDown)
        {
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
            time += Time.deltaTime;
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
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                
                yield return null;
            }
        }
    }
    IEnumerator MovePaddle(float iniSpeed, float maxSpeed, float duration)
    {

        float time = 0;
        float speed = iniSpeed;
        currentSpeed = speed;
        Vector3 target = transform.position;
        float rate = (maxSpeed - iniSpeed) / (duration / Time.deltaTime);
        while (true)
        {
            time += Time.deltaTime;
            float ratio = time / duration;
            speed = speedUpCurve.Evaluate(ratio) * maxSpeed;
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

                if (speed >= maxSpeed)
                {
                    speed = maxSpeed;
                    currentSpeed = speed;
                    if (currentDirection == -1)
                    {
                        target = minPoint;
                    }
                    else if (currentDirection == 1)
                    {
                        target = maxPoint;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                }
                else
                {

                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    currentSpeed = speed;
                }

                yield return null;
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

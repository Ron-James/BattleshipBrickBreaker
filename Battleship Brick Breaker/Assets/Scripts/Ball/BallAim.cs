using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAim : MonoBehaviour
{
    [SerializeField] float lineLength = 10f;
    [SerializeField] float maxAimDistance = 15f;
    [SerializeField] float initialPower = 10f;

 
    [SerializeField] Vector3 touchPoint;
    [SerializeField] Transform aimPoint;
    [SerializeField] bool aiming = false;
    [SerializeField] Vector3 direction;
    [SerializeField] GameObject ball;
    [SerializeField] float outPenaltyTime = 1f;
    [SerializeField] float outPenaltyIncrease = 0.25f;
    RaycastHit inputPoint;
    bool clicked = false;
    Touch lastTouchInField;
    LineRenderer aimLine;
    int sign;
    [SerializeField] bool right = true;
    int touchIndex;

    bool canFire = true;
    Vector3 touchPos;



    public Transform AimPoint { get => aimPoint; set => aimPoint = value; }
    public GameObject Ball { get => ball; set => ball = value; }
    public bool Right { get => right; set => right = value; }
    public bool Aiming { get => aiming; set => aiming = value; }
    public bool CanFire { get => canFire; set => canFire = value; }

    // Start is called before the first frame update
    void Start()
    {
        CanFire = true;
        aiming = true;
        aimLine = GetComponent<LineRenderer>();
        if (ball.GetComponent<BallPhysics>().RightSide)
        {
            Right = true;
            sign = 1;
        }
        else
        {
            Right = false;
            sign = -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (aiming && !clicked && CanFire)
        {

            direction = (GetComponent<Artillery>().FirePoint.position - transform.position).normalized;
            DrawAimLine();
        }
        else{
            ResetLine();
        }
        if (TouchInField(out touchIndex, out touchPos) && aiming && CanFire)
        {
            //Debug.Log("hit something");
            direction = (touchPos - transform.position).normalized;
            DrawAimLine();
            clicked = true;
        }
        if (clicked && lastTouchInField.phase == TouchPhase.Ended && CanFire)
        {
            if (ball.GetComponent<BallPhysics>().RightSide)
            {
                ball.GetComponent<BallPhysics>().Fire(initialPower, -direction);
            }
            else
            {
                ball.GetComponent<BallPhysics>().Fire(initialPower, direction);
            }


            clicked = false;
            ResetLine();
            aiming = false;

        }
        */
    }
    public bool ClickInField()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;
            if (Right)
            {
                if (Mathf.Abs(hitPos.x) < Mathf.Abs(transform.position.x) && hitPos.x >= (ball.transform.position.x - (sign * maxAimDistance)))
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
                if (Mathf.Abs(hitPos.x) < Mathf.Abs(transform.position.x) && hitPos.x <= (ball.transform.position.x - (sign * maxAimDistance)))
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
    public Vector3 ClickPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 hitPos = hit.point;
            if (Mathf.Abs(hitPos.x) < Mathf.Abs(transform.position.x))
            {
                return new Vector3(hitPos.x, 1, hitPos.z);
            }
            else
            {
                return Vector3.zero;
            }
        }
        else
        {
            return Vector3.zero;
        }
    }
    public void DrawAimLine()
    {
        aimLine.enabled = true;
        Vector3 ballPos = transform.InverseTransformPoint(ball.transform.position);
        aimLine.SetPosition(0, ballPos);
        Vector3 lineDir = transform.position + (direction * lineLength);
        aimLine.SetPosition(1, transform.InverseTransformPoint(lineDir));

    }
    public void ResetLine()
    {
        aimLine.enabled = false;
        aimLine.SetPosition(0, Vector3.zero);
        aimLine.SetPosition(1, Vector3.zero);
    }

    public void StartLaunchPenalty(){
        StartCoroutine(LaunchPenalty(outPenaltyTime));
        outPenaltyTime += outPenaltyIncrease;
    }
    IEnumerator LaunchPenalty(float period){
        CanFire = false;
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= period){
                CanFire = true;
                GetComponent<Artillery>().CanFire = true;
                break;
            }
            else{
                yield return null;
            }
        }
    }
    public bool TouchInField(out int touchIndex, out Vector3 position)
    {
        touchIndex = -1;
        position = Vector3.zero;
        if (Input.touches.Length == 0)
        {
            position = Vector3.zero;
            touchIndex = -1;
            return false;
        }
        else
        {
            for (int loop = 0; loop < Input.touchCount; loop++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[loop].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (Right)
                    {
                        if (Mathf.Abs(hit.point.x) < Mathf.Abs(ball.transform.position.x) && hit.point.x >= (ball.transform.position.x - (sign * maxAimDistance)))
                        {
                            touchIndex = loop;
                            touchPoint = hit.point;
                            lastTouchInField = Input.touches[loop];
                            position = hit.point;
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else{
                        if (Mathf.Abs(hit.point.x) < Mathf.Abs(ball.transform.position.x) && hit.point.x <= (ball.transform.position.x - (sign * maxAimDistance)))
                        {
                            touchIndex = loop;
                            touchPoint = hit.point;
                            lastTouchInField = Input.touches[loop];
                            position = hit.point;
                            return true;
                        }
                        else
                        {
                            continue;
                        }
                    }

                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }

}

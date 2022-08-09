using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AimArrow : MonoBehaviour
{
    [SerializeField] bool rightSided;
    [SerializeField] Transform ballPos;
    [SerializeField] Transform aimPoint;
    [SerializeField] GameObject backboard;

    [Header("Aiming angles and speed")]
    [SerializeField] float maxAngle = 85f;
    [SerializeField] float aimPeriod = 0.8f;
    [SerializeField] float maxTapDistance = 10f;
    [SerializeField] float initialPower = 10f;
    [SerializeField] GameObject ball;

    [Header("Out Penalty Time")]
    [SerializeField] float outPenaltyTime = 1f;
    [SerializeField] float outPenaltyIncrease = 0.25f;
    bool aiming;
    bool canFire;
    int sign;
    Coroutine oscillator;
    Touch lastTouchInField;
    Vector3 touchPoint;
    Image arrow;

    public bool Aiming { get => aiming; set => aiming = value; }
    public bool CanFire { get => canFire; set => canFire = value; }

    // Start is called before the first frame update
    void Start()
    {

        arrow = GetComponent<Image>();
        maxTapDistance = backboard.transform.localScale.z/2;
        if(rightSided){
            sign = 1;
        }
        else{
            sign = -1;
        }
        aiming = true;
        canFire = true;
        ResetRotation();
        transform.position = ballPos.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(canFire && aiming && oscillator == null){
            oscillator = StartCoroutine(Oscillate(aimPeriod));
        }

        
    }

    public void ResetRotation(){
        float straight = 180;
        if(rightSided) {
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
            if (rightSided)
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
                    if (rightSided)
                    {
                        if (Mathf.Abs(hit.point.x) < Mathf.Abs(ball.transform.position.x) && hit.point.x >= (ball.transform.position.x - (sign * maxTapDistance)))
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
                        if (Mathf.Abs(hit.point.x) < Mathf.Abs(ball.transform.position.x) && hit.point.x <= (ball.transform.position.x - (sign * maxTapDistance)))
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

    public void StartLaunchPenalty(){
        StartCoroutine(LaunchPenalty(outPenaltyTime));
        outPenaltyTime += outPenaltyIncrease;
    }

    IEnumerator LaunchPenalty(float period){
        CanFire = false;
        float time = 0;
        GetComponentInParent<PaddleController>().Slider.interactable = false;
        while(true){
            time += Time.deltaTime;
            if(time >= period){
                CanFire = true;
                GetComponentInParent<Artillery>().CanFire = true;
                GetComponentInParent<PaddleController>().Slider.interactable = true;
                break;
            }
            else{
                yield return null;
            }
        }
    }
    IEnumerator Oscillate(float period){
        arrow.enabled = true;
        float time = 0;
        float w = (1/period) * 2 * Mathf.PI;
        float straight = 180;
        int touchIndex;
        Vector3 touchPos;
        GetComponentInParent<PaddleController>().Slider.interactable = false;
        Vector3 direction = (aimPoint.position - GetComponent<RectTransform>().position).normalized;
        if(rightSided) {
            straight = 0;
        }
        else{
            straight = 180;
        }
        while(true){
            if((ClickInField() && Input.GetMouseButtonDown(0)) || (TouchInField(out touchIndex, out touchPos))){
                direction = (aimPoint.position - GetComponent<RectTransform>().position).normalized;
                ball.GetComponent<BallPhysics>().Fire(initialPower, sign * -direction);
                arrow.enabled = false;
                aiming = false;
                oscillator = null;
                GetComponentInParent<PaddleController>().Slider.interactable = true;;
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
}

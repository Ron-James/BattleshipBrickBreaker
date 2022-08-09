using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleController : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] float maxZ = 21.3f;
    [SerializeField] float speed;
    [SerializeField] Transform ballPosition;
    [SerializeField] bool isStopped = false;
    [SerializeField] GameObject sliderHandle;
    [SerializeField] bool hit;
    [SerializeField] bool player1;
    [SerializeField] bool aiPlayer = false;
    [SerializeField] float outPenaltyTime = 1f;
    [SerializeField] float outPenaltyIncrease = 0.25f;
    [SerializeField] float hitPenalty = 5f;
    [SerializeField] Transform backboard;
    [SerializeField] BallPhysics ball;

    AimArrow ballAim;

    [Header("Materials")]
    [SerializeField] Material mainMaterial;
    [SerializeField] Material hitMaterial;

    float currentSliderValue;
    float paddleWidth;
    float backboardOffset = 0;

    Vector3 defaultPos;

    public bool Hit { get => hit; set => hit = value; }
    public Transform Backboard { get => backboard; set => backboard = value; }
    public bool IsStopped { get => isStopped; set => isStopped = value; }
    public Slider Slider { get => slider; set => slider = value; }
    public bool Player1 { get => player1; set => player1 = value; }



    // Start is called before the first frame update
    void Start()
    {
        ballAim = GetComponentInChildren<AimArrow>();
        defaultPos = transform.position;
        paddleWidth = transform.localScale.z;
        currentSliderValue = slider.value;
        backboardOffset = backboard.position.z;
        Hit = false;
        GetComponentInChildren<MeshRenderer>().material = mainMaterial;


    }

    // Update is called once per frame
    void Update()
    {
        if (!IsStopped && !aiPlayer)
        {
            Vector3 position = transform.position;
            position.z = (slider.value * maxZ) + backboardOffset;
            transform.position = Vector3.MoveTowards(transform.position, position, speed);
            currentSliderValue = slider.value;
        }

    }

    public void OutPenalize()
    {
        StartCoroutine(OutPenalty(outPenaltyTime));
        outPenaltyTime += outPenaltyIncrease;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void StartHitPenalty()
    {
        StartCoroutine(HitPenalty(hitPenalty));
    }
    IEnumerator HitPenalty(float time)
    {
        float t = 0;
        GetComponent<Artillery>().CanFire = false;
        Collider collider = GetComponentInChildren<Collider>();
        collider.enabled = false;
        GetComponentInChildren<MeshRenderer>().material = hitMaterial;
        if (GetComponentInChildren<AimArrow>().Aiming && GetComponentInChildren<AimArrow>().CanFire)
        {
            GetComponentInChildren<AimArrow>().CanFire = false;
        }
        IsStopped = true;
        Hit = true;
        slider.interactable = false;
        while (true)
        {
            t += Time.deltaTime;
            if (t >= time)
            {
                if (ball.IsOut)
                {
                    GoneOut();
                }
                else
                {
                    GetComponent<BallAim>().CanFire = true;
                    GetComponent<Artillery>().CanFire = true;
                }
                GetComponentInChildren<MeshRenderer>().material = mainMaterial;
                collider.enabled = true;
                IsStopped = false;
                Hit = false;
                slider.interactable = true;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }

    public void GoneOut()
    {
        transform.position = defaultPos;
        slider.value = 0;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ball.BindToPaddle();
        ballAim.CanFire = false;
        ballAim.Aiming = true;
        GetComponent<Artillery>().CanFire = false;

        ballAim.StartLaunchPenalty();

        //StartCoroutine(AimWait(outPenaltyTime));
        //outPenaltyTime += outPenaltyIncrease;
    }


    IEnumerator OutPenalty(float time)
    {
        float t = 0;

        IsStopped = true;
        transform.position = defaultPos;
        slider.value = 0;
        slider.interactable = false;
        ball.BindToPaddle();
        while (true)
        {
            t += Time.deltaTime;
            if (t >= time)
            {
                IsStopped = false;
                ballAim.Aiming = true;
                slider.interactable = true;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }


}

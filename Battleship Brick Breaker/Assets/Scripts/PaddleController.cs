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
    [SerializeField] bool player1;
    [SerializeField] bool aiPlayer = false;
    [SerializeField] float outPenaltyTime = 1f;
    [SerializeField] float outPenaltyIncrease = 0.25f;
    [SerializeField] Transform backboard;
    [SerializeField] BallPhysics ball;
    public BallEvents ballEvents;
    [SerializeField] Collider paddleCollider;
    [SerializeField] bool isHandicapped;
    [SerializeField] float handicapTimeRemaining = 0;

    AimArrow ballAim;
    Artillery artillery;
    BombLauncher bombLauncher;
    PowerUpManager powerUpManager;

    float currentSliderValue;
    float paddleWidth;
    float backboardOffset = 0;

    Vector3 defaultPos;


    public Transform Backboard { get => backboard; set => backboard = value; }
    public bool IsStopped { get => isStopped; set => isStopped = value; }
    public Slider Slider { get => slider; set => slider = value; }
    public bool Player1 { get => player1; set => player1 = value; }
    public BallPhysics Ball { get => ball; set => ball = value; }
    public Transform BallPosition { get => ballPosition; set => ballPosition = value; }
    public Collider PaddleCollider { get => paddleCollider; set => paddleCollider = value; }
    public bool IsHandicapped { get => isHandicapped; set => isHandicapped = value; }


    private void Awake()
    {
        BallEvents ballEvents = ball.GetComponent<BallEvents>();
        ballAim = GetComponentInChildren<AimArrow>();
        powerUpManager = GetComponent<PowerUpManager>();
        artillery = GetComponent<Artillery>();
        bombLauncher = GetComponent<BombLauncher>();

        ballEvents.OnBallOut.AddListener(delegate { OnBallOut(); });
        ballEvents.OnBallOut.AddListener(delegate { ballAim.OnBallOut(); });
        ballEvents.OnBallOut.AddListener(delegate { bombLauncher.OnBallOut(); });
        ballEvents.OnBallOut.AddListener(delegate { artillery.OnBallOut(); });
        ballEvents.OnBallOut.AddListener(delegate { powerUpManager.OnBallOut(); });
    }
    private void OnDisable()
    {
        //ball.gameObject.GetComponent<BallEvents>().OnBallOut.RemoveListener(OnBallOut);
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {

        handicapTimeRemaining = 0;

        defaultPos = transform.position;
        paddleWidth = transform.localScale.z;
        currentSliderValue = slider.value;
        backboardOffset = backboard.position.z;
        //GetComponentInChildren<MeshRenderer>().material = mainMaterial;


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

    public void OnBallOut()
    {
        transform.position = defaultPos;
        slider.value = 0;
        ballAim.CanHit = false;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballAim.CanLaunch = false;
        ball.GetComponent<CollisionVelocityControl>().LargestMagnitude = 0;
        GetComponent<PowerUpManager>().ResetPowerUp();
    }


}

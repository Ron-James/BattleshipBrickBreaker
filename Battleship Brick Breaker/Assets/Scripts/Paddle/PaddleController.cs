using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleController : MonoBehaviour
{

    public enum ControlScheme{
        slider = 0,
        button = 1
    }
    [SerializeField] Slider slider;
    [SerializeField] Transform ballPosition;
    [SerializeField] bool isStopped = false;
    [SerializeField] bool player1;
    [SerializeField] Transform backboard;
    [SerializeField] BallPhysics ball;

    public ControlScheme controlScheme = ControlScheme.slider;


    AimArrow ballAim;
    Artillery artillery;
    BombLauncher bombLauncher;
    PowerUpManager powerUpManager;
    HandicapController handicapController;

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


    private void Awake()
    {
        BallEvents ballEvents = ball.GetComponent<BallEvents>();
        ballAim = GetComponentInChildren<AimArrow>();
        powerUpManager = GetComponent<PowerUpManager>();
        artillery = GetComponent<Artillery>();
        bombLauncher = GetComponent<BombLauncher>();
        handicapController = GetComponent<HandicapController>();

        ballEvents.OnBallOut.AddListener(delegate { handicapController.OnBallOut(); });
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



        defaultPos = transform.position;
        paddleWidth = transform.localScale.z;
        currentSliderValue = slider.value;
        backboardOffset = backboard.position.z;
        //GetComponentInChildren<MeshRenderer>().material = mainMaterial;


    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (!IsStopped && !aiPlayer)
        {
            Vector3 position = transform.position;
            position.z = (slider.value * maxZ) + backboardOffset;
            transform.position = Vector3.MoveTowards(transform.position, position, speed);
            currentSliderValue = slider.value;
        }
        */

    }

    public void OnBallOut()
    {
        transform.position = defaultPos;
        slider.value = 0;
        ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
        ballAim.CanLaunch = false;
        ball.GetComponent<CollisionVelocityControl>().LargestMagnitude = 0;
        GetComponent<PowerUpManager>().ResetPowerUp();
    }


}

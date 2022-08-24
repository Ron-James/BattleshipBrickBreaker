using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float radius = 0.5f;
    [SerializeField] GameObject paddle;
    [SerializeField] float bounciness = 1.005f;
    [SerializeField] float aimLineLength = 3f;
    [SerializeField] bool rightSide = true;

    [Header("Velocity Mods")]
    [SerializeField] Vector3 lastVelocity;
    [SerializeField] Vector3 oldVelocity;
    [SerializeField] float terminalVelocity;
    [SerializeField] bool isOut = false;
    [SerializeField] bool currentPlayer1 = true;


    [Header("Power Up Things")]
    public bool catcher = false;
    public bool onFire = false;
    public float fireDamage = 2;

    [Header("Sounds")]
    [SerializeField] Sound generalHit;


    Rigidbody rb;
    int inwardSign;





    public float Radius { get => radius; set => radius = value; }
    public float Bounciness { get => bounciness; set => bounciness = value; }
    public bool RightSide { get => rightSide; set => rightSide = value; }
    public bool IsOut { get => isOut; set => isOut = value; }
    public bool CurrentPlayer1 { get => currentPlayer1; set => currentPlayer1 = value; }
    public Vector3 LastVelocity { get => lastVelocity; set => lastVelocity = value; }




    void Start()
    {
        IgnoreBallCollisions();
        generalHit.src = GetComponent<AudioSource>();
        Radius = transform.localScale.x / 2;
        IsOut = false;
        rb = GetComponent<Rigidbody>();

        if (rightSide)
        {
            inwardSign = -1;
        }
        else
        {
            inwardSign = 1;
        }
        BindToPaddle();

    }
    private void LateUpdate() {
        if (!rb.isKinematic)
        {
            oldVelocity = lastVelocity;
            lastVelocity = rb.velocity;
        }
    }
    private void FixedUpdate()
    {
        

        if (rb.velocity.magnitude > GameManager.instance.MaxBallVelocity)
        {
            Debug.Log("Clamp Velocity");
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, GameManager.instance.MaxBallVelocity);
        }
        if (transform.parent != null && rb.velocity.magnitude > 0)
        {
            BindToPaddle();
        }
    }
    // Update is called once per frame
    void Update()
    {
        /*
        if (lastVelocity.magnitude < oldVelocity.magnitude)
        {
            if (transform.parent == null && !rb.velocity.Equals(Vector3.zero) && oldVelocity.magnitude > 0)
            {
                Debug.Log("maintain velocity");
                Vector3 newVelocity = lastVelocity.normalized * oldVelocity.magnitude;
                //GameManager.instance.ApplyForceToVelocity(rb, newVelocity, 1000);
            }
        }

        */

        if (transform.parent == null && rb.velocity.magnitude == 0)
        {
            //Debug.Log("what is happening");
            rb.velocity = lastVelocity;
            /*
            
            if(lastVelocity.magnitude == 0){
                if(oldVelocity.magnitude == 0){
                    rb.velocity = paddle.GetComponentInChildren<AimArrow>().lastAimDirection * GameManager.instance.InitialVelocity; 

                }
                else{
                    rb.velocity = oldVelocity.normalized * GameManager.instance.InitialVelocity;
                }
            }
            else{
                rb.velocity = lastVelocity.normalized * GameManager.instance.InitialVelocity;
            }
            */
        }

    }
    public void SetVelocityMagnitude(float magnitude)
    {
        Vector3 direction = rb.velocity.normalized;
        Vector3 newVelocity = direction * magnitude;
        rb.velocity = newVelocity;
    }
    public void IncreaseSize(float percent)
    {
        float newRadius = (Radius * percent) * 2;
        transform.localScale = newRadius * Vector3.one;
    }
    public void ResetSize()
    {
        float newRadius = (Radius) * 2;
        transform.localScale = newRadius * Vector3.one;
    }

    public void IgnoreBallCollisions(){
        GameObject [] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls){
            Physics.IgnoreCollision(ball.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
        }

    }
    private void OnCollisionEnter(Collision other)
    {


        switch (other.collider.tag)
        {

            default:
                generalHit.PlayOnce();
                //Debug.Log("hit reflected");
                Reflect(other, bounciness);
                break;
            case "Paddle":
                generalHit.PlayOnce();
                Debug.Log("cacher " + other.collider.GetComponentInParent<PowerUpManager>().catcher);
                if (other.collider.GetComponentInParent<PowerUpManager>().catcher && GetComponent<PlayerTracker>().GetMaintOwner() != 0)
                {

                    if (!other.collider.GetComponentInChildren<AimArrow>().Aiming)
                    {
                        if(other.GetContact(0).normal == (Vector3.right * inwardSign)){
                            PaddleCatch();
                            Debug.Log("Catch");
                        }
                        
                    }

                }
                IncreaseVelocity(bounciness);
                break;
            case "Ball":
                //Debug.Log("hit ball");
                //Physics.IgnoreCollision(other.collider, GetComponent<Collider>());
                break;
            case "Brick":
                Reflect(other, bounciness);
                if (onFire)
                {
                    other.collider.GetComponent<BrickHealth>().TakeDamge(fireDamage, (int)GetComponent<PlayerTracker>().CurrentOwner1);
                }
                else
                {
                    other.collider.GetComponent<BrickHealth>().TakeDamge(1, (int)GetComponent<PlayerTracker>().CurrentOwner1);
                }
                break;

        }

    }
    private void Reflect(Collision collision, float percent)
    {
        Vector3 reflected = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);
        //Debug.Log(Vector3.SignedAngle(lastVelocity, reflected, Vector3.up));
        /*
        if (reflected.x == (-1 * lastVelocity.x) || reflected.z == (-1 * lastVelocity.z))
        {
            Debug.Log(Vector3.SignedAngle(lastVelocity, reflected, Vector3.up) + " angle");
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                Vector3 newReflected = Quaternion.AngleAxis(5, Vector3.up) * reflected;
                rb.velocity = newReflected;
                //Debug.Log(Vector3.SignedAngle(lastVelocity, newReflected, Vector3.up) + " new angle");
            }
            else
            {
                Vector3 newReflected = Quaternion.AngleAxis(-5, Vector3.up) * reflected;
                rb.velocity = newReflected;
                //Debug.Log(Vector3.SignedAngle(lastVelocity, newReflected, Vector3.up) + " new angle");
            }
        }
        */
        rb.velocity = reflected;
        IncreaseVelocity(percent);
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    public void IncreaseVelocity(float percent)
    {
        Vector3 direction = rb.velocity.normalized;
        float newMag = rb.velocity.magnitude * percent;
        //newMag = Mathf.Clamp(newMag, -GameManager.instance.MaxBallVelocity, GameManager.instance.MaxBallVelocity);
        //rb.velocity = newMag * direction;
        GameManager.instance.ApplyForceToVelocity(rb, newMag * direction, 1000);

    }
    public Vector3 RotateVector(Vector3 input, float angle)
    {
        Vector3 newVector = Quaternion.AngleAxis(angle, Vector3.up) * input;
        return newVector;
    }
    public void BindToPaddle()
    {
        rb.velocity = Vector3.zero;
        transform.SetParent(paddle.transform);

        transform.position = paddle.GetComponent<PaddleController>().BallPosition.position;


    }

    public void PaddleCatch()
    {
        paddle.GetComponentInChildren<AimArrow>().CanHit = true;
        StartCoroutine(SetKinematic(1));
        transform.SetParent(paddle.transform);
        paddle.GetComponentInChildren<AimArrow>().Aiming = true;
    }

    IEnumerator SetKinematic(int frames){
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        for (int loop = 0; loop < frames; loop++){
            yield return null;
        }
        rb.isKinematic = false;
    }
    
    public void Launch(float power, Vector3 direction)
    {
        isOut = false;
        if (rb.isKinematic)
        {
            rb.isKinematic = false;
        }
        transform.parent = null;
        //rb.velocity = inwardSign * (direction * power);
        GameManager.instance.ApplyForceToVelocity(rb, inwardSign * power * direction, 1000);

        GetComponent<Collider>().enabled = true;
        Debug.Log(power + " Power " + rb.velocity.magnitude + " Velocity ");
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "OutZone":
                //IsOut = true;
                //GameManager.instance.DisablePowerUps(rightSide);
                

                //paddle.GetComponent<PowerUpManager>().ResetPowerUp();

                if (rightSide)
                {
                    //GameManager.instance.Missed[0]++;
                }
                else
                {
                    //GameManager.instance.Missed[1]++;
                }
                break;
        }
    }
    public void OnBallOut(){
        BindToPaddle();
    }
    public void SwitchPlayer()
    {
        currentPlayer1 = !currentPlayer1;
        // change material
    }

    public void StartRandomReturn(float velocity, Transform point)
    {
        StartCoroutine(RandomReturn(3, velocity, point));
    }

    IEnumerator RandomReturn(int framesStop, float velocity, Transform point)
    {
        rb.velocity = Vector3.zero;
        //rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        for (int s = 0; s < framesStop; s++)
        {
            yield return null;
        }
        transform.position = point.position;
        rb.isKinematic = false;
        Vector3 direction = inwardSign * Vector3.left;
        direction = Quaternion.AngleAxis(Random.Range(-15, 16), Vector3.up) * direction;
        direction = direction.normalized;
        rb.velocity = Vector3.zero;

        Debug.Log("velocity is " + velocity);
        //direction = RotateVector(direction, Random.Range(-5, 5));
        //rb.AddForce(direction * velocity, ForceMode.VelocityChange);
        rb.velocity = direction * velocity;

    }

}

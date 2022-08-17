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
        Radius = transform.localScale.x / 2;
    }
    private void FixedUpdate()
    {
        if (!rb.isKinematic)
        {
            oldVelocity = lastVelocity;
            lastVelocity = rb.velocity;
        }

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

        if (lastVelocity.magnitude < oldVelocity.magnitude)
        {
            if (transform.parent == null && !rb.velocity.Equals(Vector3.zero) && oldVelocity.magnitude > 0)
            {
                Debug.Log("maintain velocity");
                //SetVelocityMagnitude(oldVelocity.magnitude);
            }
        }

        if (transform.parent == null && rb.velocity.magnitude == 0)
        {
            //Debug.Log("what is happening");
            rb.velocity = paddle.GetComponentInChildren<AimArrow>().lastAimDirection * GameManager.instance.InitialVelocity;
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

    private void OnCollisionEnter(Collision other)
    {


        switch (other.collider.tag)
        {

            default:
                //Debug.Log("hit reflected");
                Reflect(other, bounciness);
                break;
            case "Paddle":
                Debug.Log("cacher " + other.collider.GetComponentInParent<PowerUpManager>().catcher);
                if (other.collider.GetComponentInParent<PowerUpManager>().catcher && GetComponent<PlayerTracker>().GetMaintOwner() != 0)
                {

                    if (!other.collider.GetComponentInChildren<AimArrow>().Aiming)
                    {
                        PaddleCatch();
                        Debug.Log("Catch");
                    }

                }

                break;
            case "Ball":
                //Debug.Log("hit ball");
                Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
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
        IsOut = false;
        rb.velocity = Vector3.zero;
        transform.SetParent(paddle.transform);

        transform.position = paddle.GetComponent<PaddleController>().BallPosition.position;


    }

    public void PaddleCatch()
    {
        paddle.GetComponentInChildren<AimArrow>().CanHit = true;
        rb.isKinematic = true;
        transform.SetParent(paddle.transform);
        paddle.GetComponentInChildren<AimArrow>().Aiming = true;
    }

    public void RandomReturn()
    {
        Vector3 position = Vector3.zero;
        position.x = paddle.transform.position.x;
        int sign = 1;
        if (paddle.GetComponent<PaddleController>().Player1)
        {
            sign = -1;
        }
        else
        {
            sign = 1;
        }
        position.x += (sign * GameManager.instance.RandomReturnDistance);
        transform.position = position;

        Vector3 direction = Vector3.zero;
        direction.z = Random.Range(-1, 2);
        direction.x = -sign;
        rb.velocity = Vector3.zero;
        Fire(GameManager.instance.InitialVelocity * 2, direction.normalized);
    }
    public void Fire(float power, Vector3 direction)
    {
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
                IsOut = true;
                GameManager.instance.DisablePowerUps(rightSide);
                if (!paddle.GetComponent<PaddleController>().Hit)
                {
                    //paddle.GetComponent<PaddleController>().GoneOut();
                }

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
        transform.position = point.position;
        //rb.velocity = Vector3.zero;
        for (int s = 0; s < framesStop; s++)
        {
            rb.isKinematic = true;
            yield return null;
        }

        rb.isKinematic = false;
        Vector3 direction = point.forward;
        direction = RotateVector(direction, Random.Range(-15, 15));
        Fire(velocity, direction);

    }

}

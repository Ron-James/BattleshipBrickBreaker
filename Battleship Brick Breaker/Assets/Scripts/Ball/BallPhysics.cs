using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPhysics : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float radius = 0.5f;
    [SerializeField] GameObject paddle;
    [SerializeField] float bounciness = 1.005f;
    [SerializeField] bool rightSide = true;

    [Header("Velocity Mods")]
    [SerializeField] Vector3 lastVelocity;
    [SerializeField] Vector3 oldVelocity;
    [SerializeField] bool isOut = false;


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
    private void LateUpdate()
    {
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
        if (transform.parent != null && rb.velocity.magnitude > 0 && paddle != null)
        {
            BindToPaddle();
        }
    }
    // Update is called once per frame
    void Update()
    {

        if (transform.parent == null && rb.velocity.magnitude == 0)
        {
            rb.velocity = lastVelocity;
        }

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

    public void IgnoreBallCollisions()
    {
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach (GameObject ball in balls)
        {
            Physics.IgnoreCollision(ball.GetComponent<Collider>(), this.GetComponent<Collider>(), true);
        }

    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.collider.tag)
        {

            default:
                generalHit.PlayOnce();
                Reflect(other, bounciness);
                break;
            case "Paddle":
                generalHit.PlayOnce();
                if (other.collider.GetComponentInParent<PowerUpManager>().catcher && GetComponent<PlayerTracker>().GetMainOwner() != 0)
                {

                    if (!other.collider.GetComponentInChildren<AimArrow>().Aiming)
                    {
                        if (other.GetContact(0).normal == (Vector3.right * inwardSign))
                        {
                            PaddleCatch();
                            Debug.Log("Catch");
                        }

                    }

                }
                IncreaseVelocity(bounciness);
                break;
            case "Ball":
                
                break;
            case "Brick":
                Reflect(other, bounciness);
                if (onFire)
                {
                    other.collider.GetComponent<BrickHealth>().TakeDamge(fireDamage, rightSide);
                }
                else
                {
                    other.collider.GetComponent<BrickHealth>().TakeDamge(1, rightSide);
                }
                break;

        }

    }
    private void Reflect(Collision collision, float percent)
    {
        Vector3 reflected = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);
        rb.velocity = reflected;
        IncreaseVelocity(percent);
    }
    private void OnDisable()
    {
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

    public void BindToPaddle()
    {
        if (paddle == null)
        {
            return;
        }
        rb.velocity = Vector3.zero;
        transform.SetParent(paddle.transform);

        transform.position = paddle.GetComponent<PaddleController>().BallPosition.position;


    }

    public void PaddleCatch()
    {
        if (paddle == null)
        {
            return;
        }
        paddle.GetComponentInChildren<AimArrow>().CanHit = true;
        StartCoroutine(SetKinematic(1));
        transform.SetParent(paddle.transform);
        paddle.GetComponentInChildren<AimArrow>().Aiming = true;
    }

    IEnumerator SetKinematic(int frames)
    {
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;
        for (int loop = 0; loop < frames; loop++)
        {
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
        //Debug.Log(power + " Power " + rb.velocity.magnitude + " Velocity ");
    }

    public void OnBallOut()
    {
        BindToPaddle();
        isOut = true;

    }


    public void StartRandomReturn(float velocity, Transform point)
    {
        StartCoroutine(RandomReturn(5, velocity, point));
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

        //Debug.Log("velocity is " + velocity);
        //direction = RotateVector(direction, Random.Range(-5, 5));
        //rb.AddForce(direction * velocity, ForceMode.VelocityChange);
        rb.velocity = direction * velocity;

    }

}

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

    [SerializeField] Material flameMaterial;
    [SerializeField] Material defaultMaterial;

    Rigidbody rb;
    int inwardSign;





    public float Radius { get => radius; set => radius = value; }
    public float Bounciness { get => bounciness; set => bounciness = value; }
    public bool RightSide { get => rightSide; set => rightSide = value; }
    public bool IsOut { get => isOut; set => isOut = value; }
    public bool CurrentPlayer1 { get => currentPlayer1; set => currentPlayer1 = value; }

    public void FlameOn()
    {
        GetComponent<MeshRenderer>().material = flameMaterial;
        onFire = true;
    }

    public void FlameOff()
    {
        GetComponent<MeshRenderer>().material = defaultMaterial;
        onFire = false;
    }
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

    // Update is called once per frame
    void Update()
    {
        oldVelocity = lastVelocity;
        lastVelocity = rb.velocity;
        
        if(lastVelocity.magnitude < oldVelocity.magnitude){
            if(transform.parent == null && !rb.velocity.Equals(Vector3.zero) && oldVelocity.magnitude > 0){
                Debug.Log("maintain velocity");
                SetVelocityMagnitude(oldVelocity.magnitude);
            }
        }
        if (transform.parent != null && rb.velocity.magnitude > 0)
        {
            BindToPaddle();
        }

    }
    public void SetVelocityMagnitude(float magnitude){
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
                Reflect(other, bounciness);
                break;
            case "Paddle":

                if (other.collider.GetComponentInParent<PowerUpManager>().catcher && !other.collider.GetComponentInChildren<AimArrow>().Aiming)
                {
                    PaddleCatch();
                }
                else
                {
                    Debug.Log("Reflected");
                    Reflect(other, bounciness);
                }

                break;
            case "Ball":
                Physics.IgnoreCollision(GetComponent<Collider>(), other.collider);
                break;


        }

    }
    private void Reflect(Collision collision, float percent)
    {
        Vector3 reflected = Vector3.Reflect(lastVelocity, collision.GetContact(0).normal);
        if (reflected.x == (-1 * lastVelocity.x) || reflected.z == (-1 * lastVelocity.z))
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                Vector3 newReflected = Quaternion.AngleAxis(10, Vector3.up) * reflected;
                rb.velocity = newReflected;
            }
            else
            {
                Vector3 newReflected = Quaternion.AngleAxis(-10, Vector3.up) * reflected;
                rb.velocity = newReflected;
            }
        }
        else
        {
            rb.velocity = reflected;
        }
        IncreaseVelocity(percent);
    }
    public void IncreaseVelocity(float percent)
    {
        Vector3 direction = rb.velocity.normalized;
        float newMag = rb.velocity.magnitude * percent;
        rb.velocity = newMag * direction;

    }
    public void BindToPaddle()
    {
        IsOut = false;
        rb.velocity = Vector3.zero;
        transform.SetParent(paddle.transform);
        int sign = 1;
        if (!rightSide)
        {
            sign = -1;
        }
        transform.localPosition = (sign * Vector3.left) * (0.5f + radius);


    }

    public void PaddleCatch()
    {
        rb.velocity = Vector3.zero;
        transform.SetParent(paddle.transform);
        paddle.GetComponentInChildren<AimArrow>().Aiming = true;
    }

    public void Fire(float power, Vector3 direction)
    {
        transform.parent = null;
        rb.velocity = inwardSign * (direction * power);

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
                    paddle.GetComponent<PaddleController>().GoneOut();
                }

                paddle.GetComponent<PowerUpManager>().ResetPowerUp();

                if (rightSide)
                {
                    GameManager.instance.Missed[0]++;
                }
                else
                {
                    GameManager.instance.Missed[1]++;
                }
                break;
        }
    }

    public void SwitchPlayer()
    {
        currentPlayer1 = !currentPlayer1;
        // change material
    }

}

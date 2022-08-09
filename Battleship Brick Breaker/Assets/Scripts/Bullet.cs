using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool isActive = false;
    public bool player1 = true;
    [SerializeField] GameObject actives;
    [SerializeField] GameObject inactives;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        DisableBullet();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Paddle":
                if (other != GetComponent<Collider>())
                {
                    
                    DisableBullet();
                    if(other.GetComponentInParent<PaddleController>() != null){
                        other.GetComponentInParent<PaddleController>().StartHitPenalty();
                    }
                    
                    if (player1)
                    {
                        GameManager.instance.ShotsHit[0]++;
                    }
                    else
                    {
                        GameManager.instance.ShotsHit[1]++;
                    }
                }
                //
                break;
            case "BulletDespawn":
                DisableBullet();
                break;
        }
    }

    public void Launch(float height, Vector3 target, float gravity)
    {
        isActive = true;
        if (gravity > 0)
        {
            Physics.gravity = Vector3.up * (gravity * -1);
        }
        else
        {
            Physics.gravity = Vector3.up * gravity;
        }
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity(height, target);
    }
    public void EnableBullet(Vector3 position)
    {
        transform.position = position;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        transform.SetParent(actives.transform);
    }
    public void DisableBullet()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        transform.SetParent(inactives.transform);

        isActive = false;
    }

    public Vector3 CalculateLaunchVelocity(float height, Vector3 target)
    {
        float gravity = Physics.gravity.y;
        float dispY = target.y - transform.position.y;
        Vector3 dispXZ = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = dispXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (dispY - height) / gravity));

        return velocityXZ + velocityY;
    }
}

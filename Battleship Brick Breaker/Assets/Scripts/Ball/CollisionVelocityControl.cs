using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionVelocityControl : MonoBehaviour
{
    Vector3 lastVelocity;
    Vector3 oldVelocity;
    Vector3 lastCollVelocity;
    public float largestMagnitude;
    Rigidbody rb;

    public float LargestMagnitude { get => largestMagnitude; set => largestMagnitude = value; }
    public Vector3 LastVelocity { get => lastVelocity; set => lastVelocity = value; }

    // Start is called before the first frame update
    void Start()
    {
        largestMagnitude = 0;
        lastVelocity = Vector3.zero;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (!rb.isKinematic && transform.parent == null)
        {
            oldVelocity = lastVelocity;
            lastVelocity = rb.velocity;
            if (Mathf.Abs(rb.velocity.magnitude) > largestMagnitude)
            {
                largestMagnitude = rb.velocity.magnitude;
            }
        }

        if(!GetComponent<BallPhysics>().IsOut){
            if(rb.velocity.magnitude < largestMagnitude){
                //GameManager.instance.ApplyForceToVelocity(rb, rb.velocity.normalized * largestMagnitude, 10000);
            }
        }

    }
    private void OnCollisionExit(Collision other)
    {

    }

}

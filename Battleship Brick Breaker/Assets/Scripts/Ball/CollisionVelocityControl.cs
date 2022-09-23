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

    BallPhysics ballPhysics;


    public float LargestMagnitude { get => largestMagnitude; set => largestMagnitude = value; }
    public Vector3 LastVelocity { get => lastVelocity; set => lastVelocity = value; }

    // Start is called before the first frame update
    void Start()
    {
        largestMagnitude = 0;
        lastVelocity = Vector3.zero;
        rb = GetComponent<Rigidbody>();
        ballPhysics = GetComponent<BallPhysics>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        
        if (transform.parent == null)
        {
            if (!rb.isKinematic)
            {
                oldVelocity = lastVelocity;
                lastVelocity = rb.velocity;
                if (Mathf.Abs(rb.velocity.magnitude) > largestMagnitude)
                {
                    largestMagnitude = rb.velocity.magnitude;
                }
            }
        }

        if (!ballPhysics.IsBoundToPaddle)
        {
            if (rb.velocity.magnitude < largestMagnitude)
            {
                GameManager.instance.ApplyForceToVelocity(rb, lastVelocity.normalized * largestMagnitude, 10000);
            }


        }


    }
    private void OnCollisionExit(Collision other)
    {

    }

}

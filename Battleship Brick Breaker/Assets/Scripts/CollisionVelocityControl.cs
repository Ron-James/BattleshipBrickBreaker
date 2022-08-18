using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionVelocityControl : MonoBehaviour
{
    Vector3 lastVelocity;
    Vector3 oldVelocity;
    Vector3 lastCollVelocity;
    float largestMagnitude;
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
                largestMagnitude = Mathf.Abs(rb.velocity.magnitude);
            }
            if (Mathf.Abs(lastVelocity.magnitude) < Mathf.Abs(oldVelocity.magnitude))
            {
                Vector3 newVelocity = largestMagnitude * lastVelocity.normalized;
                GameManager.instance.ApplyForceToVelocity(rb, newVelocity, 10000);
                //rb.velocity = newVelocity;
            }
        }

    }
    private void OnCollisionExit(Collision other)
    {

    }

}

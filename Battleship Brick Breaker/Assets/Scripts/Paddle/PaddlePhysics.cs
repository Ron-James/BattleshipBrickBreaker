using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddlePhysics : MonoBehaviour
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] bool player1;
    [SerializeField] float maxBounceAngle = 75f;
    int sign;
    // Start is called before the first frame update
    void Start()
    {
        if(player1){
            sign = -1;
        }
        else{
            sign = 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        BallPhysics ball = other.collider.gameObject.GetComponent<BallPhysics>();
        if(ball != null){
            //Debug.Log("Hit paddle");
            Vector3 paddlePos = transform.position;
            Vector3 contactPoint = other.GetContact(0).point;

            float offset = paddlePos.z - contactPoint.z;
            float width = boxCollider.bounds.size.z/2;

            float currentAngle = Vector3.SignedAngle(sign * Vector3.right, ball.gameObject.GetComponent<Rigidbody>().velocity, Vector3.up);
            float bounceAngle = (offset / width) * this.maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle);

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.up);
            ball.gameObject.GetComponent<Rigidbody>().velocity = rotation * (Vector3.right * sign) * ball.gameObject.GetComponent<Rigidbody>().velocity.magnitude;

        }
    }
}

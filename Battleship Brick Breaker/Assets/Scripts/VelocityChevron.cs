using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityChevron : MonoBehaviour
{
    [SerializeField] float velocityMultiplier = 1f;
    [SerializeField] Transform direction;

    bool velocityChanged = false;
    Transform lastBall;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator VelocityChangeDelay(float delay){
        yield return new WaitForSeconds(delay);
        lastBall = null;
        velocityChanged = false;
    }

    private void OnTriggerExit(Collider other){
        if(other.transform.Equals(lastBall)){
            StartCoroutine(VelocityChangeDelay(Time.fixedDeltaTime * 5));
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch(other.tag){
            default:
                return;
            case "Ball":
                Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody>();
                if(rigidbody != null){
                    if(!velocityChanged){
                        Vector3 currentVelocity = rigidbody.velocity;
                        float currentMagnitude = currentVelocity.magnitude;
                    
                        Vector3 forceDirection = direction.forward.normalized;
                        Debug.Log(currentMagnitude + " Current Magnitude " + currentMagnitude * velocityMultiplier * forceDirection);
                    
                        //GameManager.instance.ApplyForceToVelocity(rigidbody, (0.000001f * Vector3.one), Mathf.Infinity);
                        //rigidbody.AddForce(currentMagnitude * forceDirection * velocityMultiplier, ForceMode.VelocityChange);
                        rigidbody.velocity = currentMagnitude * forceDirection;
                        Debug.Log(rigidbody.velocity.magnitude + "New Magnitude");
                        velocityChanged = true;
                        lastBall = other.transform;
                    }
                    else{
                        return;
                    }
                    
                }
            break;
        }
    }


    
}

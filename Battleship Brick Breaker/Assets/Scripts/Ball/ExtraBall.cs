using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBall : MonoBehaviour
{

    [SerializeField] GameObject actives;
    [SerializeField] GameObject inactives;

    bool inPlay;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DisableBall();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if(inPlay){
            if(rb.velocity.magnitude < GameManager.instance.InitialVelocity){
                Vector3 direction = Vector3.right;
                direction = Quaternion.AngleAxis(Random.Range(-360, 361), Vector3.up) * direction;
                GameManager.instance.ApplyForceToVelocity(rb, GameManager.instance.InitialVelocity * direction.normalized, 10000);
            }
        }
    }

    public void EnableBall(bool player1, Vector3 position, float speed){
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
        transform.SetParent(null);
        GetComponent<PlayerTracker>().SetCurrentOwner(player1);
        transform.position = position;
        Vector3 direction = Vector3.right;
        direction = Quaternion.AngleAxis(Random.Range(-360, 361), Vector3.up) * direction;
        rb.isKinematic = false;
        GameManager.instance.ApplyForceToVelocity(rb, direction * GameManager.instance.InitialVelocity, 10000);
        inPlay = true;
    }
    public void DisableBall(){
        transform.SetParent(inactives.transform);
        transform.localPosition = Vector3.zero;
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
        inPlay = false;
    }

    public void OnBallOut(){
        Debug.Log("should disable here");
        DisableBall();
    }
}

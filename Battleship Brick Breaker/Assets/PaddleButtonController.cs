using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaddleButtonController : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float maxSpeed = 10;
    [SerializeField] float posAccelaration = 10;
    [SerializeField] float slowDownTime = 0.6f;
    [SerializeField] bool upButtonDown;
    [SerializeField] bool downButtonDown;
    Coroutine reduceVelocity;
    Coroutine increaseVelocity;
    int direction = 0;
    PaddleController paddleController;
    public void UpdateButtonInput()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        paddleController = GetComponent<PaddleController>();
        upButtonDown = false;
        downButtonDown = false;
        reduceVelocity = null;
        rb = GetComponent<Rigidbody>();
        if(paddleController.controlScheme == PaddleController.ControlScheme.button){
            Debug.Log("Button");
            rb.isKinematic = false;
        }
    }

    public void SetUpButton(bool down){
        upButtonDown = down;
    }
    public void SetDownButton(bool down){
        downButtonDown = down;
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if(!rb.isKinematic){
            if(paddleController.controlScheme == PaddleController.ControlScheme.button){
                if(!paddleController.IsStopped){
                    ApplyButtonInput();
                }
                else{
                    rb.velocity = Vector3.zero;
                }
            }
            
        }
        
    }

    public void ApplyButtonInput(){
        if(upButtonDown && !downButtonDown){
            direction = 1;
            ApplyForceToVelocity(rb, Vector3.forward * maxSpeed, posAccelaration);
        }
        else if(!upButtonDown && downButtonDown){
            direction = -1;
            ApplyForceToVelocity(rb, -Vector3.forward * maxSpeed, posAccelaration);
        }
        else if(!downButtonDown && !upButtonDown){
            direction = 0;
            if(reduceVelocity == null){
                reduceVelocity = StartCoroutine(ReduceVelocity(slowDownTime));
            }
        }
        else if(downButtonDown && upButtonDown){
            direction = 0;
            if(reduceVelocity == null){
                reduceVelocity = StartCoroutine(ReduceVelocity(slowDownTime));
            }
        }
    }

    public void ApplyForceToVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        //Debug.Log(velocity.magnitude + " Increase to velocity");
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedtoTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedtoTarget) * force, mode);
        }

    }

    public Vector3 ForceToApplyToVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return Vector3.zero;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);
        if (rigidbody.velocity.magnitude == 0)
        {
            //rigidbody.AddForce(velocity * force, mode);
            return velocity * force;
        }
        else
        {
            var velocityProjectedtoTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            //rigidbody.AddForce((velocity - velocityProjectedtoTarget) * force, mode);
            return (velocity - velocityProjectedtoTarget) * force;
        }
    }


    IEnumerator IncreaseVelocity(float period, Vector3 velocity)
    {
        float time = 0;
        if (velocity.magnitude < rb.velocity.magnitude)
        {
            yield return null;

        }
        else
        {
            float increaseRate = (velocity.magnitude - rb.velocity.magnitude) / (period / Time.fixedDeltaTime);
            while (true)
            {
                time += Time.fixedDeltaTime;

                if (rb.velocity.magnitude + increaseRate >= velocity.magnitude)
                {
                    ChangeVelocity(velocity.magnitude);
                    increaseVelocity = null;
                    break;
                }
                else if (time >= period)
                {
                    ChangeVelocity(velocity.magnitude);
                    increaseVelocity = null;
                    break;
                }
                else
                {
                    float vel = rb.velocity.magnitude;
                    ChangeVelocity(vel + increaseRate);
                    yield return new WaitForFixedUpdate();
                }
            }
        }
        float rate = velocity.magnitude - rb.velocity.magnitude;
    }
    IEnumerator ReduceVelocity(float period)
    {
        float time = 0;
        float rate = rb.velocity.magnitude / (period / Time.fixedDeltaTime);
        Debug.Log("Reduce velocity");
        while (true)
        {
            time += Time.fixedDeltaTime;
            if (rb.velocity.magnitude - rate <= 0)
            {
                rb.velocity = Vector3.zero;
                reduceVelocity = null;
                //yield return null;
                break;
            }
            else if (time >= period || rb.velocity.Equals(Vector3.zero))
            {
                Debug.Log("Reduce velocity");
                rb.velocity = Vector3.zero;
                reduceVelocity = null;
                //yield return null;
                break;
            }
            else if(upButtonDown || downButtonDown){
                reduceVelocity = null;
                //yield return null;
                break;
            }
            else
            {
                float velocity = rb.velocity.magnitude;

                ChangeVelocity(velocity - rate);
                yield return new WaitForFixedUpdate();
            }
        }
    }

    public void ChangeVelocity(float magnitude)
    {
        if (magnitude == 0)
        {
            return;
        }
        Vector3 direction = rb.velocity.normalized;
        ApplyForceToVelocity(rb, direction * magnitude, 10000);
        //rb.velocity = direction * magnitude;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickEffects : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] GameObject cracked;
    [SerializeField] GameObject sinkBrick;
    [SerializeField] ParticleSystem splash;
    [SerializeField] Sound splashSound;
    [SerializeField] float fallSpeed;
    [SerializeField] AnimationCurve fallCurve;
    [SerializeField] float fallTime = 2f;
    [SerializeField] float fallDistance = 3.5f;
    Rigidbody rb;
    Vector3 defaulPos;


    // Start is called before the first frame update
    void Start()
    {
        defaulPos = transform.position;
        rb = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        ResetBrick();
    }
    public void ResetBrick(){
        meshRenderer.enabled = true;
        sinkBrick.SetActive(false);
        cracked.SetActive(false);
        splashSound.StopSource();
        splash.Stop();
    }

    public void EnableBasic(){
        meshRenderer.enabled = true;
        sinkBrick.SetActive(false);
        cracked.SetActive(false);
    }

    public void EnableCrackBrick(){
        meshRenderer.enabled = false;
        sinkBrick.SetActive(false);
        cracked.SetActive(true);
    }

    public void EnableSinkBrick(){
        meshRenderer.enabled = false;
        sinkBrick.SetActive(true);
        cracked.SetActive(false);
    }

    public void DisableAll(){
        meshRenderer.enabled = false;
        sinkBrick.SetActive(false);
        cracked.SetActive(false);
    }
    public void StartSinkBrick(){
        StartCoroutine(SinkBrick(fallTime, fallDistance));
    }
    IEnumerator SinkBrick(float duration, float fallDistance){
        float time = 0;
        EnableSinkBrick();
        Vector3 target = transform.position;
        target.y = -Mathf.Abs(fallDistance);
        //rb.isKinematic = false;
        //rb.useGravity = true;

        
        while(true){
            time += Time.deltaTime;
            float ratio = time / duration;
            float speed = fallCurve.Evaluate(ratio) * fallSpeed;
            if(transform.position.y >= target.y){
                DisableAll();
                transform.position = defaulPos;
                Splash();
                //rb.isKinematic = false;
                //rb.useGravity = false;
                break;
            }   
            else{
                //Debug.Log(transform.position.y + "brick height");
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                yield return null;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Splash(){
        Vector3 splashPos = transform.position;
        splashPos.y = 1.1f;
        splash.transform.position = splashPos;
        splash.Play();
        splashSound.PlayOnce();
        StartCoroutine(SplashDelay(5f));
    }

    IEnumerator SplashDelay(float time){
        yield return new WaitForSeconds(time);
        splash.Stop();
    }
    /*
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Backboard":
                //Splash();
                break;
        }
    }
    */
}

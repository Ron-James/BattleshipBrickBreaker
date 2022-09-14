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
        StartCoroutine(SinkBrick());
    }
    IEnumerator SinkBrick(){
        float time = 0;
        EnableSinkBrick();
        Vector3 target = transform.position;
        target.y = -4f;
        rb.isKinematic = false;
        rb.useGravity = true;
        
        while(true){
            if(time >= 3f){
                DisableAll();
                transform.position = defaulPos;
                rb.isKinematic = false;
                rb.useGravity = false;
                break;
            }   
            else{
                //Debug.Log(transform.position.y + "brick height");
                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
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
    }
    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "Backboard":
                Splash();
                break;
        }
    }
}

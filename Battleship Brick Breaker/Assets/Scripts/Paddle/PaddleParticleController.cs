using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleParticleController : MonoBehaviour
{
    [SerializeField] ParticleSystem boatHit;
    Coroutine hitEffect;
    // Start is called before the first frame update
    void Start()
    {
        hitEffect = null;
        boatHit.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBoatHitEffect(Vector3 position){
        if(hitEffect == null){
            hitEffect = StartCoroutine(BoatHitEffect(position));
        }
    }

    IEnumerator BoatHitEffect(Vector3 position){
        Vector3 defaultPos = boatHit.transform.position;
        Vector3 particlePos = boatHit.transform.position;
        particlePos.z = position.z;
        boatHit.transform.position = particlePos;

        float duration = boatHit.main.duration;
        float time = 0;

        boatHit.Play();
        while(true){
            if(time >= duration){
                boatHit.Stop();
                boatHit.transform.position = defaultPos;
                hitEffect = null;
                break;
            }
            else{
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
}

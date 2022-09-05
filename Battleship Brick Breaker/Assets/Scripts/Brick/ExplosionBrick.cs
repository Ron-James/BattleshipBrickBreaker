using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionBrick : MonoBehaviour
{
    [SerializeField] ParticleSystem explosion;
    [SerializeField] float radius;
    // Start is called before the first frame update
    void Start()
    {
        explosion.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Explode(bool player1){
        Collider [] colliders = Physics.OverlapSphere(transform.position, radius);
        StartCoroutine(PlayParticleEffect());
        foreach(Collider element in colliders){
            if(element.GetComponent<BrickHealth>() != null){
                element.GetComponent<BrickHealth>().TakeDamge(1, player1);
            }
        }
    }

    IEnumerator PlayParticleEffect(){
        float duration = explosion.main.duration;
        float time = 0;
        explosion.Play();
        while(true){
            if(time >= duration){
                explosion.Stop();
                break;
            }
            else{
                time += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void OnDrawGizmos() {
        Color color = Color.magenta;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);

    }
}

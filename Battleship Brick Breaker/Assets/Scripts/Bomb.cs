using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject actives;
    public GameObject inactives;
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float brickDamage = 2;
    [SerializeField] ParticleSystem explosion;
    AudioSource src;
    [SerializeField] AudioClip explosionSound;
    bool isActive;
    bool player1 = true;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        DisableBomb();
        src = GetComponent<AudioSource>();

    }
    public void PlayExplosionSound(){
        src.clip = explosionSound;
        src.Play();
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Explode()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, explosionRadius);
        Debug.Log("explosion");
        PlayExplosionSound();
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider item in colliders)
        {
            if (item.GetComponent<BrickHealth>() != null)
            {
                if (player1)
                {
                    item.GetComponent<BrickHealth>().TakeDamge(brickDamage, 1);
                }
                else
                {
                    item.GetComponent<BrickHealth>().TakeDamge(brickDamage, 2);
                }

            }
            else if (item.GetComponentInParent<PaddleController>() != null)
            {
                item.GetComponentInParent<PaddleController>().StartBombPenalty();
            }
        }
        StartCoroutine(ExplosionEffect());
    }
    public void EnableBomb(Vector3 position)
    {
        transform.position = position;
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        //transform.SetParent(actives.transform);
    }
    public void DisableBomb()
    {
        transform.SetParent(inactives.transform);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.localPosition = Vector3.zero;
        GetComponentInChildren<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        
        player1 = true;
        isActive = false;
    }
    public Vector3 CalculateLaunchVelocity(float height, Vector3 target)
    {
        float gravity = Physics.gravity.y;
        float dispY = target.y - transform.position.y;
        Vector3 dispXZ = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * height);
        Vector3 velocityXZ = dispXZ / (Mathf.Sqrt(-2 * height / gravity) + Mathf.Sqrt(2 * (dispY - height) / gravity));

        return velocityXZ + velocityY;
    }

    public void LaunchBomb(Vector3 target, float height, bool Player1)
    {
        Debug.Log(target + " Bomb Target");
        player1 = Player1;
        isActive = true;
        transform.SetParent(null);
        Vector3 launchVelocity = CalculateLaunchVelocity(height, target);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = launchVelocity;
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "Brick":
                if (isActive)
                {
                    Explode();
                }

                break;
            case "BombTrigger":
                if (isActive)
                {
                    Explode();
                }
                break;
            case "Paddle":
                if (isActive)
                {
                    Explode();
                }
                break;
        }
    }
    IEnumerator ExplosionEffect()
    {
        float duration = explosion.main.duration + 0.5f;
        float time = 0;

        explosion.Play();
        while (true)
        {
            if (time >= duration)
            {
                explosion.Stop();
                GetComponent<Collider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                DisableBomb();
                break;
            }
            else
            {
                time += Time.deltaTime;
                yield return null;
            }
        }
    }
    
}

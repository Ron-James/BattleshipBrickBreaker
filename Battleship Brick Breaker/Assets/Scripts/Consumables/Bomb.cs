using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public GameObject actives;
    public GameObject inactives;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Collider bombCollider;
    [SerializeField] float explosionRadius = 5;
    [SerializeField] float brickDamage = 2;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] ParticleSystem fuseBurn;
    [SerializeField] Sound explosionSound;
    [SerializeField] Sound fuseSound;
    bool isActive;
    bool player1 = true;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isActive = false;
        DisableBomb();

        explosionSound.src = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Explode()
    {
        //Gizmos.color = Color.red;
        //Gizmos.DrawSphere(transform.position, explosionRadius);
        Debug.Log("Exploded " + gameObject.name);
        fuseBurn.Pause();
        fuseSound.StopSource();
        explosionSound.PlayOnce();
        isActive = false;
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider item in colliders)
        {
            if (item.GetComponent<BrickHealth>() != null)
            {
                item.GetComponent<BrickHealth>().TakeDamge(brickDamage, player1);


            }
            else if (item.GetComponentInParent<HandicapController>() != null)
            {
                item.GetComponentInParent<HandicapController>().AddHandicapTime(GameManager.instance.BombHitPenalty);
                GameManager.instance.ChangeAllBalls(player1);
            }
        }
        StartCoroutine(ExplosionEffect());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void EnableBomb(Vector3 position)
    {
        fuseBurn.Play();
        fuseSound.PlayLoop();
        transform.position = position;
        Debug.Log(position + " Bomb position");
        
        bombCollider.enabled = true;
        meshRenderer.enabled = true;
        //transform.SetParent(actives.transform);
    }
    public void DisableBomb()
    {
        transform.SetParent(inactives.transform);
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.localPosition = Vector3.zero;
        bombCollider.enabled = false;
        meshRenderer.enabled = false;
        fuseSound.StopSource();
        fuseBurn.Stop();
        explosion.Stop();
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

        //Debug.Log(target + " Bomb Target");
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
                    if (player1)
                    {
                        if (!other.gameObject.GetComponentInParent<PaddleController>().Player1)
                        {
                            Explode();
                        }
                    }
                    else
                    {
                        if (other.gameObject.GetComponentInParent<PaddleController>().Player1)
                        {
                            Explode();
                        }
                    }

                }
                break;
        }
    }
    IEnumerator ExplosionEffect()
    {
        float duration = explosion.main.duration;
        float time = 0;
        explosion.transform.position = transform.position + (2 * Vector3.up);
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

    /// <summary>
    /// Callback to draw gizmos that are pickable and always drawn.
    /// </summary>
    void OnDrawGizmos()
    {
        Color color = Color.red;
        color.a = 0.2f;
        Gizmos.color = color;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);

    }

}

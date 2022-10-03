using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public enum BulletType{
        cannon = 0,
        snow = 1
    }
    public bool isActive = false;
    public bool player1 = true;
    [SerializeField] GameObject actives;
    [SerializeField] GameObject inactives;
    [SerializeField] BulletType bulletType = BulletType.cannon;
    [SerializeField] GameObject cannonBall;
    [SerializeField] GameObject snowBall;

    

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        isActive = false;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        DisableBullet();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "Paddle":
                Debug.Log("Hit boat " + other.gameObject.name);

                if (other.GetComponentInParent<PaddleController>() != null)
                {
                    if (other.GetComponentInParent<PaddleController>().Player1 != player1)
                    {
                        if(bulletType == BulletType.cannon){
                            PaddleController paddle = other.GetComponentInParent<PaddleController>();
                            paddle.GetComponent<HandicapController>().AddHandicapTime(GameManager.instance.CannonballHitPenalty, true);
                            paddle.GetComponent<PaddleParticleController>().StartBoatHitEffect(transform.position);
                            paddle.gameObject.GetComponentInChildren<PaddleSoundBox>().boatHit.PlayOnce();
                            GameManager.instance.ChangeAllBalls(player1);
                            DisableBullet();
                            GameManager.instance.GetPaddle(player1).Ball.GetComponent<PlayerTracker>().BuffBallDamage(5);
                            
                        }
                        else if(bulletType == BulletType.snow){
                            other.gameObject.GetComponentInParent<PaddleButtonController>().AddSnowSlowTime(GameManager.instance.SnowSlowDuration);
                            DisableBullet();
                        }
                        
                    }


                }
                
                //
                break;
            case "BulletDespawn":
                DisableBullet();
                break;
        }
    }

    public void Launch(float height, Vector3 target, bool player)
    {
        player1 = player;
        isActive = true;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().velocity = CalculateLaunchVelocity(height, target);
    }
    public void EnableBullet(Vector3 position, bool player, BulletType type)
    {
        bulletType = type;
        UpdateMaterial();
        player1 = player;
        transform.position = position;
    
        GetComponent<Collider>().enabled = true;
        
        transform.SetParent(actives.transform);
    }
    public void DisableBullet()
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        transform.SetParent(inactives.transform);
        cannonBall.SetActive(false);
        snowBall.SetActive(false);
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

    public void UpdateMaterial(){
        switch((int) bulletType){
            case 0:
                cannonBall.SetActive(true);
                snowBall.SetActive(false);
            break;
            case 1:
                cannonBall.SetActive(false);
                snowBall.SetActive(true);
            break;
            default:
                cannonBall.SetActive(true);
                snowBall.SetActive(false);
            break;
        }
    }
}

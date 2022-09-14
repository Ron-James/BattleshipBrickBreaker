using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BrickHealth : MonoBehaviour
{
    public enum BrickType
    {
        Basic = 0,
        PowerUp = 1,
        Ammo = 2,
        Objective = 3,
        Split = 4,
        Explosion = 5
    }
    [SerializeField] float maxHealth = 3f;
    [SerializeField] bool objective = false;
    [SerializeField] bool ammo = false;
    [SerializeField] bool powerUp = false;
    [SerializeField] bool split = false;
    [SerializeField] BrickType brickType;

    [SerializeField] GameObject crack;
    [SerializeField] GameObject feedbackOverlay;
    [SerializeField] bool tutorial;


    public float currentHealth;
    [SerializeField] BrickEffects brickEffects;
    public bool isBroken = false;
    [Header("Audio")]
    [SerializeField] Sound hitSound;
    [SerializeField] UnityEvent OnBrickBreak;
    [SerializeField] UnityEvent OnBrickCrack;
    // Start is called before the first frame update

    void Start()
    {
        
        hitSound.src = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        if (crack != null)
        {
            crack.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {
        switch (other.collider.tag)
        {
            case "Ball":
                hitSound.PlayOnce();
                break;
        }
    }

    public void TakeDamge(float amount, bool player1)
    {
        currentHealth -= amount;
        //Debug.Log("damage take by player" + player);
        if (currentHealth == 1)
        {
            OnBrickCrack.Invoke();
        }
        if (currentHealth <= 0)
        {
            BreakBrick(player1);
            OnBrickBreak.Invoke();
            
        }
        else
        {
            StartCoroutine(FlashOverlay(0.3f));
        }
    }
    public void SetColliderActive(bool active){
        GetComponent<Collider>().enabled = active;
    }

    IEnumerator DisableColliderDelay(float duration){
        float time = 0;
        SetColliderActive(true);
        while(true){
            if(time >= Time.deltaTime){
                SetColliderActive(false);
                break;
            }
            else{
                yield return null;
            }
        }

    }
    public void BreakBrick(bool player1)
    {
        //StartCoroutine(DisableColliderDelay(0.2f));
        SetColliderActive(false);
        isBroken = true;
        Vector3 position = transform.position;
        if (crack != null)
        {
            crack.SetActive(false);
        }
        if (player1)
        {
            StatsManager.instance.bricksBroken[0]++;
        }
        else
        {
            StatsManager.instance.bricksBroken[0]++;
        }
        switch ((int)brickType)
        {
            case 0://Basic Brick
                if (GameManager.instance.NumberOfActivePowerUps(player1) > 0)
                {
                    return;
                }
                int random = Random.Range(0, GameManager.instance.PowerUpDropRate);
                //Debug.Log(random + " Random number");
                if (random == 0)
                {
                    position = transform.position;
                    position.y = 3;
                    GameManager.instance.SpawnPowerup(position, player1);
                    Debug.Log("Power up spawned " + player1);
                }
                
                break;
            case 1://power up
                position = transform.position;
                position.y = 3;
                GameManager.instance.SpawnPowerup(position, player1);
                Debug.Log("Power up spawned " + player1);
                break;
            case 2:// ammo
                if (player1)
                {
                    GameManager.instance.paddle1.GetComponent<Artillery>().AddAmmo(1);
                    FloatingText floatingText = AssetManager.instance.GetFloatingText();
                    if(floatingText != null){
                        floatingText.gameObject.transform.eulerAngles = new Vector3(0, -90, 0);
                        Vector3 pos = transform.position;
                        pos.y = 10f;
                        floatingText.DeployFloatingText(pos);
                    }
                }
                else
                {
                    GameManager.instance.paddle2.GetComponent<Artillery>().AddAmmo(1);
                    FloatingText floatingText = AssetManager.instance.GetFloatingText();
                    if(floatingText != null){
                        floatingText.gameObject.transform.eulerAngles = new Vector3(0, 90, 0);
                        Vector3 pos = transform.position;
                        pos.y = 10f;
                        floatingText.DeployFloatingText(pos);
                    }
                    
                }
                break;
            case 3:// objective
                GameManager.instance.AddScore(player1);
                if (player1)
                {
                    StatsManager.instance.objBroken[0]++;
                }
                else
                {
                    StatsManager.instance.objBroken[1]++;
                }

                break;
            case 4:// split
                if (player1)
                {
                    GameManager.instance.paddle1.GetComponent<PaddleController>().Ball.GetComponent<BallSplitter>().SplitBall(1);
                }
                else
                {
                    GameManager.instance.paddle2.GetComponent<PaddleController>().Ball.GetComponent<BallSplitter>().SplitBall(1);
                }
                break;
            case 5://explosive
                GetComponent<ExplosionBrick>().Explode(player1);
                if (player1)
                {
                    StatsManager.instance.bricksBroken[0]++;
                }
                else
                {
                    StatsManager.instance.bricksBroken[0]++;
                }
                break;
        }
    }



    public void Revive()
    {
        GetComponent<Collider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
        isBroken = false;
    }

    public void MirrorBrick()
    {
        Vector3 pos = transform.position;
        pos.x *= -1;

        transform.position = pos;
    }

    IEnumerator FlashOverlay(float duration)
    {
        float time = 0;
        feedbackOverlay.SetActive(true);
        while (true)
        {
            if (time >= duration)
            {
                feedbackOverlay.SetActive(false);
                break;
            }
            else
            {
                time += Time.deltaTime;
                yield return null;

            }
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}

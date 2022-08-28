using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 3f;
    [SerializeField] bool objective = false;
    [SerializeField] bool ammo = false;
    [SerializeField] bool powerUp = false;
    [SerializeField] bool split = false;

    [SerializeField] GameObject crack;
    [SerializeField] GameObject feedbackOverlay;
    [SerializeField] bool tutorial;


    public float currentHealth;

    public bool isBroken = false;
    [Header("Audio")]
    [SerializeField] Sound hitSound;
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
        if (currentHealth == 1 && crack != null)
        {
            crack.SetActive(true);
        }
        if (currentHealth <= 0)
        {
            Break(player1);
        }
        else
        {
            StartCoroutine(FlashOverlay(0.3f));
        }
    }


    public void Break(bool player1)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        if (crack != null)
        {
            crack.SetActive(false);
        }

        isBroken = true;
        if (!objective && !ammo && !split)
        {
            if (player1)
            {
                StatsManager.instance.bricksBroken[0]++;
            }
            else
            {
                StatsManager.instance.bricksBroken[0]++;
            }
            int random = Random.Range(0, GameManager.instance.PowerUpDropRate);
            //Debug.Log(random + " Random number");
            if (random == 0)
            {
                if (player1)
                {
                    Vector3 position = transform.position;
                    position.y = 1;
                    GameManager.instance.SpawnPowerup(position, true);
                }
                else
                {
                    Vector3 position = transform.position;
                    position.y = 1;
                    GameManager.instance.SpawnPowerup(position, false);
                }
            }
        }
        else if (split)
        {
            if (player1)
            {
                GameManager.instance.paddle1.GetComponent<PaddleController>().Ball.GetComponent<BallSplitter>().SplitBall(2);
            }
            else
            {
                GameManager.instance.paddle2.GetComponent<PaddleController>().Ball.GetComponent<BallSplitter>().SplitBall(2);
            }
        }
        else if (powerUp)
        {
            if (player1)
            {
                if (TutorialManager.instance.isTutorial)
                {
                    Vector3 position = transform.position;
                    position.y = 1;
                    GameManager.instance.SpawnPowerup(position, true, 4);
                }
                else
                {
                    Vector3 position = transform.position;
                    position.y = 1;
                    GameManager.instance.SpawnPowerup(position, true);
                }

            }

            else
            {
                if (TutorialManager.instance.isTutorial)
                {
                    Vector3 position = transform.position;
                    position.y = 1;
                    GameManager.instance.SpawnPowerup(position, false, 4);
                }
                else
                {
                    Vector3 position = transform.position;
                    position.y = 1;
                    GameManager.instance.SpawnPowerup(position, false);
                }
            }


        }

        else if (objective)
        {
            if (player1)
            {
                if (TutorialManager.instance.isTutorial)
                {
                    
                }
                GameManager.instance.AddScore(1);
                StatsManager.instance.objBroken[0]++;
            }
            else
            {
                if (TutorialManager.instance.isTutorial)
                {
                    
                }
                GameManager.instance.AddScore(2);
                StatsManager.instance.objBroken[0]++;
            }


        }
        else if (ammo)
        {
            if (player1)
            {
                if (TutorialManager.instance.isTutorial)
                {
                    TutorialManager.instance.cannonLaunch.OpenPrompt(true);
                }
                GameManager.instance.paddle1.GetComponent<Artillery>().AddAmmo(1);
            }
            else
            {
                if (TutorialManager.instance.isTutorial)
                {
                    TutorialManager.instance.cannonLaunch.OpenPrompt(false);
                }
                GameManager.instance.paddle2.GetComponent<Artillery>().AddAmmo(1);
            }
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

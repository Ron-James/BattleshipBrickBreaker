using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickHealth : MonoBehaviour
{
    [SerializeField] float maxHealth = 3f;
    [SerializeField] bool objective = false;
    [SerializeField] bool ammo = false;
    [SerializeField] bool powerUp = false;

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

    public void TakeDamge(float amount, int player)
    {
        currentHealth -= amount;
        //Debug.Log("damage take by player" + player);
        if (currentHealth == 1 && crack != null)
        {
            crack.SetActive(true);
        }
        if (currentHealth <= 0)
        {
            Break(player);
        }
        else
        {
            StartCoroutine(FlashOverlay(0.3f));
        }
    }


    public void Break(int player)
    {
        GetComponent<Collider>().enabled = false;
        GetComponent<MeshRenderer>().enabled = false;
        if (crack != null)
        {
            crack.SetActive(false);
        }

        isBroken = true;
        if (!objective && !ammo)
        {
            if (player == 1)
            {
                StatsManager.instance.bricksBroken[0]++;
            }
            else if (player == 2)
            {
                StatsManager.instance.bricksBroken[0]++;
            }
            if (!ammo && !powerUp)
            {
                int random = Random.Range(0, GameManager.instance.PowerUpDropRate);
                //Debug.Log(random + " Random number");
                if (random == 0)
                {
                    if (player == 1)
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
            else if (powerUp)
            {


                if (player == 1)
                {
                    if (TutorialManager.instance != null)
                    {
                        TutorialManager.instance.powerUpBrick.ClosePrompt(true);
                        TutorialManager.instance.powerUpCollect.OpenPrompt(true);
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
                    if (TutorialManager.instance != null)
                    {
                        TutorialManager.instance.powerUpBrick.ClosePrompt(false);
                        TutorialManager.instance.powerUpCollect.OpenPrompt(false);
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
        }
        else if (objective)
        {
            if (player == 1)
            {
                if (TutorialManager.instance != null)
                {
                    TutorialManager.instance.objBrick.ClosePrompt(true);
                }
                GameManager.instance.AddScore(1);
                StatsManager.instance.objBroken[0]++;
            }
            else if (player == 2)
            {
                if (TutorialManager.instance != null)
                {
                    TutorialManager.instance.objBrick.ClosePrompt(false);
                }
                GameManager.instance.AddScore(2);
                StatsManager.instance.objBroken[0]++;
            }


        }
        else if (ammo)
        {
            if (player == 1)
            {
                if (TutorialManager.instance != null)
                {
                    TutorialManager.instance.ammoBrick.ClosePrompt(true);
                    TutorialManager.instance.cannonLaunch.OpenPrompt(true);
                }
                GameManager.instance.paddle1.GetComponent<Artillery>().AddAmmo(1);
            }
            else
            {
                if (TutorialManager.instance != null)
                {
                    TutorialManager.instance.ammoBrick.ClosePrompt(false);
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

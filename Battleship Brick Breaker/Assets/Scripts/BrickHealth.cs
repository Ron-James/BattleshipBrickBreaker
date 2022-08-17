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

    public float currentHealth;

    public bool isBroken = false;
    [Header("Audio")]
    AudioSource audioSource;
    [SerializeField] AudioClip hitSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
                PlayHitSound();
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
            else if(powerUp){
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
        else if (objective)
        {
            if (player == 1)
            {
                GameManager.instance.AddScore(1);
                StatsManager.instance.objBroken[0]++;
            }
            else if (player == 2)
            {
                GameManager.instance.AddScore(2);
                StatsManager.instance.objBroken[0]++;
            }
        }
        else if (ammo)
        {
            if (player == 1)
            {
                GameManager.instance.paddle1.GetComponent<Artillery>().AddAmmo(1);
            }
            else
            {
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
    public void PlayHitSound(){
        audioSource.clip = hitSound;
        audioSource.Play();
    }
}

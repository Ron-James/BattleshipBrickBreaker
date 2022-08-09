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
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        if(crack != null){
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

                if (other.gameObject.GetComponent<BallPhysics>().onFire)
                {
                    TakeDamge(other.gameObject.GetComponent<BallPhysics>().fireDamage, (int) other.gameObject.GetComponent<PlayerTracker>().CurrentOwner1);
                }
                else
                {
                    TakeDamge(1, (int) other.gameObject.GetComponent<PlayerTracker>().CurrentOwner1);
                }

                break;
        }
    }

    public void TakeDamge(float amount, int player)
    {
        currentHealth -= amount;
        Debug.Log("damage take by player" + player);
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
        if(crack != null){
            crack.SetActive(false);
        }
        
        isBroken = true;
        if (!objective)
        {
            if (player == 1)
            {
                GameManager.instance.BricksBroken[0]++;
            }
            else if(player == 2)
            {
                GameManager.instance.BricksBroken[1]++;
            }
        }
        if (objective)
        {
            if (player == 1)
            {
                GameManager.instance.AddScore(1);
                GameManager.instance.ObjBroken[0]++;
            }
            else if(player == 2)
            {
                GameManager.instance.AddScore(2);
                GameManager.instance.ObjBroken[1]++;
            }
        }
        else if (powerUp)
        {
            if (player == 1)
            {
                GameManager.instance.SpawnPowerup(transform.position, true);
            }
            else{
                GameManager.instance.SpawnPowerup(transform.position, false);
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
}

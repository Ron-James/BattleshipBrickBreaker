using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    public enum Owner
    {
        none = 0,
        player1 = 1,
        player2 = 2
    }
    public enum CurrentOwner
    {
        player1 = 1,
        player2 = 2
    }

    [SerializeField] Owner owner;
    [SerializeField] CurrentOwner currentOwner;
    [SerializeField] Material player1Material;
    [SerializeField] Material player2Material;
    [SerializeField] bool isBuffed;
    [SerializeField] ParticleSystem fireParticles;
    [SerializeField] float buffTime;

    Coroutine buffCoroutine;
    public Owner Owner1 { get => owner; set => owner = value; }
    public CurrentOwner CurrentOwner1 { get => currentOwner; set => currentOwner = value; }


    public int GetCurrentOwner()
    {
        return (int)currentOwner;
    }
    public int GetMainOwner()
    {
        return (int)owner;
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetOwner();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision other)
    {

        switch (other.collider.tag)
        {
            case "Paddle":
                if ((int)currentOwner == 1 && !other.collider.GetComponentInParent<PaddleController>().Player1)
                {
                    SwitchCurrentOwner();
                }
                else if ((int)currentOwner == 2 && other.collider.GetComponentInParent<PaddleController>().Player1)
                {
                    SwitchCurrentOwner();
                }
                break;
            case "Brick":
                float damage = 1f;
                if(isBuffed){
                    damage = 2;
                }
                else{
                    damage = 1;
                }
                if(currentOwner == CurrentOwner.player1){
                    other.collider.GetComponent<BrickHealth>().TakeDamge(damage, true);
                }
                else{
                    other.collider.GetComponent<BrickHealth>().TakeDamge(damage, false);
                }
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "OutZone":
                ResetOwner();
                break;
        }
    }
    public void ResetOwner()
    {
        if (GetMainOwner() == 0)
        {
            return;
        }
        currentOwner = (CurrentOwner)((int)owner);
        if (owner == Owner.player1)
        {
            GetComponent<MeshRenderer>().material = player1Material;
        }
        else if (owner == Owner.player2)
        {
            GetComponent<MeshRenderer>().material = player2Material;
        }
    }
    public void SwitchCurrentOwner()
    {
        switch ((int)currentOwner)
        {
            case 1:
                currentOwner = CurrentOwner.player2;
                GetComponent<MeshRenderer>().material = player2Material;
                break;
            case 2:
                currentOwner = CurrentOwner.player1;
                GetComponent<MeshRenderer>().material = player1Material;
                break;
        }
    }

    public void SetCurrentOwner(bool player1)
    {
        if (player1)
        {
            currentOwner = CurrentOwner.player1;
            GetComponent<MeshRenderer>().material = player1Material;
        }
        else
        {
            currentOwner = CurrentOwner.player2;
            GetComponent<MeshRenderer>().material = player2Material;
        }
    }

    public void OnBallOut()
    {
        ResetOwner();
        switch (GetMainOwner())
        {
            case 0:
                break;

            case 1:
                GameManager.instance.DisablePowerUps(true);
                break;
            case 2:
                GameManager.instance.DisablePowerUps(false);
                break;
        }

    }

    public void BuffBallDamage(float duration){
        buffTime = duration;
        if(buffCoroutine == null){
            buffCoroutine = StartCoroutine(BuffDamage());
        }
    }

    public void ResetBuffDamage(){
        buffTime = 0;
    }

    IEnumerator BuffDamage(){
        isBuffed = true;
        fireParticles.Play();
        while(true){
            buffTime -= Time.fixedDeltaTime;
            if(buffTime <= 0){
                fireParticles.Stop();
                isBuffed = false;
                buffCoroutine = null;
                break;
            }
            else{

                yield return new WaitForFixedUpdate();
            }
        }
    }


}

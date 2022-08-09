using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Power
{
    longer,
    speed,
    sizeUp,
    catcher,
    split,
    fire


}
public class PowerUp : MonoBehaviour
{
    // Start is called before the first frame update
    public bool isStopped = true;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] GameObject collectable;
    [SerializeField] Transform actives;
    [SerializeField] Transform inactives;
    [SerializeField] Power power;
    [SerializeField] GameObject[] text = new GameObject [6];
    bool rightSided = true;
    Vector3 target = Vector3.zero;

    public bool RightSided { get => rightSided; set => rightSided = value; }

    void Start()
    {
        DisablePowerUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isStopped && !target.Equals(Vector3.zero))
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "OutZone":
                Debug.Log("Power up gone out");
                DisablePowerUp();
                break;
            case "Paddle":
                other.gameObject.GetComponentInParent<PowerUpManager>().ApplyPowerUp((int)power);
                DisablePowerUp();
                break;
        }
    }

    public void EnablePowerUp(Vector3 position, int powerUp, bool side)
    {
        RightSided = side;
        if(powerUp < 0 || powerUp > Power.GetNames(typeof(Power)).Length - 1){
            Debug.Log("power up index out of bounds");
            return;
        }
        if(powerUp == 4){ // remove later
            power = (Power) 5;
        }
        else{
            power = (Power)powerUp;
        }

        UpdateText((int)power);
        transform.SetParent(actives);
        if (RightSided)
        {
            target = new Vector3(100, 1.5f, position.z);
        }
        else
        {
            target = new Vector3(-100, 1.5f, position.z);
        }
        transform.position = new Vector3(position.x, 1.5f, position.z);
        collectable.SetActive(true);
        isStopped = false;

    }
    public void DisablePowerUp()
    {
        transform.SetParent(inactives);
        target = Vector3.zero;
        isStopped = true;
        collectable.SetActive(false);
        transform.position = Vector3.zero;
    }

    public void UpdateText(int index){
        for (int loop = 0; loop < text.Length; loop++){
            if(loop == index){
                text[loop].SetActive(true);
            }
            else{
                text[loop].SetActive(false);
            }
        }
    }

}

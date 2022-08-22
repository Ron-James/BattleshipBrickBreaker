using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum Power
{
    longer = 0,
    speed = 1,
    sizeUp = 2,
    catcher = 3,
    bomb = 4,
    fire = 5


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
    [SerializeField] TextMeshPro powerUpText;
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
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
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
        if (powerUp < 0 || powerUp > Power.GetNames(typeof(Power)).Length - 1)
        {
            Debug.Log("power up index out of bounds");
            return;
        }
        if (powerUp == 5)
        {
            power = (Power)4;
        }
        else
        {
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

    public void UpdateText(int index)
    {

        switch ((int)power)
        {
            default:
                powerUpText.text = "none";
                break;
            case 0:
                powerUpText.text = "Long Paddle";
                break;
            case 1:
                powerUpText.text = "Speed Up";
                break;
            case 2:
                powerUpText.text = "Large Ball";
                break;
            case 3:
                powerUpText.text = "Catcher";
                break;
            case 4:
                powerUpText.text = "Bomb";
                break;
            case 5:
                powerUpText.text = "Fire";
                break;
            case 6:
                powerUpText.text = "New";
                break;

        }
    }

}

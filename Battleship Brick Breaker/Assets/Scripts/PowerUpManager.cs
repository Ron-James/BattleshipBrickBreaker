using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject paddle;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject bigBoat;
    [SerializeField] GameObject regBoat;
    [SerializeField] BoxCollider coll;
    [SerializeField] GameObject slimeBall;
    public bool catcher;

    [Header("power Up numbers")]
    [SerializeField] float paddleLengthUpgrade = 1.25f;
    [SerializeField] float ballSpdIncrease = 1.35f;
    [SerializeField] float ballSizeUpgrade;
    [SerializeField] float colliderLargeSize = 1.67f;

    float paddleLength;
    PaddleSoundBox paddleSoundBox;
    // Start is called before the first frame update
    void Start()
    {
        paddleSoundBox = GetComponentInChildren<PaddleSoundBox>();
        paddleLength = coll.gameObject.transform.localScale.z;
        ResetPowerUp();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreasePaddleLength()
    {
        regBoat.SetActive(false);
        bigBoat.SetActive(true);
        coll.gameObject.transform.localScale = new Vector3(1, 1, colliderLargeSize);

    }

    public void ResetPaddleLength()
    {
        coll.gameObject.transform.localScale = new Vector3(1, 1, paddleLength);
        regBoat.SetActive(true);
        bigBoat.SetActive(false);

    }

    public void ApplyPowerUp(int powerUp)
    {
        switch (powerUp)
        {
            case 0: //longer paddle
                IncreasePaddleLength();
                paddleSoundBox.powerUpSound.PlayOnce();
                break;
            case 1://speed up ball
                paddleSoundBox.powerUpSound.PlayOnce();
                ball.GetComponent<BallPhysics>().IncreaseVelocity(ballSpdIncrease);
                break;
            case 2: //increase ball size
                paddleSoundBox.powerUpSound.PlayOnce();
                ball.GetComponent<BallPhysics>().IncreaseSize(ballSizeUpgrade);
                break;
            case 3: // catch ball;
                paddleSoundBox.catcherPowerup.PlayOnce();
                catcher = true;
                slimeBall.SetActive(true);
                break;
            case 4: //bomb
                paddleSoundBox.powerUpSound.PlayOnce();
                if(TutorialManager.instance.isTutorial){
                    TutorialManager.instance.bombPowerUp.OpenPrompt(GetComponent<PaddleController>().Player1);
                }
                GetComponent<BombLauncher>().GiveBomb();
                break;
            case 5: //increase ball damage
                //ball.GetComponent<BallPhysics>().FlameOn();
                break;
            default:
                return;
        }
    }

    public void ResetPowerUp()
    {
        ResetPaddleLength();
        ball.GetComponent<BallPhysics>().ResetSize();
        catcher = false;
        slimeBall.SetActive(false);
    }


}

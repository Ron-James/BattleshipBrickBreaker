using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject paddle;
    [SerializeField] GameObject ball;
    public bool catcher;

    [Header("power Up numbers")]
    [SerializeField] float paddleLengthUpgrade = 1.25f;
    [SerializeField] float ballSpdIncrease = 1.35f;
    [SerializeField] float ballSizeUpgrade;
    
    float paddleLength;
    // Start is called before the first frame update
    void Start()
    {
        paddleLength = paddle.transform.localScale.z;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void IncreasePaddleLength(float percent)
    {
        float newLength = percent * paddleLength;
        Vector3 paddleScale = new Vector3(paddle.transform.localScale.x, paddle.transform.localScale.y, newLength);
        paddle.transform.localScale = paddleScale;
    }

    public void ResetPaddleLength()
    {
        Vector3 paddleScale = new Vector3(paddle.transform.localScale.x, paddle.transform.localScale.y, paddleLength);
        paddle.transform.localScale = paddleScale;
    }

    public void ApplyPowerUp(int powerUp)
    {
        switch (powerUp)
        {
            case 0: //longer paddle
                IncreasePaddleLength(paddleLengthUpgrade);
                break;
            case 1://speed up ball
                ball.GetComponent<BallPhysics>().IncreaseVelocity(ballSpdIncrease);
                break;
            case 2: //increase ball size
                ball.GetComponent<BallPhysics>().IncreaseSize(ballSizeUpgrade);
                break;
            case 3: // catch ball;
                catcher = true;
                break;
            case 4: //split balls

                break;
            case 5: //increase ball damage
                ball.GetComponent<BallPhysics>().FlameOn();
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
        ball.GetComponent<BallPhysics>().FlameOff();
    }


}

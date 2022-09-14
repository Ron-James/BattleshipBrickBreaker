using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct MinMaxWeightPair{
    int min;
    int max;
}
public class PowerUpManager : MonoBehaviour
{
    [SerializeField] GameObject paddle;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject bigBoat;
    [SerializeField] GameObject regBoat;
    [SerializeField] BoxCollider coll;
    [SerializeField] GameObject slimeBall;
    [SerializeField] TextFlash tripleCannonText;
    public bool catcher;
    [Header("Triple Cannon Text Flash Max Time")]
    [SerializeField] float trippleCannonTextFlashTime = 2f;

    [Header("power Up numbers")]
    [SerializeField] float ballSpdIncrease = 1.35f;
    [SerializeField] float ballSizeUpgrade;
    [SerializeField] float colliderLargeSize = 1.5f;
    [SerializeField] bool[] currentPowerUps = new bool[6];
    [SerializeField] int [] powerUpWeights = new int[6];
    float paddleLength;
    PaddleSoundBox paddleSoundBox;
    bool player1;

    public bool[] CurrentPowerUps { get => currentPowerUps; set => currentPowerUps = value; }

    // Start is called before the first frame update
    void Start()
    {
        player1 = GetComponent<PaddleController>().Player1;
        paddleSoundBox = GetComponentInChildren<PaddleSoundBox>();
        paddleLength = coll.gameObject.transform.localScale.z;
        ResetPowerUp();
        if (currentPowerUps.Length != PowerUp.NumOfPowerUps)
        {
            Array.Resize(ref currentPowerUps, PowerUp.NumOfPowerUps);
            Array.Resize(ref powerUpWeights, PowerUp.NumOfPowerUps);
        }

        ResetCurrentPowerUps();
    }

    public List<MinMaxWeightPair> CalculateMinMaxPairs(){
        
        List<MinMaxWeightPair> pairs = new List<MinMaxWeightPair>(6);

        return pairs;
    }
    public void ResetCurrentPowerUps()
    {
        for (int loop = 0; loop < currentPowerUps.Length; loop++)
        {
            currentPowerUps[loop] = false;
        }
    }

    public int NumberOfOpenPowerUps()
    {
        int num = 0;
        for (int loop = 0; loop < currentPowerUps.Length; loop++)
        {
            if (currentPowerUps[loop] == false)
            {
                num++;
            }
            else
            {
                continue;
            }
        }
        return num;
    }
    public void ResetBombPowerUp(){
        currentPowerUps[4] = false;
    }
    
    public int IndexOfOpenPowerUp(int order)
    {
        if (order > NumberOfOpenPowerUps() || order > currentPowerUps.Length)
        {
            return -1;
        }
        int num = 0;
        int count = 0;
        for (int loop = 0; loop < currentPowerUps.Length; loop++)
        {
            if (currentPowerUps[loop] == false)
            {
                num = loop;
                count++;
                if (count >= order)
                {
                    return num;
                }
                else
                {
                    continue;
                }
            }
            else
            {
                continue;
            }
        }
        return -1;
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
        GetComponent<PaddleButtonController>().CalculateMaximumValues();

    }

    public void ResetPaddleLength()
    {
        coll.gameObject.transform.localScale = new Vector3(1, 1, paddleLength);
        regBoat.SetActive(true);
        bigBoat.SetActive(false);
        GetComponent<PaddleButtonController>().CalculateMaximumValues();

    }
    public bool IsTripleCannon(){
        return currentPowerUps[6];
    }
    public void ApplyPowerUp(int powerUp)
    {
        if (powerUp > currentPowerUps.Length)
        {
            Debug.Log("Current powerups out of bounds");
            return;
        }
        switch (powerUp)
        {
            case 0: //longer paddle
                IncreasePaddleLength();
                paddleSoundBox.powerUpSound.PlayOnce();
                currentPowerUps[powerUp] = true;
                break;
            case 1://speed up ball
                paddleSoundBox.powerUpSound.PlayOnce();
                ball.GetComponent<BallPhysics>().IncreaseVelocity(ballSpdIncrease);
                break;
            case 2: //increase ball size
                paddleSoundBox.powerUpSound.PlayOnce();
                ball.GetComponent<BallPhysics>().IncreaseSize(ballSizeUpgrade);
                currentPowerUps[powerUp] = true;
                break;
            case 3: // catch ball;
                paddleSoundBox.powerUpSound.PlayOnce();
                GameManager.instance.ControlBarriers(player1, true);
                currentPowerUps[powerUp] = true;
                break;
            case 4: //bomb
                paddleSoundBox.powerUpSound.PlayOnce();
                
                GetComponent<BombLauncher>().LoadBomb();
                currentPowerUps[powerUp] = true;
                break;
            case 5: //split
                ball.GetComponent<BallSplitter>().SplitBall(1);
                if (GameManager.NumberOfExtraBalls() > GameManager.instance.MaxExtraBalls)
                {
                    currentPowerUps[powerUp] = true;
                }
                break;
            case 6: // triple cannon
                tripleCannonText.FlashText(trippleCannonTextFlashTime);
                currentPowerUps[powerUp] = true;
                break;
            default:
                return;
        }
    }

    public void ResetPowerUp()
    {
        ResetCurrentPowerUps();
        ResetPaddleLength();
        ball.GetComponent<BallPhysics>().ResetSize();
        catcher = false;
        slimeBall.SetActive(false);
        GameManager.instance.ControlBarriers(player1, false);
        tripleCannonText.FlashText(0);
    }

    public void OnBallOut()
    {
        ResetPowerUp();
    }

    public void OpenBombSlot(){
        currentPowerUps[4] = false;
    }


}

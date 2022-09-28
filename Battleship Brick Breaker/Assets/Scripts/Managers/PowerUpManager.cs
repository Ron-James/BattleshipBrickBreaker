using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public struct MinMaxWeightPair{
    public int min;
    public int max;

    public MinMaxWeightPair(int min, int max)
    {
        this.min = min;
        this.max = max;
    }
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
    Artillery artillery;
    bool player1;

    public bool[] CurrentPowerUps { get => currentPowerUps; set => currentPowerUps = value; }

    // Start is called before the first frame update
    void Start()
    {
        artillery = GetComponent<Artillery>();
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

    public List<MinMaxWeightPair> CalculateMinMaxPairs(out int totalWeight){
        List<MinMaxWeightPair> pairs = new List<MinMaxWeightPair>(6);
        totalWeight = 0;
        if(NumberOfOpenPowerUps() == 0){
            return pairs;
        }
        else{
            int firstMax = powerUpWeights[IndexOfOpenPowerUp(1)];
            MinMaxWeightPair first = new MinMaxWeightPair(0, firstMax - 1);
            pairs.Add(first);
            totalWeight += first.max;

            int openPowerUpsTotal = NumberOfOpenPowerUps();
            if(openPowerUpsTotal == 1){
                return pairs;
            }
            else{
                int prevMax = first.max;
                for(int loop = 2; loop <= openPowerUpsTotal; loop++){
                    int currentMin = prevMax + 1;
                    int currentWeight = powerUpWeights[IndexOfOpenPowerUp(loop)];
                    int currentMax = currentMin + (currentWeight - 1);
                    totalWeight += currentMax;
                    prevMax = currentMax;
                    MinMaxWeightPair current = new MinMaxWeightPair(currentMin, currentMax);
                    pairs.Add(current);
                }
            }
            

        }
        
        
        return pairs;
    }
    public int GetTotalWeight(){
        int total = 0;
        for(int loop = 0; loop < currentPowerUps.Length; loop++){
            if(currentPowerUps[loop] == false){
                total += powerUpWeights[loop];
            }
            else{
                continue;
            }
        }
        return total;
    }
    public int CalculatePowerUpDrop(int random){
        int total;
        List<MinMaxWeightPair> pairs = CalculateMinMaxPairs(out total);
        if(pairs.Count == 0 || random > total || random < 0){
            return -1;
        }
        else if(pairs.Count == 1){
            return 0;
        }
        else{
            for(int loop = 0; loop < pairs.Count; loop++){
                if(random >= pairs[loop].min && random <= pairs[loop].max){
                    return IndexOfOpenPowerUp(loop + 1);
                }
                else{
                    continue;
                }
            }

            return 0;
        }
    }

    public int FirstOpenPowerUp(){
        for (int loop = 0; loop < currentPowerUps.Length; loop++)
        {
            if(currentPowerUps[loop] == false){
                return loop;
            }
            else{
                continue;
            }
        }

        return -1;
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
                GameObject cannonBtn = SettingsManager.instance.ActiveCannonButton(player1);
                cannonBtn.GetComponent<CannonButton>().TripleCannonIndicator.SetActive(true);
                tripleCannonText.FlashText();
                currentPowerUps[powerUp] = true;
                artillery.Fire();
                break;
            
            case 7:
                GetComponent<Artillery>().AddSnowThrowerTime(100000);
                currentPowerUps[powerUp] = true;
            break;
            default:
                return;
        }
    }

    public void ResetPowerUp()
    {
        GameObject cannonBtn = SettingsManager.instance.ActiveCannonButton(player1);
        cannonBtn.GetComponent<CannonButton>().TripleCannonIndicator.SetActive(false);
        ResetCurrentPowerUps();
        ResetPaddleLength();
        ball.GetComponent<BallPhysics>().ResetSize();
        catcher = false;
        slimeBall.SetActive(false);
        GameManager.instance.ControlBarriers(player1, false);
        tripleCannonText.StopTextFlash();
        GetComponent<Artillery>().StopSnowThrower();
        GameManager.instance.GetPaddle(player1).Ball.GetComponent<PlayerTracker>().ResetBuffDamage();
        if(TutorialManager.instance.isTutorial){
            TutorialManager.instance.bombPowerUp.ClosePrompt(player1);
        }
    }

    public void OnBallOut()
    {
        ResetPowerUp();
    }

    public void OpenBombSlot(){
        currentPowerUps[4] = false;
    }

    public bool IsLarge(){
        return currentPowerUps[0];
    }


}

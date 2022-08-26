using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] int p1Score = 0;
    [SerializeField] int p2Score = 0;
    [SerializeField] int objScore = 10;
    [SerializeField] bool localMultiplayer = false;
    [SerializeField] GameObject p1Indicators;
    [SerializeField] GameObject p2Indicators;
    [SerializeField] GameObject inactivePowerups;
    [SerializeField] GameObject activePowerups;
     

    [SerializeField] WinScreen winScreen;
    [SerializeField] TextMeshProUGUI countDownTime;

    [Header("Objective Indicators")]
    public ObjBrickIndicator[] p1ObjIndicators;
    public ObjBrickIndicator[] p2ObjIndicators;

    [Header("paddles")]
    public GameObject paddle1;
    public GameObject paddle2;

    [Header("Game Constants")]
    [SerializeField] int powerUpDropRate = 5;
    [SerializeField] float doubleTapTime = 0.5f;
    [SerializeField] float maxTapDistance = 10;
    [SerializeField] float cannonballHitPenalty = 8f;
    [SerializeField] float bombHitPenalty = 7f;
    [SerializeField] float minBombLaunchDistance = 18f;
    [SerializeField] float bombPowerUpTime = 5f;
    [SerializeField] float bombCountDownTime = 7f;
    [SerializeField] float initialVelocity = 21f;
    [SerializeField] float maxBallVelocity = 45f;
    [SerializeField] float randomReturnDistance = 40f;
    [SerializeField] float initialOutPenalty;
    [SerializeField] float outPenaltyIncrease;
    [SerializeField] float maxExtraBalls;

    [SerializeField] float outPenalty = 3f;
    public static bool gameOver;





    public int P2Score { get => p2Score; set => p2Score = value; }
    public bool LocalMultiplayer { get => localMultiplayer; set => localMultiplayer = value; }

    public float InitialVelocity { get => initialVelocity; set => initialVelocity = value; }
    public int PowerUpDropRate { get => powerUpDropRate; set => powerUpDropRate = value; }
    public float DoubleTapTime { get => doubleTapTime; set => doubleTapTime = value; }
    public float MaxTapDistance { get => maxTapDistance; set => maxTapDistance = value; }
    public float CannonballHitPenalty { get => cannonballHitPenalty; set => cannonballHitPenalty = value; }
    public float BombHitPenalty { get => bombHitPenalty; set => bombHitPenalty = value; }
    public float MinBombLaunchDistance { get => minBombLaunchDistance; set => minBombLaunchDistance = value; }
    public float BombPowerUpTime { get => bombPowerUpTime; set => bombPowerUpTime = value; }
    public float BombCountDownTime { get => bombCountDownTime; set => bombCountDownTime = value; }
    public float MaxBallVelocity { get => maxBallVelocity; set => maxBallVelocity = value; }
    public float RandomReturnDistance { get => randomReturnDistance; set => randomReturnDistance = value; }
    public float InitialOutPenalty { get => initialOutPenalty; set => initialOutPenalty = value; }
    public float OutPenaltyIncrease { get => outPenaltyIncrease; set => outPenaltyIncrease = value; }
    public float MaxExtraBalls { get => maxExtraBalls; set => maxExtraBalls = value; }


    // Start is called before the first frame update
    void Start()
    {
        TurnOffAllObjIndicators();
        p1ObjIndicators = p1Indicators.GetComponentsInChildren<ObjBrickIndicator>();
        p2ObjIndicators = p2Indicators.GetComponentsInChildren<ObjBrickIndicator>();
        StartCoroutine(StartSequence());

    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.K))
        {
            SpawnPowerup(Vector3.zero, true, 5);
        }
    }

    public void ChangeAllBalls(bool player1){
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        foreach(GameObject ball in balls){
            if(ball.transform.parent == null){
                ball.GetComponent<PlayerTracker>().SetCurrentOwner(player1);
            }
            
        }
    }
    public int RandomOpenPowerUp(PowerUpManager powerUpManager){
        int length = powerUpManager.NumberOfOpenPowerUps();
        int random = Random.Range(1, length + 1);

        int index = powerUpManager.IndexOfOpenPowerUp(random);
        return index;

    }
    public bool TouchInField(out int index)
    {
        index = -1;
        if (Input.touches.Length == 0)
        {
            return false;
        }
        else
        {
            for (int loop = 0; loop < Input.touchCount; loop++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[loop].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    index = loop;
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }
    public bool TouchInField(out int index, out Vector3 position)
    {
        index = -1;
        position = Vector3.zero;
        if (Input.touches.Length == 0)
        {
            return false;
        }
        else
        {
            for (int loop = 0; loop < Input.touchCount; loop++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[loop].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    position = hit.point;
                    index = loop;
                    return true;
                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }

    public static int NumberOfExtraBalls(){
        GameObject[] balls = GameObject.FindGameObjectsWithTag("Ball");
        int num = 0;
        foreach(GameObject ball in balls){
            if(ball.transform.parent == null){
                if(ball.GetComponent<ExtraBall>() != null){
                    num++;
                }
                else{
                    continue;
                }
            }
            else{
                continue;
            }
        }
        return num;
    }
    public bool TouchInField(out int index, out Vector3 position, bool player1)
    {
        index = -1;
        position = Vector3.zero;
        if (Input.touches.Length == 0)
        {
            return false;
        }
        else
        {
            for (int loop = 0; loop < Input.touchCount; loop++)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.touches[loop].position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (player1)
                    {
                        if (paddle1.transform.position.x > hit.point.x && hit.point.x > (paddle1.transform.position.x - maxTapDistance))
                        {
                            //Debug.Log("success");
                            position = hit.point;
                            index = loop;
                            return true;
                        }
                    }
                    else
                    {
                        if (hit.point.x > paddle2.transform.position.x && hit.point.x < (paddle2.transform.position.x + maxTapDistance))
                        {
                            position = hit.point;
                            index = loop;
                            return true;
                        }
                    }

                }
                else
                {
                    continue;
                }
            }
            return false;
        }
    }
    public PaddleController GetPaddle(bool player1){
        if(player1){
            return paddle1.GetComponent<PaddleController>();
        }
        else{
            return paddle2.GetComponent<PaddleController>();
        }
    }
    public void ApplyForceToVelocity(Rigidbody rigidbody, Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        //Debug.Log(velocity.magnitude + " Increase to velocity");
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedtoTarget = (velocity.normalized * Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedtoTarget) * force, mode);
        }

    }
    public void UpdateObjIndicators(int player)
    {
        switch (player)
        {
            case 1:
                if (p1Score == 0)
                {
                    return;
                }
                else
                {
                    for (int loop = 0; loop < p1Score; loop++)
                    {
                        p1ObjIndicators[loop].TurnOn();
                    }
                }
                break;
            case 2:
                if (p2Score == 0)
                {
                    return;
                }
                else
                {
                    for (int loop = 0; loop < p2Score; loop++)
                    {
                        p2ObjIndicators[loop].TurnOn();
                    }
                }
                break;
            default:
                Debug.Log("player index out of bounds");
                return;
        }
    }
    public void TurnOffAllObjIndicators()
    {
        if (p1ObjIndicators.Length != p2ObjIndicators.Length)
        {
            Debug.Log("uneven number of indicators");
            return;
        }
        for (int loop = 0; loop < p1ObjIndicators.Length; loop++)
        {
            p1ObjIndicators[loop].TurnOff();
            p2ObjIndicators[loop].TurnOff();
        }
    }
    public void AddScore(int player)
    {
        switch (player)
        {
            case 1:
                p1Score++;
                UpdateObjIndicators(player);
                if (p1Score >= objScore)
                {
                    gameOver = true;
                    winScreen.Open(true);
                }
                break;
            case 2:
                p2Score++;
                UpdateObjIndicators(player);
                if (p2Score >= objScore)
                {
                    gameOver = true;
                    winScreen.Open(false);
                }
                break;

            default:
                return;
        }
    }

    public void SpawnPowerup(Vector3 position, bool player1)
    {
        int rand = 0;
        if(player1){
            rand = RandomOpenPowerUp(paddle1.GetComponent<PowerUpManager>());
        }
        else{
            rand = RandomOpenPowerUp(paddle2.GetComponent<PowerUpManager>());
        }
        inactivePowerups.GetComponentsInChildren<PowerUp>()[0].EnablePowerUp(position, rand, player1);
    }
    
    public void SpawnPowerup(Vector3 position, bool side, int type)
    {
        if(type > PowerUp.NumOfPowerUps){
            type = PowerUp.NumOfPowerUps;
        }
        inactivePowerups.GetComponentsInChildren<PowerUp>()[0].EnablePowerUp(position, type, side);
    }
    public Vector3 RotateVector(Vector3 input, float angle)
    {
        Vector3 newVector = Quaternion.AngleAxis(angle, Vector3.up) * input;
        return newVector;
    }
    public void DisablePowerUps(bool right)
    {
        PowerUp[] powers = activePowerups.GetComponentsInChildren<PowerUp>();
        for (int loop = 0; loop < powers.Length; loop++)
        {
            if (powers[loop].RightSided == right)
            {
                powers[loop].DisablePowerUp();
            }
        }
    }
    [SerializeField] Sound countDownSound;
    IEnumerator StartSequence(){
        countDownTime.gameObject.SetActive(true);
        
        int time = 3;
        countDownTime.text = time.ToString();
        PauseManager.PauseGameplay();
        countDownSound.PlayOnce();
        yield return new WaitForSecondsRealtime(1);
        for (int loop = 0; loop <= 3; loop++){
            if(time == 0){
                countDownTime.text = "Battle!";
                PauseManager.ResumeGameplay();
                
            }
            else{
                countDownTime.text = time.ToString();
                countDownSound.PlayOnce();
            }
            yield return new WaitForSecondsRealtime(1);
            time--;
            
        }
        countDownTime.gameObject.SetActive(false);
    }
}

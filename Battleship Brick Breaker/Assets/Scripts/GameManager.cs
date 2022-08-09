using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    int p1Score = 0;
    int p2Score = 0;
    int objScore = 10;
    [SerializeField] bool localMultiplayer = false;
    [SerializeField] GameObject p1Indicators;
    [SerializeField] GameObject p2Indicators;
    [SerializeField] GameObject inactivePowerups;
    [SerializeField] GameObject activePowerups;

    [SerializeField] WinScreen winScreen;

    [Header("Objective Indicators")]
    public ObjBrickIndicator[] p1ObjIndicators;
    public ObjBrickIndicator[] p2ObjIndicators;

    [Header("paddles")]
    public GameObject paddle1;
    public GameObject paddle2;

    bool gameOver;

    float timeElapsed = 0;
    int [] bricksBroken = new int [2];
    int [] objBroken = new int [2];
    int[] missed = new int[2];
    int[] shotsHit = new int[2];



    public int P2Score { get => p2Score; set => p2Score = value; }
    public bool LocalMultiplayer { get => localMultiplayer; set => localMultiplayer = value; }
    public float TimeElapsed { get => timeElapsed; set => timeElapsed = value; }
    public int[] BricksBroken { get => bricksBroken; set => bricksBroken = value; }
    public int[] ObjBroken { get => objBroken; set => objBroken = value; }
    public int[] Missed { get => missed; set => missed = value; }
    public int[] ShotsHit { get => shotsHit; set => shotsHit = value; }

    // Start is called before the first frame update
    void Start()
    {
        timeElapsed = 0;
        TurnOffAllObjIndicators();
        p1ObjIndicators = p1Indicators.GetComponentsInChildren<ObjBrickIndicator>();
        p2ObjIndicators = p2Indicators.GetComponentsInChildren<ObjBrickIndicator>();

    }

    // Update is called once per frame
    void Update()
    {
        if(!gameOver){
            timeElapsed += Time.deltaTime;
        }
        if(Input.GetKeyDown(KeyCode.K)){
            timeElapsed = 100;
            AddScore(1);
        }
    }
    public bool TouchInField(out int index){
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
    public bool TouchInField(out int index, out Vector3 position){
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

    public void SpawnPowerup(Vector3 position, bool side){
        int rand = Random.Range(0, Power.GetNames(typeof(Power)).Length);
        inactivePowerups.GetComponentsInChildren<PowerUp>()[0].EnablePowerUp(position, rand, side);
    }
    public void DisablePowerUps(bool right){
        PowerUp[] powers = activePowerups.GetComponentsInChildren<PowerUp>();
        for (int loop = 0; loop < powers.Length; loop++){
            if(powers[loop].RightSided == right){
                powers[loop].DisablePowerUp();
            }
        }
    }
}

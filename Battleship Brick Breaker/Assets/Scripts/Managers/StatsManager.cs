using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : Singleton<StatsManager>
{
    public float timeElapsed = 0;
    public int [] bricksBroken = new int [2];
    public int [] objBroken = new int [2];
    public int[] missed = new int[2];
    public int[] shotsHit = new int[2];
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.gameOver){
            timeElapsed += Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBallManager : Singleton<ExtraBallManager>
{
    [SerializeField] GameObject actives;
    [SerializeField] GameObject inactives;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnExtraBall(Vector3 position, float velocity, bool player1){
        ExtraBall ball = inactives.GetComponentsInChildren<ExtraBall>()[0];
        ball.EnableBall(player1, position, velocity);
    }
}

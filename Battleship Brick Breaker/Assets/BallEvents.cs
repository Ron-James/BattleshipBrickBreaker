using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onBallOut;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        switch(other.tag){
            case "OutZone":
                OutZone outZone = other.gameObject.GetComponent<OutZone>();
                if(GetComponent<BallPhysics>().RightSide){
                    if(outZone.PlayerSide == OutZone.Player.player1){
                        onBallOut.Invoke();
                    }
                }
                else{
                    if(outZone.PlayerSide == OutZone.Player.player2){
                        onBallOut.Invoke();
                    }
                }
                break;
        }
    }
}

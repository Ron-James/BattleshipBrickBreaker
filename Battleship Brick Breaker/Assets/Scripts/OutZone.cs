using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutZone : MonoBehaviour
{
    public enum Player
    {
        player1 = 1,
        player2 = 2
    }
    [SerializeField] PaddleController paddle1;
    [SerializeField] PaddleController paddle2;
    [SerializeField] Player player;
    [SerializeField] Transform returnPoint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other) {
        BallPhysics ball = other.gameObject.GetComponent<BallPhysics>();

        if(ball != null){
            PlayerTracker playerTracker = ball.gameObject.GetComponent<PlayerTracker>();
            if(player == Player.player1){
                if(playerTracker.GetMaintOwner() == 1){
                    paddle1.GoneOut();
                }
                else{
                    ball.StartRandomReturn(ball.GetComponent<CollisionVelocityControl>().LargestMagnitude, returnPoint);
                }
            }
            else{
                if(playerTracker.GetMaintOwner() == 2){
                    paddle2.GoneOut();
                }
                else{
                    ball.StartRandomReturn(ball.GetComponent<CollisionVelocityControl>().LargestMagnitude, returnPoint);

                }
            }
        }
    }
}

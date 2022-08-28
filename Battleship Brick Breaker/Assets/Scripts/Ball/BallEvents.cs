using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BallEvents : MonoBehaviour
{
    [SerializeField] private UnityEvent onBallOut;
    BallPhysics ballPhysics;
    PlayerTracker playerTracker;

    public UnityEvent OnBallOut { get => onBallOut; set => onBallOut = value; }

    // Start is called before the first frame update
    void Start()
    {
        ballPhysics = GetComponent<BallPhysics>();
        playerTracker = GetComponent<PlayerTracker>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {

        switch (other.tag)
        {
            case "OutZone":
                OutZone outZone = other.gameObject.GetComponent<OutZone>();
                if (playerTracker.GetMainOwner() == 0)
                {

                    Debug.Log("No main owner");
                    switch (playerTracker.GetCurrentOwner())
                    {
                        case 1:
                            if (outZone.PlayerSide == OutZone.Player.player1)
                            {
                                Debug.Log("On ball out");
                                onBallOut.Invoke();
                            }
                            else
                            {
                                Debug.Log("Should random return");
                                ballPhysics.StartRandomReturn(GetComponent<CollisionVelocityControl>().LargestMagnitude, outZone.ReturnPoint);
                            }
                            break;
                        case 2:
                            if (outZone.PlayerSide == OutZone.Player.player2)
                            {
                                Debug.Log("On ball out");
                                onBallOut.Invoke();
                            }
                            else
                            {
                                Debug.Log("Should random return");
                                ballPhysics.StartRandomReturn(GetComponent<CollisionVelocityControl>().LargestMagnitude, outZone.ReturnPoint);
                            }
                            break;
                    }
                }
                else
                {
                    switch (playerTracker.GetMainOwner())
                    {
                        case 1:
                            if (outZone.PlayerSide == OutZone.Player.player1)
                            {
                                onBallOut.Invoke();
                            }
                            else
                            {
                                ballPhysics.StartRandomReturn(GetComponent<CollisionVelocityControl>().LargestMagnitude, outZone.ReturnPoint);
                            }
                            break;
                        case 2:
                            if (outZone.PlayerSide == OutZone.Player.player2)
                            {
                                onBallOut.Invoke();
                            }
                            else
                            {
                                ballPhysics.StartRandomReturn(GetComponent<CollisionVelocityControl>().LargestMagnitude, outZone.ReturnPoint);
                            }
                            break;

                    }
                }
                break;
        }
    }
}

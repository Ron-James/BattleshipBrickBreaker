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

    public Player PlayerSide { get => player; set => player = value; }
    public Transform ReturnPoint { get => returnPoint; set => returnPoint = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}

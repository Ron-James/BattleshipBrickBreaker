using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicBox : MonoBehaviour
{
    [SerializeField] Sound musicTrack;
    // Start is called before the first frame update
    void Start()
    {
        musicTrack.src = GetComponent<AudioSource>();
        musicTrack.PlayLoop();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonButton : MonoBehaviour
{
    [SerializeField] GameObject tripleCannonIndicator;

    public GameObject TripleCannonIndicator { get => tripleCannonIndicator; set => tripleCannonIndicator = value; }

    // Start is called before the first frame update
    void Start()
    {
        tripleCannonIndicator.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

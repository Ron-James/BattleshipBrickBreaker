using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wedge : MonoBehaviour
{
    public Transform face;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        switch(other.collider.tag){
            case "":
        
                break;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(face.transform.position, face.transform.position + (face.up * 1));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = target.GetComponent<BoxCollider>().bounds.size.x / target.GetComponent<BoxCollider>().bounds.size.z;

        if(screenRatio >= targetRatio){
            GetComponent<Camera>().orthographicSize = target.GetComponent<BoxCollider>().bounds.size.z / 2;
        }
        else{
            float diffInSize = targetRatio / screenRatio;
            GetComponent<Camera>().orthographicSize = (target.GetComponent<BoxCollider>().bounds.size.z / 2) * diffInSize;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

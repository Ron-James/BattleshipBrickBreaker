using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode()]
public class GridObject : MonoBehaviour
{
    [SerializeField] GameObject target;
    [SerializeField] GameObject structure;
    Vector3 position;
    [SerializeField] float gridSize;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        position.x = Mathf.Floor(target.transform.position.x / gridSize) * gridSize;
        position.y = Mathf.Floor(target.transform.position.y / gridSize) * gridSize;
        position.z = Mathf.Floor(target.transform.position.z / gridSize) * gridSize;
        structure.transform.position = position;
    }
}

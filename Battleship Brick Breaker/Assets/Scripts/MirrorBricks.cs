using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorBricks : MonoBehaviour
{
    [SerializeField] GameObject bricksP1;
    BrickHealth[] bricks;
    // Start is called before the first frame update
    void Start()
    {
        GameObject p2Bricks = Instantiate(bricksP1, bricksP1.transform.position, Quaternion.identity);
        p2Bricks.transform.SetParent(this.transform);
        p2Bricks.name = "P2 Bricks";
        bricks = p2Bricks.GetComponentsInChildren<BrickHealth>();
        Mirror();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Mirror()
    {
        for (int loop = 0; loop < bricks.Length; loop++)
        {
            bricks[loop].MirrorBrick();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterRotate : MonoBehaviour
{
    [SerializeField] Material [] materials;
    [SerializeField] float frequency = 0.5f;

    MeshRenderer meshRenderer;
    int currentMat = 0;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        currentMat = 0;
        StartCoroutine(RotateMaterial());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void NextMat(){
        if(currentMat ==  materials.Length - 1){
            currentMat = 0;
        }
        else{
            currentMat++;
        }
        meshRenderer.material = materials[currentMat];
        
    }
    IEnumerator RotateMaterial(){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            if(time >= frequency){
                time = 0;
                NextMat();
            }
            else{
                yield return null;
            }
            
        }
    }
}

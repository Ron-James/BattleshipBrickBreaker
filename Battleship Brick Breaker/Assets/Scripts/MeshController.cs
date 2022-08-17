using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    [SerializeField] MeshRenderer meshRenderer;
    Material material;
    float maxAlpha;
    // Start is called before the first frame update
    void Start()
    {
        material = meshRenderer.material;
        maxAlpha = material.color.a;
        ChangeAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision other) {
        if(other.collider.tag == "Ball"){
            StartCoroutine(FlashMeshRenderer(0.2f));
        }
    }
    private void OnCollisionExit(Collision other) {
        
    }

    public void ChangeAlpha(float alpha){
        Color color = material.color;
        color.a = alpha;
        material.SetColor("color", color);
    }
    IEnumerator FlashMeshRenderer(float duration){
        ChangeAlpha(maxAlpha);
        float rate = maxAlpha / (duration / Time.deltaTime);
        float alpha = maxAlpha;
        while(true){
            if(alpha <= 0){
                ChangeAlpha(0);
                break;
            }
            else{
                alpha -= rate;
                ChangeAlpha(alpha);
                yield return null;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPrompt : MonoBehaviour
{
    [SerializeField] Image prompt;
    [SerializeField] bool isStopped;
    [SerializeField] float flashPeriod;
    [SerializeField] float maxAlpha;
    [SerializeField] Coroutine fadeFlash;
    public bool IsStopped { get => isStopped; set => isStopped = value; }

    // Start is called before the first frame update
    void Start()
    {
        fadeFlash = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStopped){

        }
    }
    public void StopFadeFlash(){
        isStopped = true;
    }
    public void StartFadeFlash(){
        
        isStopped = false;
        if(fadeFlash == null){
            fadeFlash = StartCoroutine(FadeFlash(flashPeriod, maxAlpha));
            
        }
        
    }
    private void OnDisable(){
        StopAllCoroutines();
    }
    IEnumerator FadeFlash(float period, float maxAlpha){
        Debug.Log("Start Fade Flash");
        prompt.gameObject.SetActive(true);
        Color defaultColor = prompt.color;
        Color color = prompt.color;
        float time = 0;
        float w = (1 / period) * 2 * Mathf.PI;
        while(true){
            if(isStopped){
                Debug.Log("stopping here");
                prompt.color = defaultColor;
                prompt.gameObject.SetActive(false);
                fadeFlash = null;
                break;
            }
            else{
                float alpha = maxAlpha * Mathf.Abs(Mathf.Sin(w * time));
                time += Time.deltaTime;
                color.a = alpha;
                prompt.color = color;
                yield return null;
            }
        }
    }
}

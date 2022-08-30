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
        isStopped = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isStopped){

        }
    }

    public void SwitchButtonPrompt(bool stopped){
        isStopped = stopped;
        if(!isStopped){
            if(fadeFlash == null){
                fadeFlash = StartCoroutine(FadeFlash(flashPeriod, maxAlpha));
            }
            
        }
        
    }
    IEnumerator FadeFlash(float period, float maxAlpha){
        prompt.gameObject.SetActive(true);
        Color defaultColor = prompt.color;
        Color color = prompt.color;
        float time = 0;
        float w = (1 / period) * 2 * Mathf.PI;
        while(true){
            if(isStopped){
                Debug.Log("Should Stop here");
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

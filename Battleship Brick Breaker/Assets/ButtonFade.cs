using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ButtonFade : MonoBehaviour
{
    Image image;
    TextMeshProUGUI text;

    [SerializeField] AnimationCurve imageFadeIn, textFadeIn;
    [SerializeField] AnimationCurve imageFadeOut, textFadeOut;
    [SerializeField] float maxImageAlpha = 0.3f, maxTextAlpha = 0.5f;
    [SerializeField] float fadeInTime = 0.4f, fadeOutTime = 0.4f;

    Coroutine fadeInRoutine, fadeOutRoutine;

    bool buttonDown;

    public void SetButtonDown(bool down){
        buttonDown = down;
    }
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        ChangeImageAlpha(0);
        ChangeTextAlpha(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartFadeIn(){
        if(fadeInRoutine == null){
            fadeInRoutine = StartCoroutine(FadeIn(fadeInTime));
        }
        else{
            return;
        }
    }

    public void StartFadeOut(){
        if(fadeOutRoutine == null){
            fadeOutRoutine = StartCoroutine(FadeOut(fadeOutTime));
        }
        else{
            return;
        }
    }
    IEnumerator FadeIn(float duration){
        float time = 0;
        while(true){
            time += Time.deltaTime;
            float ratio = time / duration;
            if(time >= duration){
                ChangeImageAlpha(maxImageAlpha);
                ChangeTextAlpha(maxTextAlpha);
                fadeInRoutine = null;
                break;
            }
            else if(!buttonDown){

                fadeInRoutine = null;
                break;
            }
            else{
                Color imageColor = image.color;
                imageColor.a = imageFadeIn.Evaluate(ratio) * maxImageAlpha;
                image.color = imageColor;
                Color textColor = text.color;
                textColor.a = textFadeIn.Evaluate(ratio) * maxTextAlpha;
                text.color = textColor;
                yield return null;
            }
        }
    }

    public void ChangeImageAlpha(float alpha){
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void ChangeTextAlpha(float alpha){
        Color color = text.color;
        color.a = alpha;
        text.color = color;
    }
    IEnumerator FadeOut(float duration){
        float time = duration;

        while(true){
            time -= Time.deltaTime;
            float ratio = time/duration;
            if(time <= 0){
                ChangeImageAlpha(0);
                ChangeTextAlpha(0);
                fadeOutRoutine = null;
                break;
            }
            else if(buttonDown){

                fadeOutRoutine = null;
                break;
            }
            else{
                Color imageColor = image.color;
                imageColor.a = imageFadeOut.Evaluate(ratio) * maxImageAlpha;
                image.color = imageColor;
                Color textColor = text.color;
                textColor.a = textFadeOut.Evaluate(ratio) * maxTextAlpha;
                text.color = textColor;
                yield return null;
            }
        }
    }
}

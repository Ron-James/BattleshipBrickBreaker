using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlash : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] Color flashColor;
    [SerializeField] float flashDuration;
    [SerializeField] float flashPeriod;
    [SerializeField] Sound flashSound;
    Coroutine flashCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FlashText(){
        if(flashCoroutine == null){
            flashCoroutine = StartCoroutine(Flash(flashDuration, flashPeriod, flashColor));
        }
        else{
            return;
        }
    }
    IEnumerator Flash(float duration, float period, Color color)
    {
        flashSound.PlayLoop();
        text.gameObject.SetActive(true);
        float time = 0;
        float flashTime = 0;
        int count = 0;
        Color defaultColor = text.color;
        while (true)
        {
            if (time >= duration)
            {
                flashCoroutine = null;
                text.color = defaultColor;
                text.gameObject.SetActive(false);
                flashSound.StopSource();
                break;
            }
            else
            {
                time += Time.deltaTime;
                flashTime += Time.deltaTime;
                if (flashTime >= period)
                {
                    count++;
                    flashTime = 0;
                }
                if (count % 2 == 0)
                {
                    text.color = defaultColor;
                }
                else
                {
                    text.color = color;
                }
                yield return null;
            }
        }
    }
}

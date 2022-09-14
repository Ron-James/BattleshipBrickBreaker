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
    [SerializeField] float currentFlashTime = 0;
    Coroutine flashCoroutine;
    // Start is called before the first frame update
    void Start()
    {
        text.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FlashText(float duration)
    {
        float time = Mathf.Abs(duration);
        if (time > 0)
        {
            if (flashCoroutine == null)
            {
                flashCoroutine = StartCoroutine(Flash(time, flashPeriod, flashColor));
            }
            else
            {
                return;
            }
        }
        else{
            currentFlashTime = 0;
        }




    }
    IEnumerator Flash(float duration, float period, Color color)
    {
        flashSound.PlayLoop();
        text.enabled = true;
        currentFlashTime = duration;
        float flashTime = 0;
        int count = 0;
        Color defaultColor = text.color;
        while (true)
        {
            if (currentFlashTime <= 0)
            {
                flashCoroutine = null;
                text.color = defaultColor;
                text.enabled = false;
                flashSound.StopSource();
                break;
            }
            else
            {
                currentFlashTime -= Time.deltaTime;
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

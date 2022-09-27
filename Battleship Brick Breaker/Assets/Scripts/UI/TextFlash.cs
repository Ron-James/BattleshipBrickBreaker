using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextFlash : MonoBehaviour
{
    [SerializeField] TextMeshPro text;
    [SerializeField] TextMeshProUGUI UItext;
    [SerializeField] Color flashColor;
    [SerializeField] float flashDuration;
    [SerializeField] float flashPeriod;
    [SerializeField] Sound flashSound;
    [SerializeField] float currentFlashTime = 0;
    Coroutine flashCoroutine;

    public Color FlashColor { get => flashColor; set => flashColor = value; }
    public float FlashDuration { get => flashDuration; set => flashDuration = value; }

    // Start is called before the first frame update
    void Start()
    {
        SetTextActive(false);

    }

    public void SetTextActive(bool active)
    {
        if (text != null)
        {
            text.enabled = active;
        }

        if (UItext != null)
        {
            UItext.enabled = active;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StopTextFlash()
    {
        currentFlashTime = 0;
    }

    public void FlashText()
    {

        if (flashCoroutine == null)
        {
            flashCoroutine = StartCoroutine(Flash(flashDuration, flashPeriod, FlashColor));
        }
        else
        {
            return;
        }
    }

    public void ChangeColor(Color color)
    {
        if (text != null)
        {
            text.color = color;
        }

        if (UItext != null)
        {
            UItext.color = color;
        }
    }

    public Color TextColor()
    {
        if (text != null)
        {
            return text.color;
        }

        else if (UItext != null)
        {
            return UItext.color;
        }
        else{
            return flashColor;
        }
    }


    IEnumerator Flash(float duration, float period, Color color)
    {
        flashSound.PlayLoop();
        SetTextActive(true);
        currentFlashTime = duration;
        float flashTime = 0;
        int count = 0;
        Color defaultColor = TextColor();
        while (true)
        {
            if (currentFlashTime <= 0)
            {
                flashCoroutine = null;
                ChangeColor(defaultColor);
                SetTextActive(false);
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
                    ChangeColor(defaultColor);
                }
                else
                {
                    ChangeColor(color);
                }
                yield return null;
            }
        }
    }
}

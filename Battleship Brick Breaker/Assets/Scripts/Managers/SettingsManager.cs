using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : Singleton<SettingsManager>
{
    [Header("Fire Button Control")]
    [SerializeField] Toggle rightHandP1;
    [SerializeField] Toggle rightHandP2;
    [SerializeField] TextMeshProUGUI p1Side;
    [SerializeField] TextMeshProUGUI p2Side;
    [SerializeField] GameObject rightSideFireButtonP1;
    [SerializeField] GameObject leftSideFireButtonP1;
    [SerializeField] GameObject rightSideFireButtonP2;
    [SerializeField] GameObject leftSideFireButtonP2;

    public static bool p1Right;
    public static bool p2Right;

    [Header("Fire Button Control")]
    [SerializeField] Slider initialVelocity;

    public Toggle RightHandP2 { get => rightHandP2; set => rightHandP2 = value; }
    public Toggle RightHandP1 { get => rightHandP1; set => rightHandP1 = value; }


    // Start is called before the first frame update
    void Start()
    {
        UpdateFireButtons();
        //UpdateSliderText(initialVelocity);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateFireButtons()
    {
        if (rightHandP1.isOn)
        {
            p1Right = true;
            rightSideFireButtonP1.SetActive(true);
            leftSideFireButtonP1.SetActive(false);
            p1Side.text = "Right";
        }
        else
        {
            p1Right = false;
            rightSideFireButtonP1.SetActive(false);
            leftSideFireButtonP1.SetActive(true);
            p1Side.text = "Left";
        }

        if (rightHandP2.isOn)
        {
            p2Right = true;
            rightSideFireButtonP2.SetActive(true);
            leftSideFireButtonP2.SetActive(false);
            p2Side.text = "Right";
        }
        else
        {
            p2Right = false;
            rightSideFireButtonP2.SetActive(false);
            leftSideFireButtonP2.SetActive(true);
            p2Side.text = "Left";
        }
    }
    

    public void UpdateSliderText(Slider slider)
    {
        TextMeshProUGUI text = slider.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        text.text = slider.value.ToString();
    }

    public void UpdateInitialVelocity()
    {
        GameManager.instance.InitialVelocity = initialVelocity.value;
    }

    public void ToggleTutorial(Toggle toggle)
    {
        TutorialManager.instance.isTutorial = toggle.isOn;
        TutorialManager.instance.DisableAllPrompts();
    }

}

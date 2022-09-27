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

    [SerializeField] GameObject[] player1CannonButtons = new GameObject[2];
    [SerializeField] GameObject[] player2CannonButtons = new GameObject[2];

    [SerializeField] GameObject[] player1BombButtons = new GameObject[2];
    [SerializeField] GameObject[] player2BombButtons = new GameObject[2];

    [Header("Volume Sliders")]
    [SerializeField] Slider globalSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    [Header("Volume")]
    public static float globalVolume;
    public static float sfxVolume;
    public static float musicVolume;




    public static bool p1Right;
    public static bool p2Right;

    [Header("Fire Button Control")]
    [SerializeField] Slider initialVelocity;

    public Toggle RightHandP2 { get => rightHandP2; set => rightHandP2 = value; }
    public Toggle RightHandP1 { get => rightHandP1; set => rightHandP1 = value; }

    public void SetGlobalVolume(float volume)
    {
        SettingsManager.globalVolume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        SettingsManager.sfxVolume = volume;
    }
    public void SetMusicVolume(float volume)
    {
        SettingsManager.musicVolume = volume;
    }



    // Start is called before the first frame update
    void Start()
    {
        UpdateVolumeFromPrefs();
        SetDefaultPrefs();
        UpdateFromPrefs();
        UpdateFireButtons();
        Debug.Log(ActiveBombButton(true).name);

        //UpdateSliderText(initialVelocity);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject ActiveBombButton(bool player1)
    {
        if (player1)
        {
            if (p1Right)
            {
                return player1BombButtons[1];
            }
            else
            {
                return player1BombButtons[0];
            }
        }
        else
        {
            if (p2Right)
            {
                return player2BombButtons[1];
            }
            else
            {
                return player2BombButtons[0];
            }
        }
    }
    public GameObject ActiveCannonButton(bool player1)
    {
        if (player1)
        {
            if (p1Right)
            {
                return player1CannonButtons[0];
            }
            else
            {
                return player1CannonButtons[1];
            }
        }
        else
        {
            if (p2Right)
            {
                return player2CannonButtons[0];
            }
            else
            {
                return player2CannonButtons[1];
            }
        }
    }
    public void SetDefaultPrefs()
    {
        if (!PlayerPrefs.HasKey("P1 Hand"))
        {
            PlayerPrefs.SetString("P1 Hand", "Right");
        }
        if (!PlayerPrefs.HasKey("P2 Hand"))
        {
            PlayerPrefs.SetString("P2 Hand", "Right");
        }
    }
    public void UpdateFromPrefs()// update button game objects from player prefs
    {
        string p1Hand = PlayerPrefs.GetString("P1 Hand");
        string p2Hand = PlayerPrefs.GetString("P2 Hand");

        switch (p1Hand)
        {
            case "Left":
                rightHandP1.isOn = false;
                p1Right = false;
                SetButtonSide(player1CannonButtons, false);
                SetButtonSide(player1BombButtons, true);
                p1Side.text = "Left";
                break;
            case "Right":
                rightHandP1.isOn = true;
                p1Right = true;
                SetButtonSide(player1CannonButtons, true);
                SetButtonSide(player1BombButtons, false);
                p1Side.text = "Right";
                break;
        }
        switch (p2Hand)
        {
            case "Left":
                rightHandP2.isOn = false;
                p2Right = false;
                SetButtonSide(player2CannonButtons, false);
                SetButtonSide(player2BombButtons, true);
                p2Side.text = "Left";
                break;
            case "Right":
                rightHandP2.isOn = true;
                p2Right = true;
                SetButtonSide(player2CannonButtons, true);
                SetButtonSide(player2BombButtons, false);
                p2Side.text = "Right";
                break;
        }

    }
    public void UpdateFireButtons()
    {
        if (rightHandP1.isOn)
        {
            PlayerPrefs.SetString("P1 Hand", "Right");

            p1Right = true;
            SetButtonSide(player1CannonButtons, true);
            SetButtonSide(player1BombButtons, false);
            p1Side.text = "Right";
        }
        else
        {
            PlayerPrefs.SetString("P1 Hand", "Left");
            p1Right = false;
            SetButtonSide(player1CannonButtons, false);
            SetButtonSide(player1BombButtons, true);
            p1Side.text = "Left";
        }

        if (rightHandP2.isOn)
        {
            PlayerPrefs.SetString("P2 Hand", "Right");
            p2Right = true;
            SetButtonSide(player2CannonButtons, true);
            SetButtonSide(player2BombButtons, false);
            p2Side.text = "Right";
        }
        else
        {
            PlayerPrefs.SetString("P2 Hand", "Left");
            p2Right = false;
            SetButtonSide(player2CannonButtons, false);
            SetButtonSide(player2BombButtons, true);
            p2Side.text = "Left";
        }
    }


    public void UpdateVolumePrefs()
    {
        SettingsManager.globalVolume = globalSlider.value;
        SettingsManager.sfxVolume = sfxSlider.value;
        SettingsManager.musicVolume = musicSlider.value;
        PlayerPrefs.SetFloat("Global Volume", SettingsManager.globalVolume);
        PlayerPrefs.SetFloat("Music Volume", SettingsManager.musicVolume);
        PlayerPrefs.SetFloat("SFX Volume", SettingsManager.sfxVolume);
    }

    public void UpdateVolumeFromPrefs()
    {
        if (!PlayerPrefs.HasKey("Global Volume"))
        {
            UpdateVolumePrefs();
        }
        else
        {
            SettingsManager.globalVolume = PlayerPrefs.GetFloat("Global Volume");
            SettingsManager.sfxVolume = PlayerPrefs.GetFloat("SFX Volume");
            SettingsManager.musicVolume = PlayerPrefs.GetFloat("Music Volume");
            globalSlider.value = SettingsManager.globalVolume;
            sfxSlider.value = SettingsManager.sfxVolume;
            musicSlider.value = SettingsManager.musicVolume;
        }

    }


    public void SetButtonSide(GameObject[] buttons, bool right = true)
    {
        if (right)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);

        }
        else
        {
            buttons[1].SetActive(true);
            buttons[0].SetActive(false);
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

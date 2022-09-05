using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt : MonoBehaviour
{
    [SerializeField] GameObject[] prompts = new GameObject[2];
    public bool hasAckP1;
    public bool hasAckP2;

    bool isEnabledP1;
    bool isEnabledP2;

    
    public bool IsEnabledP1 { get => isEnabledP1; set => isEnabledP1 = value; }
    public bool IsEnabledP2 { get => isEnabledP2; set => isEnabledP2 = value; }

    private void Awake()
    {
        hasAckP1 = false;
        hasAckP2 = false;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenPrompt(bool player1)
    {
        if (player1)
        {
            prompts[0].SetActive(true);
            isEnabledP1 = true;
        }
        else
        {
            prompts[1].SetActive(true);
            isEnabledP2 = true;
        }
    }

    public void ClosePrompt(bool player1)
    {
        if (player1)
        {
            prompts[0].SetActive(false);
            isEnabledP1 = false;
        }
        else
        {
            prompts[1].SetActive(false);
            isEnabledP2 = false;
        }
    }

    public void EnableBoth()
    {
        OpenPrompt(true);
        OpenPrompt(false);
        isEnabledP1 = true;
        isEnabledP2 = true;
    }

    public void DisableBoth()
    {
        ClosePrompt(true);
        ClosePrompt(false);
        isEnabledP2 = false;
        isEnabledP1 = false;
    }

    public bool IsOpen(bool player1)
    {
        if (player1)
        {
            return prompts[0].activeInHierarchy;
        }
        else
        {
            return prompts[1].activeInHierarchy;
        }
    }

    public bool HasAcknowledged(bool player1){
        if(player1){
            return hasAckP1;
        }
        else{
            return hasAckP2;
        }
    }
    public void SetAcknowledge(bool ack, bool player1){
        if(player1){
            hasAckP1 = ack;
        }
        else{
            hasAckP2 = ack;
        }
    }

    public bool IsEnabled(bool player1){
        if(player1){
            return isEnabledP1;
        }
        else{
            return isEnabledP2;
        }
    }

}

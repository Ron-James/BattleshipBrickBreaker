using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] GameObject panel;

    [SerializeField] Text elapsedTime;
    [SerializeField] Text winner;
    [SerializeField] Text[] bricksBroken = new Text[2];
    [SerializeField] Text[] objBricks = new Text[2];
    [SerializeField] Text[] shotsHit = new Text[2];
    [SerializeField] Text[] missed = new Text[2];
    // Start is called before the first frame update
    void Start()
    {
        ResetClose();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open(bool p1)
    {
        panel.SetActive(true);
        if (p1)
        {
            winner.text = "Player 1 (Right) Wins!";
        }
        else
        {
            winner.text = "Player 2 (Left) Wins!";
        }
        bricksBroken[0].text = GameManager.instance.BricksBroken[0].ToString();
        bricksBroken[1].text = GameManager.instance.BricksBroken[1].ToString();

        objBricks[0].text = GameManager.instance.ObjBroken[0].ToString();
        objBricks[1].text = GameManager.instance.ObjBroken[1].ToString();

        shotsHit[0].text = GameManager.instance.ShotsHit[0].ToString();
        shotsHit[1].text = GameManager.instance.ShotsHit[1].ToString();

        missed[0].text = GameManager.instance.Missed[0].ToString();
        missed[1].text = GameManager.instance.Missed[1].ToString();

        UpdateTime();
    }

    public void ResetClose()
    {
        panel.SetActive(false);
        bricksBroken[0].text = "0";
        bricksBroken[1].text = "0";

        objBricks[0].text = "0";
        objBricks[1].text = "0";

        shotsHit[0].text = "0";
        shotsHit[1].text = "0";

        missed[0].text = "0";
        missed[1].text = "0";

        elapsedTime.text = "00:00";
    }

    public void UpdateTime()
    {
        int mins = (int)GameManager.instance.TimeElapsed / 60;
        int seconds = (int)GameManager.instance.TimeElapsed % 60;
        if(mins > 9 && seconds > 9){
            elapsedTime.text = mins.ToString() + ":" + seconds.ToString();
        }
        else if(mins < 10 && seconds > 9){
            elapsedTime.text = "0" + mins.ToString() + ":" + seconds.ToString();
        }
        else if(mins < 10 && seconds < 9){
            elapsedTime.text = "0" + mins.ToString() + ":0" + seconds.ToString();
        }
        
    }
}

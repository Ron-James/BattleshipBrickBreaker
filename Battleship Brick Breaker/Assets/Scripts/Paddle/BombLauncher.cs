using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BombLauncher : MonoBehaviour
{
    [SerializeField] bool player1;
    [SerializeField] GameObject bombLauncherUI;
    [SerializeField] Image powerUpBar;
    [SerializeField] GameObject inactiveBombs;

    [SerializeField] float bombHeight = 12f;
    [SerializeField] Transform firePoint;
    [SerializeField] bool hasBomb;
    [SerializeField] Bomb currentBomb;
    [SerializeField] PaddleSoundBox paddleSoundBox;
    [SerializeField] Transform targetMarker;
    [SerializeField] float targetMarkerDelayTime = 1f;
    [SerializeField] bool bombButtonDown;
    public bool canLaunch;
    PowerUpManager powerUpManager;
    Touch lastTouch;
    int touchIndex;
    Vector3 touchPos;
    Coroutine launchSequence;
    GameObject bombButton;
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }
    public GameObject BombButton { get => bombButton; set => bombButton = value; }
    public Transform FirePoint { get => firePoint; set => firePoint = value; }

    public void SetBombButtonActive(bool active)
    {
        GameObject bombButtonCurrent = SettingsManager.instance.ActiveBombButton(player1);
        bombButtonCurrent.SetActive(active);

    }


    public void SetBombButtonDown(bool down)
    {
        bombButtonDown = down;

        if (bombButtonDown == true)
        {
            if (currentBomb != null && canLaunch)
            {
                if (launchSequence == null)
                {
                    launchSequence = StartCoroutine(BombLaunchSequence(GameManager.instance.BombPowerUpTime));
                }

            }
            
        }


    }
    private void OnDisable()
    {
        StopAllCoroutines();
        //BallEvents ballEvents = GetComponent<PaddleController>().Ball.GetComponent<BallEvents>();

    }

    // Start is called before the first frame update
    void Start()
    {
        SetBombButtonActive(false);
        powerUpManager = GetComponent<PowerUpManager>();
        //SetBombButtonActive(false);
        paddleSoundBox = GetComponentInChildren<PaddleSoundBox>();
        ResetBombLauncher();
        DisablePowerUpBar();


    }

    // Update is called once per frame
    void Update()
    {

        /*
        if (GameManager.instance.TouchInField(out touchIndex, out touchPos, player1) && hasBomb && launchSequence == null && !GetComponentInChildren<AimArrow>().Aiming && Input.touches[touchIndex].phase == TouchPhase.Began)
        {
            if (currentBomb != null && canLaunch)
            {
                lastTouch = Input.touches[touchIndex];
                launchSequence = StartCoroutine(BombLaunchSequence(GameManager.instance.BombPowerUpTime));
            }


        }
        */
    }
    public void EnablePowerUpBar()
    {
        bombLauncherUI.SetActive(true);
        powerUpBar.fillAmount = 0;
    }
    public void DisablePowerUpBar()
    {
        bombLauncherUI.SetActive(false);
        powerUpBar.fillAmount = 0;
    }
    public void LoadBomb()
    {
        if (currentBomb != null)
        {
            return;
        }


        if (TutorialManager.instance.isTutorial)
        {
            if (!TutorialManager.instance.bombPowerUp.HasAcknowledged(player1))
            {
                SetBombButtonActive(true);
                TutorialManager.instance.bombPowerUp.OpenPrompt(player1);
                Debug.Log(SettingsManager.instance.ActiveBombButton(player1).name);
                SettingsManager.instance.ActiveBombButton(player1).GetComponent<ButtonPrompt>().StartFadeFlash();
            }
        }
        Bomb bomb = inactiveBombs.GetComponentInChildren<Bomb>();
        bomb.EnableBomb(firePoint);
        bomb.gameObject.transform.SetParent(transform);

        currentBomb = bomb;
        SetBombButtonActive(true);

    }

    public void ResetBombLauncher()
    {
        if (currentBomb != null)
        {
            currentBomb.DisableBomb();
            currentBomb = null;
        }
        DisablePowerUpBar();
        powerUpManager.OpenBombSlot();
        hasBomb = false;
    }

    public void GiveBomb()
    {
        hasBomb = true;
    }

    public void SetTargetMarkerPosition(Vector3 position)
    {
        Vector3 newPos = position;
        newPos.y = 3f;
        targetMarker.position = newPos;
    }

    IEnumerator TargetMarkerDelay(float time)
    {
        yield return new WaitForSeconds(time);
        targetMarker.gameObject.SetActive(false);
        targetMarker.SetParent(this.transform);
    }

    public void DisableTargetMarker()
    {
        targetMarker.SetParent(null);
        StartCoroutine(TargetMarkerDelay(targetMarkerDelayTime));
    }
    IEnumerator BombLaunchSequence(float period)
    {
        GetComponent<PowerUpManager>().ResetBombPowerUp();
        
        targetMarker.gameObject.SetActive(true);
        powerUpBar.fillAmount = 0;
        float time = 0;
        bool soundSwitched = false;
        float xDifference = GameManager.instance.paddle1.transform.position.x - GameManager.instance.paddle2.transform.position.x;
        xDifference = Mathf.Abs(xDifference);
        Vector3 target = firePoint.position;
        int sign = -1;
        float diff = xDifference - GameManager.instance.MinBombLaunchDistance;
        SetTargetMarkerPosition(target);
        //Debug.Log(xDifference + " Xdifference" + diff + " difference ");
        float w = 2 * Mathf.PI * (1 / period);
        paddleSoundBox.bombPowerUp.PlayOnce();

        if(TutorialManager.instance.isTutorial){
            TutorialManager.instance.bombPowerUp.SetAcknowledge(player1, true);
            TutorialManager.instance.bombPowerUp.ClosePrompt(player1);
        }




        if (!player1)
        {
            sign = 1;
        }

        EnablePowerUpBar();
        while (true)
        {
            if (currentBomb == null)
            {
                targetMarker.gameObject.SetActive(false);
                launchSequence = null;
                powerUpManager.OpenBombSlot();
                ResetBombLauncher();
                DisableTargetMarker();
                SetBombButtonActive(false);

                break;
            }

            else if (!bombButtonDown)
            {
                SetTargetMarkerPosition(new Vector3(target.x, 5f, target.z));
                currentBomb.LaunchBomb(target, bombHeight, player1);
                powerUpManager.OpenBombSlot();
                paddleSoundBox.bombThrow.PlayOnce();
                launchSequence = null;
                currentBomb = null;
                DisablePowerUpBar();
                hasBomb = false;
                DisableTargetMarker();
                SetBombButtonActive(false);

                break;
            }
            else if (time >= (period / 2) || Mathf.Sin(time * w) < 0)
            {

                float d = GameManager.instance.MinBombLaunchDistance + (diff * Mathf.Sin(time * w));
                target = firePoint.position;
                target.x += (sign * d);
                SetTargetMarkerPosition(new Vector3(target.x, 3f, target.z));
                currentBomb.LaunchBomb(target, bombHeight, player1);
                powerUpManager.OpenBombSlot();
                launchSequence = null;
                ResetBombLauncher();
                Debug.Log(target + " Target Position");
                paddleSoundBox.bombThrow.PlayOnce();
                DisableTargetMarker();
                SetBombButtonActive(false);
                
                break;
            }
            else
            {
                float d = GameManager.instance.MinBombLaunchDistance + (diff * Mathf.Sin(time * w));
                target = firePoint.position;
                target.x += (sign * d);
                SetTargetMarkerPosition(new Vector3(target.x, 3f, target.z));
                time += Time.deltaTime;
                powerUpBar.fillAmount = Mathf.Sin(time * w);

                yield return null;
            }
            if (time >= period / 4 && !soundSwitched)
            {
                paddleSoundBox.bombPowerUp.StopSource();
                soundSwitched = true;
            }
        }
    }


    public void OnBallOut()
    {
        ResetBombLauncher();
        SetBombButtonActive(false);
        SetBombButtonActive(false);
        if (launchSequence != null)
        {
            currentBomb = null;
        }
    }
}

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
    public bool canLaunch;
    Touch lastTouch;
    int touchIndex;
    Vector3 touchPos;
    Coroutine launchSequence;
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }

    // Start is called before the first frame update
    void Start()
    {
        paddleSoundBox = GetComponentInChildren<PaddleSoundBox>();
        ResetBombLauncher();
        DisablePowerUpBar();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            hasBomb = true;
        }
        if (hasBomb)
        {
            if (currentBomb == null)
            {
                LoadBomb();
            }
        }
        if (GameManager.instance.TouchInField(out touchIndex, out touchPos, player1) && hasBomb && launchSequence == null && !GetComponentInChildren<AimArrow>().Aiming && Input.touches[touchIndex].phase == TouchPhase.Began)
        {
            if (currentBomb != null && canLaunch)
            {
                lastTouch = Input.touches[touchIndex];
                launchSequence = StartCoroutine(BombLaunchSequence(GameManager.instance.BombPowerUpTime));
            }


        }
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
        Bomb bomb = inactiveBombs.GetComponentInChildren<Bomb>();
        bomb.gameObject.transform.SetParent(this.transform);
        bomb.EnableBomb(firePoint.position);
        currentBomb = bomb;
    }

    public void ResetBombLauncher()
    {
        if (currentBomb != null)
        {
            currentBomb.DisableBomb();
            currentBomb = null;
        }
        DisablePowerUpBar();
        hasBomb = false;
    }

    public void GiveBomb()
    {
        hasBomb = true;
    }

    public void SetTargetMarkerPosition(Vector3 position){
        Vector3 newPos = position;
        newPos.y = 2f;
        targetMarker.position = newPos;
    }

    IEnumerator TargetMarkerDelay(float time){
        yield return new WaitForSeconds(time);
        targetMarker.gameObject.SetActive(false);
        targetMarker.SetParent(this.transform);
    }

    public void DisableTargetMarker(){
        targetMarker.SetParent(null);
        StartCoroutine(TargetMarkerDelay(targetMarkerDelayTime));
    }
    IEnumerator BombLaunchSequence(float period)
    {
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
        if (!player1)
        {
            sign = 1;
        }

        EnablePowerUpBar();
        while (true)
        {
            if (!GameManager.instance.TouchInField(out touchIndex, out touchPos, player1))
            {
                Debug.Log(target + " Target Position");
                SetTargetMarkerPosition(target);
                currentBomb.LaunchBomb(target, bombHeight, player1);
                paddleSoundBox.bombThrow.PlayOnce();
                launchSequence = null;
                currentBomb = null;
                DisablePowerUpBar();
                hasBomb = false;
                DisableTargetMarker();
                break;
            }
            else if (time >= (period / 2) || Mathf.Sin(time * w) < 0)
            {
                float d = GameManager.instance.MinBombLaunchDistance + (diff * Mathf.Sin(time * w));
                target = firePoint.position;
                target.x += (sign * d);
                SetTargetMarkerPosition(target);
                currentBomb.LaunchBomb(target, bombHeight, player1);
                launchSequence = null;
                ResetBombLauncher();
                Debug.Log(target + " Target Position");
                paddleSoundBox.bombThrow.PlayOnce();
                DisableTargetMarker();
                break;
            }
            else if (currentBomb == null)
            {
                targetMarker.gameObject.SetActive(false);
                launchSequence = null;
                ResetBombLauncher();
                break;
            }
            else
            {
                float d = GameManager.instance.MinBombLaunchDistance + (diff * Mathf.Sin(time * w));
                target = firePoint.position;
                target.x += (sign * d);
                SetTargetMarkerPosition(target);
                time += Time.deltaTime;
                powerUpBar.fillAmount = Mathf.Sin(time * w);

                yield return null;
            }
            if(time >= period / 4 && !soundSwitched){
                paddleSoundBox.bombPowerUp.StopSource();
                soundSwitched = true;
            }
        }
    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    
    IEnumerator BombLaunchCountDown(float duration){
        float time = duration;
        powerUpBar.fillAmount = 1;
        while(true){
            if(time <= 0){
                launchSequence = null;
                ResetBombLauncher();
                break;
            }
            if(GameManager.instance.TouchInField(out touchIndex, out touchPos, player1) && currentBomb != null){
                launchSequence = null;
                lastTouch = Input.touches[touchIndex];
                launchSequence = StartCoroutine(BombLaunchSequence(GameManager.instance.BombPowerUpTime));
                break;
            }
            else{
                powerUpBar.fillAmount = time / duration;
                time -= Time.deltaTime;
                yield return null;
            }
        }
    }
}

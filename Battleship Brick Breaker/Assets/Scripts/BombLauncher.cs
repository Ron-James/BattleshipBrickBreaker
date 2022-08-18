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
    Touch lastTouch;
    int touchIndex;
    Vector3 touchPos;
    Coroutine launchSequence;
    public bool HasBomb { get => hasBomb; set => hasBomb = value; }

    // Start is called before the first frame update
    void Start()
    {
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
            if (currentBomb != null)
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


    IEnumerator BombLaunchSequence(float period)
    {
        powerUpBar.fillAmount = 0;
        float time = 0;
        float xDifference = GameManager.instance.paddle1.transform.position.x - GameManager.instance.paddle2.transform.position.x;
        xDifference = Mathf.Abs(xDifference);
        Vector3 target = firePoint.position;
        int sign = -1;
        float diff = xDifference - GameManager.instance.MinBombLaunchDistance;
        //Debug.Log(xDifference + " Xdifference" + diff + " difference ");
        float w = 2 * Mathf.PI * (1 / period);
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
                currentBomb.LaunchBomb(target, bombHeight, player1);
                launchSequence = null;
                currentBomb = null;
                DisablePowerUpBar();
                hasBomb = false;
                
                break;
            }
            else if (time >= (period / 2) || Mathf.Sin(time * w) < 0)
            {
                float d = GameManager.instance.MinBombLaunchDistance + (diff * Mathf.Sin(time * w));
                target = firePoint.position;
                target.x += (sign * d);
                currentBomb.LaunchBomb(target, bombHeight, player1);
                launchSequence = null;
                ResetBombLauncher();
                Debug.Log(target + " Target Position");
                break;
            }
            else if (currentBomb == null)
            {
                launchSequence = null;
                ResetBombLauncher();
                break;
            }
            else
            {
                float d = GameManager.instance.MinBombLaunchDistance + (diff * Mathf.Sin(time * w));
                target = firePoint.position;
                target.x += (sign * d);
                time += Time.deltaTime;
                powerUpBar.fillAmount = Mathf.Sin(time * w);

                yield return null;
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

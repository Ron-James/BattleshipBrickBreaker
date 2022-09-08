using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Artillery : MonoBehaviour
{
    [SerializeField] GameObject inactiveBullets;
    [SerializeField] Transform oppPaddle;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform[] firePoints = new Transform[3];
    [SerializeField] bool canFire = true;

    [SerializeField] float bulletGravity = -10;
    [SerializeField] float bulletHeight = 5f;

    [SerializeField] Button fireButton;
    [SerializeField] Button fireButtonLeft;
    [SerializeField] TextMeshProUGUI ammoIndicator;
    [SerializeField] TextMeshProUGUI ammoIndicatorLeft;


    [SerializeField] int ammo = 0;

    [SerializeField] bool player1;

    float fireDistance;
    [SerializeField] AimArrow aim;
    [SerializeField] PaddleSoundBox paddleSoundBox;
    public Transform FirePoint { get => firePoint; set => firePoint = value; }
    public bool CanFire { get => canFire; set => canFire = value; }
    public int Ammo { get => ammo; set => ammo = value; }

    float lastTapTime;

    int touchIndex;
    Vector3 touchPos;
    Coroutine doubleTap;
    Touch lastTouch;


    private void OnDisable()
    {
        StopAllCoroutines();
        //GetComponent<PaddleController>().Ball.GetComponent<BallEvents>().OnBallOut.RemoveListener(OnBallOut);
    }
    // Start is called before the first frame update
    void Start()
    {

        paddleSoundBox = GetComponentInChildren<PaddleSoundBox>();
        //ammoIndicator = ammoUI.GetComponentInChildren<TextMeshProUGUI>();
        //ammo = 99;
        //AddAmmo(1);
        UpdateAmmo();
        canFire = false;

    }
    void TestArtillery()
    {
        ammo++;
        Fire();
    }
    // Update is called once per frame
    void Update()
    {
        if (player1 && Input.GetKeyDown(KeyCode.Space))
        {
            TestArtillery();
        }

        if (GameManager.instance.TouchInField(out touchIndex, out touchPos, player1) && !aim.Aiming && !GetComponent<BombLauncher>().HasBomb && Input.touches[touchIndex].phase == TouchPhase.Began)
        {

            float timeSinceLastTap = Time.time - lastTapTime;
            //Debug.Log("single tap" + timeSinceLastTap);
            if (timeSinceLastTap <= GameManager.instance.DoubleTapTime )
            {
                Debug.Log("Double tap" + timeSinceLastTap);
                if (ammo > 0 && !PauseManager.isPaused)
                {
                    Fire();
                }
            }
            else
            {
                Debug.Log("single tap" + timeSinceLastTap);
            }
            lastTapTime = Time.time;
        }
    }
    public void Fire()
    {
        if (inactiveBullets.GetComponentsInChildren<Bullet>().Length == 0)
        {
            Debug.Log("No bullets to use");
            return;
        }
        else if (ammo <= 0)
        {
            Debug.Log("no ammo");
            return;
        }
        else if (!canFire)
        {
            return;
        }
        else if (GetComponent<PowerUpManager>().IsTripleCannon())
        {
            
            for (int loop = 0; loop < 3; loop++)
            {
                Bullet bullet = inactiveBullets.GetComponentsInChildren<Bullet>()[loop];
                bullet.player1 = player1;
                bullet.EnableBullet(firePoints[loop].position, player1);
                Vector3 target = new Vector3(oppPaddle.position.x, firePoints[loop].position.y, firePoints[loop].position.z);
                bullet.Launch(bulletHeight, target, player1);
            }
            AddAmmo(-1);
            if (TutorialManager.instance.isTutorial)
            {
                TutorialManager.instance.cannonLaunch.ClosePrompt(player1);
                TutorialManager.instance.cannonLaunch.SetAcknowledge(true, player1);
            }

        }
        else
        {
            
            Bullet bullet = inactiveBullets.GetComponentsInChildren<Bullet>()[0];
            bullet.player1 = player1;
            bullet.EnableBullet(firePoint.position, player1);
            Vector3 target = new Vector3(oppPaddle.position.x, transform.position.y, transform.position.z);
            bullet.Launch(bulletHeight, target, player1);
            AddAmmo(-1);
            UpdateFireButton();
            paddleSoundBox.cannonSound.PlayOnce();
            if (TutorialManager.instance.isTutorial)
            {
                TutorialManager.instance.cannonLaunch.ClosePrompt(player1);
                TutorialManager.instance.cannonLaunch.SetAcknowledge(true, player1);
            }

        }

    }
    public void UpdateAmmo()
    {
        ammoIndicator.SetText(ammo.ToString());
        ammoIndicatorLeft.SetText(ammo.ToString());

        UpdateFireButton();

    }
    public void AddAmmo(int amount)
    {
        if(TutorialManager.instance.isTutorial){
            if(!TutorialManager.instance.cannonLaunch.HasAcknowledged(player1)){
                if(!TutorialManager.instance.bombPowerUp.IsEnabled(player1) && !TutorialManager.instance.bombThrow.IsEnabled(player1)){
                    TutorialManager.instance.SwitchLauchTut(TutorialManager.LaunchTutorial.Cannon, player1);
                    TutorialManager.instance.EnableCannonPrompt(player1);
                }
            }
        }
        ammo += amount;
        if (ammo < 0)
        {
            ammo = 0;
        }
        UpdateAmmo();
        UpdateFireButton();
    }

    public void OnBallOut()
    {
        CanFire = false;
    }

    public void UpdateFireButton()
    {
        if (ammo <= 0)
        {
            fireButton.interactable = false;
            fireButtonLeft.interactable = false;
        }
        else
        {
            fireButtonLeft.interactable = true;
            fireButton.interactable = true;
        }
    }







}

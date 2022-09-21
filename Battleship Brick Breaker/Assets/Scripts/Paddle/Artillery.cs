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

    
    [SerializeField] float bulletHeight = 5f;

    [SerializeField] Button fireButton;
    [SerializeField] Button fireButtonLeft;
    [SerializeField] TextMeshProUGUI ammoIndicator;
    [SerializeField] TextMeshProUGUI ammoIndicatorLeft;



    [SerializeField] int ammo = 0;

    [SerializeField] bool player1;
    [SerializeField] float snowThrowFrequency = 0.5f;

    float fireDistance;
    [SerializeField] AimArrow aim;
    [SerializeField] PaddleSoundBox paddleSoundBox;
    [SerializeField] float snowThrowerTime;
    public Transform FirePoint { get => firePoint; set => firePoint = value; }
    public bool CanFire { get => canFire; set => canFire = value; }
    public int Ammo { get => ammo; set => ammo = value; }

    float lastTapTime;

    int touchIndex;
    Vector3 touchPos;
    Coroutine doubleTap;
    Touch lastTouch;
    Coroutine snowThrower;


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
        if(Input.GetKeyDown(KeyCode.Space) && player1 ==  true){
            AddSnowThrowerTime(10f);
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
                Bullet bullet = GetActiveBullet();
                bullet.player1 = player1;
                bullet.EnableBullet(firePoints[loop].position, player1, Bullet.BulletType.cannon);
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
            bullet.EnableBullet(firePoint.position, player1, Bullet.BulletType.cannon);
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
    public Bullet GetActiveBullet(){
        Bullet [] bullets = inactiveBullets.GetComponentsInChildren<Bullet>();
        for(int loop = 0; loop < bullets.Length; loop++){
            if(!bullets[loop].isActive){
                return bullets[loop];
            }
            else{
                continue;
            }
        }
        return null;
    }
    public void FireSnowBall(){
        Bullet snowBall = GetActiveBullet();
        snowBall.player1 = player1;
        snowBall.EnableBullet(firePoint.position, player1, Bullet.BulletType.snow);
        Vector3 target = new Vector3(oppPaddle.position.x, transform.position.y, transform.position.z);
        snowBall.Launch(bulletHeight, target, player1);

    }

    public void AddSnowThrowerTime(float duration){
        snowThrowerTime += duration;
        if(snowThrowerTime > 0){
            if(snowThrower == null){
                snowThrower = StartCoroutine(SnowBallThrow(snowThrowFrequency));
            }
        }
    }

    public void StopSnowThrower(){
        snowThrowerTime = 0;
    }

    IEnumerator SnowBallThrow(float frequency){
        float throwTime = 0;

        while(true){
            snowThrowerTime -= Time.deltaTime;
            throwTime += Time.deltaTime;

            if(snowThrowerTime <= 0){
                snowThrower = null;
                snowThrowerTime = 0;
                break;
            }
            else{
                if(throwTime >= frequency){
                    FireSnowBall();
                    throwTime = 0;
                }

                yield return null;
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
        Debug.Log(SettingsManager.instance.ActiveCannonButton(player1).GetComponentInChildren<FloatingText>().gameObject.name);
        FloatingText floatingText = SettingsManager.instance.ActiveCannonButton(player1).GetComponentInChildren<FloatingText>();
        if(floatingText != null){
            if(amount > 0){
                floatingText.DeployFloatingText(floatingText.transform.position, "+" + amount.ToString());
            }
            else if(amount < 0){
                int num = Mathf.Abs(amount);
                floatingText.DeployFloatingText(floatingText.transform.position, "-" + num.ToString());
            }
        }
        
        if(TutorialManager.instance.isTutorial){
            if(!TutorialManager.instance.cannonLaunch.HasAcknowledged(player1)){
                TutorialManager.instance.EnableCannonPrompt(player1);
                TutorialManager.instance.cannonLaunch.OpenPrompt(player1);
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

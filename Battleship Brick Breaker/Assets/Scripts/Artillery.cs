using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Artillery : MonoBehaviour
{
    [SerializeField] GameObject inactiveBullets;
    [SerializeField] Transform oppPaddle;
    [SerializeField] GameObject cannon;
    [SerializeField] float cannonRotateSpeed;
    [SerializeField] Transform firePoint;
    [SerializeField] bool PC = false;
    [SerializeField] bool canFire = true;

    [SerializeField] float bulletGravity = -10;
    [SerializeField] float bulletHeight = 5f;

    [SerializeField] Button fireButton;
    [SerializeField] TextMeshProUGUI ammoIndicator;
    [SerializeField] GameObject ammoUI;

    [SerializeField] int ammo = 0;

    [SerializeField] bool player1;

    float fireDistance;
    [SerializeField] AimArrow aim;
    [SerializeField] PaddleSoundBox paddleSoundBox;
    public Transform FirePoint { get => firePoint; set => firePoint = value; }
    public bool CanFire { get => canFire; set => canFire = value; }
    float lastTapTime;

    int touchIndex;
    Vector3 touchPos;
    Coroutine doubleTap;
    Touch lastTouch;
    // Start is called before the first frame update
    void Start()
    {
        paddleSoundBox = GetComponentInChildren<PaddleSoundBox>();
        ammoIndicator = ammoUI.GetComponentInChildren<TextMeshProUGUI>();
        //ammo = 99;
        AddAmmo(0);
        UpdateAmmo();

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
            if (timeSinceLastTap <= GameManager.instance.DoubleTapTime)
            {
                Debug.Log("Double tap" + timeSinceLastTap);
                if (ammo > 0)
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
        else
        {
            Bullet bullet = inactiveBullets.GetComponentsInChildren<Bullet>()[0];
            bullet.player1 = player1;
            bullet.EnableBullet(firePoint.position, player1);
            Vector3 target = new Vector3(oppPaddle.position.x, transform.position.y, transform.position.z);
            bullet.Launch(bulletHeight, target, bulletGravity, player1);
            AddAmmo(-1);
            paddleSoundBox.cannonSound.PlayOnce();
            if(TutorialManager.instance.isTutorial){
                TutorialManager.instance.cannonLaunch.ClosePrompt(player1);
            }

        }

    }
    private void OnDisable() {
        StopAllCoroutines();
    }
    public void UpdateAmmo()
    {
        if (ammo > 0)
        {
            //fireButton.gameObject.SetActive(true);
        }
        else if (ammo <= 0)
        {
            //fireButton.gameObject.SetActive(true);
        }
        if (player1)
        {
            ammoIndicator.SetText("X" + ammo.ToString());
        }
        else
        {
            ammoIndicator.SetText(ammo.ToString() + "X");
        }

    }
    public void AddAmmo(int amount)
    {
        ammo += amount;
        if (ammo < 0)
        {
            ammo = 0;
        }
        UpdateAmmo();
    }

    

    
    


}

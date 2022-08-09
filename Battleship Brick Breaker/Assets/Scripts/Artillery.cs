using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Artillery : MonoBehaviour
{
    [SerializeField] GameObject inactiveBullets;
    [SerializeField] Transform oppPaddle;
    [SerializeField] Transform firePointLeft;
    [SerializeField] Transform firePointRight;
    [SerializeField] Transform firePoint;
    [SerializeField] bool PC = false;
    [SerializeField] bool canFire = true;

    [SerializeField] float bulletGravity = -10;
    [SerializeField] float bulletHeight = 5f;

    [SerializeField] Button fireButton;
    [SerializeField] TextMeshProUGUI ammoIndicator;
    [SerializeField] GameObject ammoUI;

    [SerializeField] int ammo = 0;

    [SerializeField] bool rightSide;
    float fireDistance;

    public Transform FirePoint { get => firePoint; set => firePoint = value; }
    public bool CanFire { get => canFire; set => canFire = value; }


    // Start is called before the first frame update
    void Start()
    {
        ammoIndicator = ammoUI.GetComponentInChildren<TextMeshProUGUI>();
        //ammo = 99;
        rightSide = GetComponent<BallAim>().Right;
        if(rightSide){
            firePoint = firePointLeft;
            
        }
        else{
            firePoint = firePointRight;
        }
        UpdateAmmo();

    }
    void TestArtillery(){
        ammo++;
        Fire();
    }
    // Update is called once per frame
    void Update()
    {
        if(rightSide && Input.GetKeyDown(KeyCode.Space)){
            TestArtillery();
        }
    }
    public void Fire(){
        if(inactiveBullets.GetComponentsInChildren<Bullet>().Length == 0){
            Debug.Log("No bullets to use");
            return;
        }
        else if(ammo <= 0){
            Debug.Log("no ammo");
            return;
        }
        else if(!canFire){
            return;
        }
        else{
            Bullet bullet = inactiveBullets.GetComponentsInChildren<Bullet>()[0];
            bullet.player1 = rightSide;
            bullet.EnableBullet(firePoint.position);
            Vector3 target = new Vector3(oppPaddle.position.x, transform.position.y, transform.position.z);
            bullet.Launch(bulletHeight, target, bulletGravity);
            AddAmmo(-1);

        }
        
    }
    public void UpdateAmmo(){
        if(ammo > 0){
            fireButton.gameObject.SetActive(true);
        }
        else if(ammo <= 0){
            fireButton.gameObject.SetActive(true);
        }
        if(rightSide){
            ammoIndicator.SetText("X" + ammo.ToString());
        }
        else{
            ammoIndicator.SetText(ammo.ToString() + "X");
        }
        
    }
    public void AddAmmo(int amount){
        ammo += amount;
        if(ammo < 0){
            ammo = 0;
        }
        UpdateAmmo();
    }
}

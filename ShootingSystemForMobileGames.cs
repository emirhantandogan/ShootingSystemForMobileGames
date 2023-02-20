using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public Joystick joystickToShoot;
    private Vector2 directionForShooting;
    public GameObject bulletPrefab;
    public GameObject firePoint;
    public GameObject firePointTriangle;//for visualizing the shooting point.

    private Vector2 shootDirection;
    public float bulletForce;
    public float coolDown;
    public float coolDownLoadSpeed;
    public Slider coolDownSlider;
    public bool touched=true;
    public bool threw=false;
    public bool startofthegame = false;
    public bool touchedbutcooldowntimehasnotexpired = false;


    void Update()
    {
        moveX_public = joystickToShoot.Horizontal;
        moveY_public = joystickToShoot.Vertical;
        directionForShooting = new Vector2(moveX_public , moveY_public).normalized;

        shootDirection = new Vector2(directionForShooting.x, directionForShooting.y);

        Vector2 playerDirection = shootDirection;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        if (moveX_public != 0 && moveY_public != 0)//checking if joysicforshoot is touched or not
        {
            touchedbutcooldowntimehasnotexpired = true;
            firePoint.transform.rotation = targetRotation;
            if (coolDown >= 100)
            {
                touched = true;
                startofthegame = true;
                threw = false;
            }

        }
        else 
        {
            touched = false; //coolDown = 0;
            touchedbutcooldowntimehasnotexpired = false;
        }
            
        //COOLDOWN
        coolDownSlider.value = coolDown;
        if (coolDown >= 100)//I choose the max cooldown value as 100, you can change it from this line. 
        {
            if (touched == false && threw == false && startofthegame == true)
            {
                Shoot();
                threw = true;
                coolDown = 0;
            }
        }
        if (coolDown < 100)
        {
            coolDown += coolDownLoadSpeed * Time.deltaTime;
        }


        //This if statement is optional, if you want to see the triangle for visualizing the shooting direction all times you can delete this part.
        if (touchedbutcooldowntimehasnotexpired == true) 
        {
            firePointTriangle.SetActive(true);
        }
        else
            firePointTriangle.SetActive(false);

    }


    
    //shooting the bullet
    GameObject bulletPrefabF;
    public void Shoot() 
    {
        GameObject bullet = Instantiate(bulletPrefab, firePointTriangle.transform.position, firePointTriangle.transform.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePointTriangle.transform.up * bulletForce, ForceMode2D.Impulse);
    }
}

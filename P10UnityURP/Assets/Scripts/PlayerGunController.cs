using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    public bool isFiring;

    public GoodGuysBullet bullet;
    public float bulletSpeed;

    public float timeBetweenShots;
    private float shotCounter;

    public Transform firePoint;

    // Update is called once per frame
    void Update()
    {
        if(isFiring == true)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots;
                GoodGuysBullet newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as GoodGuysBullet;
                newBullet.speed = bulletSpeed;
            }
        }
        else
        {
            shotCounter = 0;
        }
    }
}

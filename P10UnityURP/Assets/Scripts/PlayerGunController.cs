using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    public bool isFiring = false;

    public GoodGuysBullet bullet;
    //public float bulletSpeed; //Currently 15 in prefab

    public float timeBetweenShots = 0.2f;
    private float shotCounter;

    public Transform firePoint;
    public ParticleSystem muzzleSmoke;
    public int weapon = 0;

    void Start()
    {
        muzzleSmoke = gameObject.transform.Find("SmokeParticles").GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        //SOLUTION 1
        /*
        shotCounter -= Time.deltaTime;
        if(isFiring == true)
        {
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots; // fire rate
                //GoodGuysBullet newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as GoodGuysBullet;
                //newBullet.speed = bulletSpeed;
                WeaponSelect(weapon); // so this now instantiate bullets?
                muzzleSmoke.Play();
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("Gunshot");
                }
            }
        }
        */

        //SOLUTION 2
        if(isFiring == true)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = timeBetweenShots; // fire rate
                //GoodGuysBullet newBullet = Instantiate(bullet, firePoint.position, firePoint.rotation) as GoodGuysBullet;
                //newBullet.speed = bulletSpeed;
                WeaponSelect(weapon); // so this now instantiate bullets?
                muzzleSmoke.Play();
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("Gunshot");
                }
            }
        }
        else
        {
            //shotCounter = 0; // this was what was used before trying to add the if-statement below in an attempt to fix it
            if(shotCounter >= 0)
            {
                shotCounter -= Time.deltaTime;
            }   
        }
    }
    //Switch for choosing what weapon to use - Was used for testing weapon types for Ally
    protected void WeaponSelect (int weaponNumber)
    {
        switch(weaponNumber)
        {
            case 0: //Normal
                GoodGuysBullet DefaultBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                DefaultBullet.damageGiven = 1;
                DefaultBullet.speed = 15f;
                break;
            case 1: //Shotgun
                float spread = 10f;
                int numOfPellets = 10;
                for (int i = 0; i <= numOfPellets; i++)
                {
                    IEnumerator DelayBetweenPellets(float timeBetweenPellets)
                    {
                        yield return new WaitForSeconds(timeBetweenPellets);
                        GoodGuysBullet ShotgunPellet = Instantiate(bullet, firePoint.position, Quaternion.Euler(new Vector3(0, firePoint.transform.eulerAngles.y+Random.Range(-spread, spread), 0)));
                        ShotgunPellet.speed = 15f;
                        ShotgunPellet.damageGiven = 0.2f;
                        ShotgunPellet.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
                        ShotgunPellet.GetComponent<TrailRenderer>().startWidth = 0.05f;  
                        if (i > numOfPellets)
                        {
                            yield break;
                        }                      
                    }
                    StartCoroutine(DelayBetweenPellets(0.01f*i));
                }
                break;
            case 2: //Sniper
                GoodGuysBullet SniperBullet = Instantiate(bullet, firePoint.position, firePoint.rotation);
                SniperBullet.isPiercing = true;
                SniperBullet.damageGiven = 3f;
                SniperBullet.speed = 60f;
                SniperBullet.transform.localScale = new Vector3(0.15f,0.15f,0.5f);
                break;
        }    
    }//WeaponSelect
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunController : MonoBehaviour
{
    public bool isFiring = false;

    public GoodGuysBullet bullet;
    public float bulletSpeed;

    public float timeBetweenShots;
    private float shotCounter;

    public Transform firePoint;
    public ParticleSystem muzzleSmoke;

    void Start()
    {
        muzzleSmoke = gameObject.transform.Find("SmokeParticles").GetComponent<ParticleSystem>();

    }

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
                muzzleSmoke.Play();
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("Gunshot");
                }
            }
        }
        else
        {
            shotCounter = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAim : MonoBehaviour
{

    public bool isFiring;

    public float fireRate;
    public float projectileSpeed;
    public BulletController bullet;
    
    public Transform muzzle;
    
    // Start is called before the first frame update
    void Start()
    {
           
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void shoot()
    {
        if(isFiring == true)
        {
            BulletController newNPCBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
            newNPCBullet.speed = projectileSpeed;
        }
    }
}

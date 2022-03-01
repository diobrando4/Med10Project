using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShooting : MonoBehaviour
{

    public bool isFiring;

    public float fireRate;
    public float projectileSpeed;
    private float shotCounter;

    public BulletController bullet;
    public AllyController script;
    public Transform muzzle;
    
    // Start is called before the first frame update
    void Start()
    {
        script = GetComponent<AllyController>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    void shoot()
    {
        if(isFiring == true)
        {
            shotCounter -= Time.deltaTime;
            if(shotCounter <= 0)
            {
                shotCounter = fireRate;
                BulletController newNPCBullet = Instantiate(bullet, muzzle.position, muzzle.rotation) as BulletController;
                newNPCBullet.speed = projectileSpeed;                
            }
        }
        else
        {
            shotCounter = 0;
        }
    }

/*     public void OnTriggerEnter(Collider col)
    {
        if(col.GetComponent<Collider>().gameObject == script.targetEnemy)
        {
            isFiring = true;
            print("EnemySpotted");
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if(col.GetComponent<Collider>().gameObject == script.targetEnemy)
        {
            isFiring = false;
            print("EnemyLeft");
        }        
    } */
    
}

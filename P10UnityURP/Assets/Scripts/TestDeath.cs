using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDeath : MonoBehaviour
{
    public ParticleSystem deathParticle;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.T))
        {
            Instantiate(deathParticle, transform.position, Quaternion.identity);
            Destroy(gameObject); // this has to be last
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// everytime you instantiate a particle system it makes a clone of itself, 
// so if we don't have this script on the particle system, 
// then the clone of the particle system will never be destroyed, 
// so we would get a bunch of clones in the scene

public class Effect : MonoBehaviour
{
    public float lifeTime; // duration
    public Color origin;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
    void Start()
    {
        if (origin != null)
        {
            gameObject.transform.Find("Cubes").GetComponent<Renderer>().material.color = origin;
        }
    }
}

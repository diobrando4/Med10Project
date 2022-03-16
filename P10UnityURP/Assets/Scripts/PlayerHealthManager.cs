using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public float playerMaxHealth = 3; // should probably rename this into max health
    //[SerializeField]
    public float playerCurrentHealth;

    public Image healthBarFill;

    // can be used for testing and maybe for invincibility frames
    public bool killable = true;

    // Start is called before the first frame update
    void Start()
    {
        // starting with full health
        playerCurrentHealth = playerMaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCurrentHealth <= 0)
        {
            gameObject.SetActive(false);
            //Debug.Log("YOU DIED");
        }
    }

    [SerializeField]
    //private Transform _canvasTransform;

    void LateUpdate()
    {
        //_canvasTransform.LookAt(Camera.main.transform);

        // this is the one we're currently using!
        //_canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void HurtPlayer(float damageTaken)
    {
        if (killable == true)
        {
            playerCurrentHealth -= damageTaken;
            healthBarFill.fillAmount = playerCurrentHealth / playerMaxHealth;
        }
    }
}

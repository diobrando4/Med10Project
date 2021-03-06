using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public float playerMaxHealth = 5; // should probably rename this into max health
    public float playerCurrentHealth;
    public Image healthBarFill;

    //Debuff related
    public bool isDebuffed = false;
    public bool isDebuffable = true;

    public bool isPlayerDead = false;

    // can be used for testing and maybe for invincibility frames
    public bool isPlayerKillable = true;

    //Revive Related
    [HideInInspector] public float reviveMax = 100;
    [HideInInspector] public float reviveCurrent = 0; // has to be reset each time reviving is aborted
    private float reviveRate = 100;
    private Image reviveBarFill;
    [HideInInspector] 
    public bool isBeingRevived = false;
    //[SerializeField]
    //[HideInInspector] 
    public GameObject revivingAlly; //Need to make sure that the revive bar does not reset just because the other ally is passing through
    
    public Material deathIcon;
    public Renderer deathRenderer;
    public Renderer debuffRenderer;
    public ParticleSystem revivePart;
    public ParticleSystem dispelPart;

    // for flashing white whenever they are hurt
    protected List<MeshRenderer> meshRenderer = new List<MeshRenderer>();
    protected List<Color> originalColor = new List<Color>();
    private float flashTime = 0.10f;

    // Start is called before the first frame update
    void Start()
    {
        // starting with full health
        playerCurrentHealth = playerMaxHealth;
        reviveBarFill = gameObject.transform.Find("ReviveBarPopUp/Canvas/ReviveBar/imgBackground/imgFill").GetComponent<Image>();
        //healthBarFill = GameObject.Find("CanvasHealthBars/HolderHealthBars/HolderPlayerHealthBar/imgBackground/imgFillPlayer").GetComponent<Image>();
        healthBarFill = GameObject.Find("CanvasHealthBars/HolderPlayerHealthBar/imgBackground/imgFillPlayer").GetComponent<Image>();
        gameObject.GetComponentInChildren<Image>().enabled = false; //Disable Image comp of Imgbackground on start
        
        deathRenderer = gameObject.transform.Find("StatusIcon").GetComponent<Renderer>();
        deathRenderer.enabled = false;
        debuffRenderer = gameObject.transform.Find("DebuffIcon").GetComponent<Renderer>();
        debuffRenderer.enabled = false;
        
        revivePart = gameObject.transform.Find("ReviveParticles").GetComponent<ParticleSystem>();
        revivePart.Stop();

        // for flashing white whenever they are hurt
        meshRenderer.Add(GetComponent<MeshRenderer>());
        meshRenderer.Add(gameObject.transform.Find("GunCube").GetComponent<MeshRenderer>());  
        for (int i = 0; i < meshRenderer.Count; i++)
        {
            originalColor.Add(meshRenderer[i].material.color);
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if(playerCurrentHealth <= 0)
        {   
                 
            isPlayerDead = true;
            if(reviveCurrent <= 0)
            {
                deathRenderer.enabled = true;     
            }
            else
            {
                deathRenderer.enabled = false;     
            }
            // player movement is disabled in the player controller!
        }
        else
        {
            deathRenderer.enabled = false;     
        }
        //if (revivePart.isEmitting)
        //Debug.Log(revivePart.isEmitting);
    }

    //[SerializeField]
    //private Transform _canvasTransform;

    void LateUpdate()
    {
        //_canvasTransform.LookAt(Camera.main.transform);

        // this is the one we're currently using!
        //_canvasTransform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void HurtPlayer(float damageTaken)
    {
        if (isPlayerKillable == true)
        {
            playerCurrentHealth -= damageTaken;
            healthBarFill.fillAmount = playerCurrentHealth / playerMaxHealth;
            if(isPlayerDead == false)
            {
                // for flashing white whenever they are hurt
                StartCoroutine(Flash());
                CameraShake.instance.StartShake(.2f, .5f); // time, power
            }
            else
            {
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("PlayerAllyDowned");
                }
            }
            if(playerCurrentHealth > 0)
            {
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("PlayerAllyHurt");
                }
            }
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.gameObject.tag == "GoodGuys" && col.gameObject.GetComponent<Ally>().isAllyDead == false)
        {
            if(isPlayerDead == true)
            {
                isBeingRevived = true;
                revivingAlly = col.gameObject;
                gameObject.GetComponentInChildren<Image>().enabled = true;
                revivePart.Play();
                StartCoroutine(RevivePlayer());
            }
        }
    }//OntriggerStay

    private void OnTriggerExit(Collider col)
    {
        //Reset revive bar to 0 if ally exits the radius
        if(col.gameObject == revivingAlly)
        {
            isBeingRevived = false;
            reviveCurrent = 0;
            reviveBarFill.fillAmount = 0;
            gameObject.GetComponentInChildren<Image>().enabled = false;
            revivePart.Stop();
            revivingAlly = null;
        }
    }//OnTriggerExit

    // for flashing white whenever they are hurt
    protected IEnumerator Flash()
    {
        for (int i = 0; i < meshRenderer.Count; i++)
        {
            meshRenderer[i].material.color = Color.white;
        }
        yield return new WaitForSeconds(flashTime);
        for (int i = 0; i < meshRenderer.Count; i++)
        {
            meshRenderer[i].material.color = originalColor[i];
        }
        yield break;
    }

    private IEnumerator RevivePlayer()
    {
        while(reviveCurrent < reviveMax)
        {
            reviveCurrent += reviveMax / reviveRate;
            FindObjectOfType<SoundManager>().SoundRepeatWOInterrupt("ReviveSound");
            //Add ReviveSound on a loop
            if (reviveCurrent >= reviveMax)
            {
                isBeingRevived = false;
                isPlayerDead = false;
                playerCurrentHealth = playerMaxHealth;
                revivingAlly = null;
                reviveCurrent = 0;
                reviveBarFill.fillAmount = 0;
                gameObject.GetComponentInChildren<Image>().enabled = false;
                healthBarFill.fillAmount = playerCurrentHealth / playerMaxHealth;
                revivePart.Stop();
                if (FindObjectOfType<SoundManager>())
                {
                    FindObjectOfType<SoundManager>().SoundPlay("ReviveEnd");
                }
            }
            // fill revive bar here
            reviveBarFill.fillAmount = reviveCurrent / reviveMax;
            yield break; // makes the coroutine stop; when x is no longer inside the trigger, so we don't have to use StopAllCoroutines
        }
    }

    //Coroutine that will make the player immune to further debuffs once an ally has Dispelled previous debuffs
    public IEnumerator DebuffImmunity()
    {
        if (isDebuffable == false)
        {
            debuffRenderer.enabled = false;
            //Debug.Log("Player Immune!");
            yield return new WaitForSeconds(3);
            isDebuffable = true;
            //Debug.Log("Player Not Immune!");
        }
        yield break;
    }//Debuffimmunity

    public void DispelEffect()
    {
        ParticleSystem newDispelPart = Instantiate(dispelPart, transform.position, dispelPart.transform.rotation) as ParticleSystem;
    }
}

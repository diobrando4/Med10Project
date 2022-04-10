using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScreenShake : MonoBehaviour
{
    // version 2
    // singleton
    public static TestScreenShake instance;
    // can be assessed by: 
    // TestScreenShake.instance.StartShake(.2f, .1f); // time, power
    // can set specific values for different things, fx light shake on fire, heavy shake on damage, etc.

    // version 2
    private float shakeTimeRemaining;
    private float shakePower;
    private float shakeFadeTime;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        // version 2
        // this is just for testing
        if (Input.GetKeyDown(KeyCode.U))
        {
            //Debug.Log("pressing U");
            StartShake(1f, .4f); // time, power
        }
    }

    void LateUpdate() // make use lateupdate rather than update?
    {
        // version 1
        // this is just for testing
        if (Input.GetKeyDown(KeyCode.Y))
        {
            //Debug.Log("pressing Y");
            StartCoroutine(Shaking(1f, .4f)); // duration, magnitude
        }

        // version 2
        if (shakeTimeRemaining > 0)
        {
            shakeTimeRemaining -= Time.deltaTime;

            float xAmount = Random.Range(-1f, 1f) * shakePower;
            float yAmount = Random.Range(-1f, 1f) * shakePower;

            transform.position += new Vector3(xAmount, yAmount, 0f);

            shakePower = Mathf.MoveTowards(shakePower, 0f, shakeFadeTime * Time.deltaTime);
        }
    }
    
    // version 2
    public void StartShake(float length, float power)
    {
        shakeTimeRemaining = length;
        shakePower = power;

        shakeFadeTime = power / length;
    }

    // version 1
    public IEnumerator Shaking(float duration, float magnitude)
    {
        // get current position of camera
        //Vector3 originalPosition = transform.localPosition;
        //Vector3 originalPosition = transform.position;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // we only want to shake on x and y axis
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            //transform.localPosition += new Vector3(x, y, originalPosition.z);
            //transform.position += new Vector3(x, y, originalPosition.z);
            transform.position += new Vector3(x, y, 0f);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // reset position of camera
        //transform.localPosition = originalPosition;
        //transform.position = originalPosition;
    }
}

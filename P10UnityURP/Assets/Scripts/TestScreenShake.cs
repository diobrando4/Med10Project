using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// either shove this script into the camera follow script, 
// or call this script from another script

public class TestScreenShake : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // this is just for testing
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Debug.Log("screen shake");

            // version 1
            StartCoroutine(ShakeCamera(.15f, .4f));

            // version 2
        }
    }

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        // get current position of camera
        Vector3 originalPosition = transform.localPosition;

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            // we only want to shake on x and y axis
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPosition.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        // reset position of camera
        transform.localPosition = originalPosition;
    }


}

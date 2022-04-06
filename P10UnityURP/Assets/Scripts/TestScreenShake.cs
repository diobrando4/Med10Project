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
            //Debug.Log("screen shake");

            // version 1
            StartCoroutine(ShakeCamera(.15f, .4f));

            // version 2

        }
    }

    public IEnumerator ShakeCamera(float duration, float magnitude)
    {
        // get current position of camera
        // this might not be a good way of doing this, since the camera might not placed at the right location yet, 
        // becuase another script puts the camera into the correct position, which ight happen at a later point?
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

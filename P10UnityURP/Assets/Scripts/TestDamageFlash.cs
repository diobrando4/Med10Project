using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamageFlash : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    private Color originalColor;

    private float flashTime = 0.10f; // can also be a public if we want to change value in inspector

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        originalColor = meshRenderer.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        // this is just for testing
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(Flash());
        }
    }

    IEnumerator Flash()
    {
        meshRenderer.material.color = Color.white;
        yield return new WaitForSeconds(flashTime);
        meshRenderer.material.color = originalColor;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerPrefsSave : MonoBehaviour
{
    [SerializeField]
    private int versionNum;
    private const string SavedVersion = "SavedVersion";
    // Start is called before the first frame update
    void Start()
    {
        if(PlayerPrefs.HasKey(SavedVersion) == true) //If there already is a saved key
        {
            versionNum = PlayerPrefs.GetInt(SavedVersion); //Use that int as versionNum
            Debug.Log("Existing save found with the number: "+versionNum);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TEST
        if(Input.GetKeyDown(KeyCode.Keypad5))
        {
            versionNum = Random.Range(1,3);
            PlayerPrefs.SetInt(SavedVersion, versionNum); //Set the int versionNum to the SavedVersion Key
            PlayerPrefs.Save(); //Save the changes to registry
            //If used In-Editor - Registry can be found using "RegEdit" with the path being Computer/HKEY_CURRENT_USER/Software/Unity/UnityEditor/DefaultCompany/Med10P1/SavedVersion_xxxxxxxx
            Debug.Log("New version number is: "+versionNum);
        }
        //Check the current versionNumber
        if(Input.GetKeyDown(KeyCode.Keypad4))
        {
            Debug.Log("Version check: "+versionNum);  
        }
        //Delete the saved key if its there. If not, nothing will happen
        if(Input.GetKeyDown(KeyCode.Keypad3))
        {
            PlayerPrefs.DeleteKey(SavedVersion); //Delete the key
            versionNum = 0; //Reset value
            Debug.Log("Deleting SavedVersion");
        }
        if(Input.GetKey(KeyCode.I) &&Input.GetKey(KeyCode.R))
        {
            Debug.Log("Test");
        }
    }
}

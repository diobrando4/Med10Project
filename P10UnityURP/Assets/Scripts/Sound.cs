using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    // bascially this script adds values to the audio source

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [HideInInspector]
    public AudioSource source;
}

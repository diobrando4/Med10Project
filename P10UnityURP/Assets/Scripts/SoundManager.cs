using UnityEngine;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;
    
    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; // to make sure no more code is called if we destroy the object
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.name = s.clip.name;

            s.source.volume = s.volume;
            //s.source.pitch = s.pitch;
        }
    }

    public void SoundPlay(string name) // name of sound to play
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.Log("Sound: " + name + " not found");
            return;
        }

        s.source.Play();
    } // SoundPlay

    public void SoundStop(string name) // name of sound to stop
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);

        if (s == null)
        {
            //Debug.Log("Sound: " + name + " not found");
            return;
        }
        
        s.source.Stop();
    } //SoundStop

    public void SoundRepeatWOInterrupt(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            //Debug.Log("Sound: " + name + " not found");
            return;
        }
        if (s.source.isPlaying == false)
        {
            //Debug.Log("Test");
            s.source.Play();        
        }
    }//SoundRepeatWOInterrupt
}

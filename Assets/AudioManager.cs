using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager audioManager;
    public ClipName[] audioClips;
    Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        audioManager = this;
        foreach (ClipName c in audioClips)
        {
            clipDict.Add(c.name, c.clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Play(string name)
    {
        source.clip = clipDict[name];
        source.Play();
    }
    public static void PlaySound(string name)
    {
        audioManager.Play(name);
    }
}



[Serializable]
public struct ClipName
{
    public AudioClip clip;
    public string name;
}
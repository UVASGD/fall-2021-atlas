using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager audioManager;
    public ClipName[] audioClips;
    Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();
    private AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        audioManager = this;
        foreach (ClipName c in audioClips)
        {
            clipDict.Add(c.name.ToLower(), c.clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public static void PlaySound(string name)
    {
        audioManager.source.clip = audioManager.clipDict[name.ToLower()];
        audioManager.source.Play();

    }

    public static int SoundIdx(string v)
    {
        try
        {
            return audioManager.clipDict[v.ToLower()].name == audioManager.source.clip.name && audioManager.source.isPlaying ? 0 : -1 ;
        } catch (Exception e)
        {
            print(e.Message);
            return -1;
        }
    }
    public static bool IsPlaying(string name)
    {
        return SoundIdx(name) != -1;
    }
    public static void StopSound(string name)
    {
        int idx = SoundIdx(name);
        if (idx != -1)
        {
            audioManager.source.Stop();
        }
    }
}



[Serializable]
public struct ClipName
{
    public AudioClip clip;
    public string name;
}
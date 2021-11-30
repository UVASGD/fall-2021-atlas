using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    static AudioManager audioManager;
    public ClipName[] audioClips;
    Dictionary<string, AudioClip> clipDict = new Dictionary<string, AudioClip>();
    private AudioSource[] sources;
    // Start is called before the first frame update
    void Start()
    {
        sources = GetComponentsInChildren<AudioSource>();
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
    private static AudioSource GetOpenSource()
    {
        for (int i = 0; i < audioManager.sources.Length; i++)
        {
            if (!audioManager.sources[i].isPlaying)
            {
                return audioManager.sources[i];
            }
        }
        return null;
    }

    public static void PlaySound(string name)
    {
        AudioSource freeSource = GetOpenSource();
        freeSource.clip = audioManager.clipDict[name.ToLower()];
        freeSource.Play();

    }

    private static int SoundIdx(string v)
    {
        try
        {

            for (int i = 0; i < audioManager.sources.Length; i++)
            {
                if (audioManager.sources[i].isPlaying && audioManager.sources[i].clip != null && audioManager.sources[i].clip.name == audioManager.clipDict[v.ToLower()].name)
                {
                    return i;
                }
            }
        } catch (Exception e)
        {
            print(e.Message);
        }
        return -1;
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
            audioManager.sources[idx].Stop();
        }
    }
}



[Serializable]
public struct ClipName
{
    public AudioClip clip;
    public string name;
}
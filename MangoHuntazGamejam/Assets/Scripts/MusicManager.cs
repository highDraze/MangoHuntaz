﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {

    private Dictionary<string, AudioClip[]> soundDict;
    private AudioSource audioSource;
    public AudioSource mainLoopSource;
    [Range(0.0f, 1f)]
    public float mainMusicVolume;


    public void Awake()
    {





        soundDict = new Dictionary<string, AudioClip[]>();
        audioSource = GetComponent<AudioSource>();

        soundDict.Add("Clown_Bite_Hit", Clown_Bite_Hit);
        soundDict.Add("Clown_Bite_Miss", Clown_Bite_Miss);
        soundDict.Add("Clown_Block", Clown_Block);
        soundDict.Add("Clown_Hammer_Hit", Clown_Hammer_Hit);
        soundDict.Add("Clown_Hammer_Miss", Clown_Hammer_Miss);
        soundDict.Add("Clown_Hammer_Swing", Clown_Hammer_Swing);
        soundDict.Add("Clown_Laugh_Heavy", Clown_Laugh_Heavy);
        soundDict.Add("Clown_Laugh_Light", Clown_Laugh_Light);
        soundDict.Add("Clown_Laugh_Win", Clown_Laugh_Win);
        soundDict.Add("Clown_Footstep", Clown_Footstep);

        soundDict.Add("SK_Machete_Hit", SK_Machete_Hit);
        soundDict.Add("SK_Machete_Miss", SK_Machete_Miss);
        soundDict.Add("SK_Block", SK_Block);
        soundDict.Add("SK_Scythe_Hit", SK_Scythe_Hit);
        soundDict.Add("SK_Scythe_Miss", SK_Scythe_Miss);
        soundDict.Add("SK_Breath_Heavy", SK_Breath_Heavy);
        soundDict.Add("SK_Breath_Light", SK_Breath_Light);
        soundDict.Add("SK_Breath_Win", SK_Breath_Win);
        soundDict.Add("SK_Footstep", SK_Footstep);

    }


    public AudioClip[] Clown_Bite_Hit;
    public AudioClip[] Clown_Bite_Miss;
    public AudioClip[] Clown_Block;
    public AudioClip[] Clown_Hammer_Hit;
    public AudioClip[] Clown_Hammer_Miss;
    public AudioClip[] Clown_Hammer_Swing;
    public AudioClip[] Clown_Laugh_Heavy;
    public AudioClip[] Clown_Laugh_Light;
    public AudioClip[] Clown_Laugh_Win;
    public AudioClip[] Clown_Footstep;


    public AudioClip[] SK_Machete_Hit;
    public AudioClip[] SK_Machete_Miss;
    public AudioClip[] SK_Block;
    public AudioClip[] SK_Scythe_Hit;
    public AudioClip[] SK_Scythe_Miss;
    public AudioClip[] SK_Breath_Heavy;
    public AudioClip[] SK_Breath_Light;
    public AudioClip[] SK_Breath_Win;
    public AudioClip[] SK_Footstep;

    public AudioClip[] Clown_Wins;
    public AudioClip[] SK_Wins;
    public AudioClip[] Haunt;
    public AudioClip[] Nightmare;
    public AudioClip[] TerrorReign;





    public void PlaySound(string soundName, Vector3 pos)
    {
        if (!soundDict.ContainsKey(soundName))
        {
            Debug.Log("Clip '" + soundName + "' does not exist");
            return;
        }

        if (soundName == null)
        {
            return;
        }

        AudioClip[] arr = soundDict[soundName];
        int index = Random.Range((int)0, (int)arr.Length);

        AudioSource.PlayClipAtPoint(arr[index], pos);
    }

    public void PlaySound(string soundName, Vector3 pos, float volume)
    {
        if (!soundDict.ContainsKey(soundName))
        {
            Debug.Log("Clip '" + soundName + "' does not exist");
            return;
        }

        if (soundName == null)
        {
            return;
        }

        AudioClip[] arr = soundDict[soundName];
        int index = Random.Range((int)0, (int)arr.Length);

        AudioSource.PlayClipAtPoint(arr[index], pos, volume);
    }

    public void PlayTerror()
    {
        audioSource.clip = TerrorReign[0];
        audioSource.Play();
    }

    public void PlayHaunt()
    {
        audioSource.clip = Haunt[0];
        audioSource.Play();
    }

    public void PlayNightmare()
    {
        audioSource.clip = Nightmare[0];
        audioSource.Play();
    }
    public void PlaySKWins()
    {
        audioSource.clip = SK_Wins[0];
        audioSource.Play();
    }

    public void PlayClownWins()
    {
        audioSource.clip = Clown_Wins[0];
        audioSource.Play();
    }


    public void AmpMainLoop()
    {
        mainLoopSource.volume = mainMusicVolume;
    }
    public void DeAmpMainLoop()
    {
        mainLoopSource.volume = 0.02f;
    }
}

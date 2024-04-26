using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource sfxObject;
    public void PlaySFX(AudioClip audioClip)
    {
        AudioSource audioSource = Instantiate(sfxObject);
        audioSource.clip = audioClip;
        audioSource.Play();

        float clipLength = audioClip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySFX(AudioClip audioClip, AudioSource audioSource)
    {
        audioSource.clip = audioClip;
        audioSource.Play();
    }
    
    public void PlaySFX(AudioClip[] audioClips)
    {
        AudioSource audioSource = Instantiate(sfxObject);
        int randomClipIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomClipIndex];
        audioSource.Play();

        float clipLength = audioClips[randomClipIndex].length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
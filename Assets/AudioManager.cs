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
    public void PlaySFX(AudioClip audioClip, bool randomPitch = false)
    {
        AudioSource audioSource = Instantiate(sfxObject, transform.position, Quaternion.identity);
        audioSource.clip = audioClip;

        if (randomPitch) audioSource.pitch = Random.Range(0.85f, 1.3f);
        
        audioSource.Play();
        
        float clipLength = audioClip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public void PlaySFX(AudioClip audioClip, AudioSource audioSource, bool randomPitch = false)
    {
        audioSource.clip = audioClip;
        if (randomPitch) audioSource.pitch = Random.Range(0.85f, 1.3f);
        audioSource.Play();
    }
    
    public void PlaySFX(AudioClip[] audioClips, bool randomPitch = false)
    {
        AudioSource audioSource = Instantiate(sfxObject, transform.position, Quaternion.identity);
        int randomClipIndex = Random.Range(0, audioClips.Length);
        audioSource.clip = audioClips[randomClipIndex];
        if (randomPitch) audioSource.pitch = Random.Range(0.85f, 1.3f);
        audioSource.Play();

        float clipLength = audioClips[randomClipIndex].length;
        Destroy(audioSource.gameObject, clipLength);
    }
}
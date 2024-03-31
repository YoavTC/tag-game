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
    [SerializeField] private float fadeDuration = 0.25f;
    
    
    public void PlaySound(AudioClip audioClip, SoundOptions soundOptions)
    {
        AudioSource audioSource = GetAudioSource(audioClip);
        audioSource.volume = soundOptions.volume;
        audioSource.loop = soundOptions.loop;
        if (soundOptions.randomPitch)
        {
            audioSource.pitch = Random.Range(soundOptions.pitchMinMax.x, soundOptions.pitchMinMax.y);
        } else audioSource.pitch = soundOptions.pitch;
        
        audioSource.PlayDelayed(soundOptions.delay);
    }

    public void PauseSound(AudioClip audioClip, bool fade, Action onPauseComplete)
    {
        AudioSource audioSource = GetAudioSource(audioClip);
        if (fade)
        {
            audioSource.DOFade(0f, fadeDuration).OnComplete(() =>
            {
                audioSource.Pause();
                onPauseComplete?.Invoke();
            });
        } else {
            audioSource.Pause();
        }
    }
    
    public void ResumeSound(AudioClip audioClip, bool fade, float volume = 1f)
    {
        AudioSource audioSource = GetAudioSource(audioClip);
        audioSource.UnPause();
        audioSource.volume = 0;
        
        if (fade)
        {
            audioSource.DOFade(volume, fadeDuration);
        } else {
            audioSource.volume = volume;
        }
    }

    public void StopSound(AudioClip audioClip, bool fade)
    {
        AudioSource audioSource = GetAudioSource(audioClip);
        PauseSound(null, fade, () =>
        {
            Destroy(audioSource);
        });
    }

    private AudioSource GetAudioSource(AudioClip audioClip)
    {
        AudioSource[] audioSources = gameObject.GetComponents<AudioSource>();
        List<AudioSource> deleteLater = new List<AudioSource>();
        
        for (int i = 0; i < audioSources.Length; i++)
        {
            if (!audioSources[i].isPlaying && audioSources[i].volume > 0f)
            {
                deleteLater.Add(audioSources[i]);
            }
            
            if (audioSources[i].clip == audioClip)
            {
                return audioSources[i];
            }
        }

        foreach (var audioSource in deleteLater)
        {
            Destroy(audioSource);
        }
        
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.clip = audioClip;
        return newAudioSource;
    }
}

[Serializable]
public class SoundOptions
{
    public SoundOptions(float volume, float pitch, float delay, bool loop, bool randomPitch, Vector2 pitchMinMax)
    {
        this.volume = volume;
        this.pitch = pitch;
        this.delay = delay;
        this.pitchMinMax = pitchMinMax;
        this.loop = loop;
        this.randomPitch = randomPitch;
    }

    public float volume, pitch, delay;
    public bool loop, randomPitch;
    public Vector2 pitchMinMax;
}

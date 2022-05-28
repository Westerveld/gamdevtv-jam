using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource1, musicSource2, sfxSource;

    public static AudioManager instance;

    private bool lastUsedMusicSource1 = false;

    public AudioClip cookingMusic, datingMusic, runnerMusic, mazeMusic, soulsMusic, turnBasedMusic, twinStickMusic;

    public float lerpSpeed = 0.25f;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void StartGameAudio(GameType type)
    {
        StartCoroutine(LerpOffMusic(lastUsedMusicSource1 ? musicSource1 : musicSource2));
        switch (type)
        {
            case GameType.Cooking:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = cookingMusic;
                }
                else
                {
                    musicSource1.clip = cookingMusic;
                }
                break;
            case GameType.Dating:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = datingMusic;
                }
                else
                {
                    musicSource1.clip = datingMusic;
                }
                break;
            case GameType.Runner:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = runnerMusic;
                }
                else
                {
                    musicSource1.clip = runnerMusic;
                }
                break;
            case GameType.Maze:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = mazeMusic;
                }
                else
                {
                    musicSource1.clip = mazeMusic;
                }
                break;
            case GameType.Souls:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = soulsMusic;
                }
                else
                {
                    musicSource1.clip = soulsMusic;
                }
                break;
            case GameType.TurnBased:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = turnBasedMusic;
                }
                else
                {
                    musicSource1.clip = turnBasedMusic;
                }
                break;
            case GameType.TwinStick:
                if (lastUsedMusicSource1)
                {
                    musicSource2.clip = twinStickMusic;
                }
                else
                {
                    musicSource1.clip = twinStickMusic;
                }
                break;
        }
        StartCoroutine(LerpOnMusic(lastUsedMusicSource1 ?  musicSource2 : musicSource1));
        lastUsedMusicSource1 = !lastUsedMusicSource1;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    IEnumerator LerpOnMusic(AudioSource source)
    {
        source.volume = 0f;
        source.Play();
        while (source.volume < 1f)
        {
            source.volume += Time.fixedDeltaTime * lerpSpeed;
            yield return null;
        }
    }

    IEnumerator LerpOffMusic(AudioSource source)
    {
        while (source.volume > 0f)
        {
            source.volume -= Time.fixedDeltaTime * lerpSpeed;
            yield return null;
        }
        source.Stop();
    }

}

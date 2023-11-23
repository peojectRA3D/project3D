using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource playerSource;
    public AudioSource monsterSource;

    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetEffectVolume(float volume)
    {
        playerSource.volume = volume;
        monsterSource.volume = volume;

    }

    public void OnSfc()
    {
        playerSource.Play();
        monsterSource.Play();
    }
}

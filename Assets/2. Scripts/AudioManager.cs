using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    [Header("#SFX")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex; // channel index

    private float shotgun1Cooldown = 0f;
    private float shotgun1CooldownTime = 0.5f; // 필요에 따라 조절하세요


    public enum Sfx
    {
        dead,
        hit,
        roll,
        heal_sfx,
        swap,
        rifle1,
        rifle2,
        rifle3,
        shotgun1,
        shotgun2,
        shotgun3
    }

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            Init();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    void Init()
    {
        // 효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("sfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].bypassListenerEffects = true;
        }


    }

    public void Playsfx(Sfx sfx)
    {
        if (sfx == Sfx.shotgun1 && Time.time < shotgun1Cooldown)
        {
            return;
        }

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClip[(int)sfx];

            // 소리 크기 조절
            float adjustedVolume = (sfx == Sfx.roll || sfx == Sfx.shotgun3 || sfx == Sfx.hit) ? 0.5f : sfxVolume;
            sfxPlayers[loopIndex].volume = adjustedVolume;

            sfxPlayers[loopIndex].Play();

            if (sfx == Sfx.shotgun1)
            {
                shotgun1Cooldown = Time.time + shotgun1CooldownTime;
            }

            break;
        }
    }

    public void SFXVolume(float volume)
    {
        sfxVolume = volume;
        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index].volume = sfxVolume;
        }
    }
}
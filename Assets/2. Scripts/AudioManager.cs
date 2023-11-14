using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager audio_instance;

    [Header("BGM")]
    public AudioClip bgmClip;
    public float bgmVolume;
    AudioSource bgmPlayer;

    [Header("SFX")]
    public AudioClip[] sfxClips;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayers;
    int channelIndex;

    public enum Sfx
    {   player_dead,
        player_hit,
        player_roll,
        weapon_rifle1,
        stage1_grunt_response,
        dragon_response
    }

    private void Awake()
    {
        if (audio_instance == null)
        {
            audio_instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        Init();
    }

    void Init()
    {
        //배경음 플레이어 초기화
        GameObject bgmObjct = new GameObject("BgmPlayer");
        bgmObjct.transform.parent = transform;
        bgmPlayer = bgmObjct.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmClip;

        //효과음 플레이어 초기화
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayers = new AudioSource[channels];

        for (int index = 0; index < sfxPlayers.Length; index++)
        {
            sfxPlayers[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayers[index].playOnAwake = false;
            sfxPlayers[index].volume = sfxVolume;
        }
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index=0; index< sfxPlayers.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayers.Length;

            if (sfxPlayers[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayers[loopIndex].clip = sfxClips[(int)sfx];
            sfxPlayers[loopIndex].Play();
            break;
        }

       
    }
}

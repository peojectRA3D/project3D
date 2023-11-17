using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 보스 HP
    public Boss boss;
    public RectTransform bossHealth;
    public RectTransform bossHealthBar;

    public PlayerParent player;
    public Text playerCurrentHealth;

    // 반투명 배경
    public RectTransform bg;

    // 옵션
    public RectTransform option;
    bool ispause;

    // 사운드
    public RectTransform sound;

    // 승리 UI
    public Image victory;

    // 패배 UI
    public Image defeat;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (player.PlayerHp <= 0)
                return;
            pause();
        }

    }

    public void pause()
    {
        ispause = !ispause;

        if (!ispause)
        {
            Time.timeScale = 0;
            bg.gameObject.SetActive(true);
            option.gameObject.SetActive(true);
        }
        else if (ispause)
        {
            Time.timeScale = 1;
            bg.gameObject.SetActive(false);
            option.gameObject.SetActive(false);
            sound.gameObject.SetActive(false);
        }
    }

    public void OnClickContinue()
    {
        Time.timeScale = 1;
        bg.gameObject.SetActive(false);
        option.gameObject.SetActive(false);
    }

    public void OnClickSound()
    {
        option.gameObject.SetActive(false);
        sound.gameObject.SetActive(true);
    }

    public void OnClickSoundClose()
    {
        option.gameObject.SetActive(true);
        sound.gameObject.SetActive(false);
    }


    void LateUpdate() // 보스 HP바
    {
        bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);
        playerCurrentHealth.text = player.PlayerHp.ToString();
    }

    public void StageClear() // 승리
    {
        victory.gameObject.SetActive(true);
    }

    public void PlayerDefeat() // 패배
    {
        defeat.gameObject.SetActive(true);
    }

    public void OnClickLobbyButton()
    {
        // "Lobby" 로비로 이동
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickStage1Button()
    {
        // "Stage1" 씬으로 이동
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickStage2Button()
    {
        // "Stage2" 씬으로 이동
        SceneManager.LoadScene("Stage2");
    }
}

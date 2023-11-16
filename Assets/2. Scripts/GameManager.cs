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
    public RectTransform blackBG;

    // 옵션 UI
    public RectTransform Option;
    bool optionToggle = false;

    // 사운드 UI
    public RectTransform Sound;

    // 승리 UI
    public Image victory;

    // 패배 UI
    public Image defeat;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OptionToggle();
        }
    }

    void LateUpdate() // 보스 HP바
    {
        bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);
        playerCurrentHealth.text = player.PlayerHp.ToString();
    }

    public void OptionToggle()
    {
        // optionToggle = !optionToggle;
        // blackBG.gameObject.SetActive(optionToggle);
        // Option.gameObject.SetActive(optionToggle);
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

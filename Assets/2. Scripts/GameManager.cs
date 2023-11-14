using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // 옵션 UI
    public RectTransform optionUI;
    public RectTransform optionSoundUI;

    // 보스 HP
    public Boss boss;
    public RectTransform bossHealth;
    public RectTransform bossHealthBar;

    // 승리 UI
    public Image victory;

    // 패배 UI
    public Image defeat;

    public void OnClickSkipButton() // 스토리 스킵
    {
        SceneManager.LoadScene("Lobby");
    }

    void LateUpdate() // 보스 HP바
    {
        bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);
    }

    public void StageClear() // 승리
    {
        victory.gameObject.SetActive(true);
    }

    public void PlayerDefeat() // 패배
    {
        defeat.gameObject.SetActive(true);
    }

    public void OnClickLobbyButton() // 로비 이동
    {
        SceneManager.LoadScene("Lobby");
    }

    public void onClickCloseOptionUI() // 옵션 닫기
    {
        optionUI.gameObject.SetActive(false);
    }

    public void OnClickStage1Button() // stage1 이동
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickStage2Button() // stage2 이동
    {
        SceneManager.LoadScene("Stage2");
    }

    public void OnClickOption()
    {
        optionUI.gameObject.SetActive(false);
        optionSoundUI.gameObject.SetActive(true);
    }
    public void OnClickOption2()
    {
        optionUI.gameObject.SetActive(true);
        optionSoundUI.gameObject.SetActive(false);
    }

public void OnClickGameQuit() // 게임 종료
    {
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // �ɼ� UI
    public RectTransform optionUI;
    public RectTransform optionSoundUI;

    // ���� HP
    public Boss boss;
    public RectTransform bossHealth;
    public RectTransform bossHealthBar;

    // �¸� UI
    public Image victory;

    // �й� UI
    public Image defeat;

    public void OnClickSkipButton() // ���丮 ��ŵ
    {
        SceneManager.LoadScene("Lobby");
    }

    void LateUpdate() // ���� HP��
    {
        bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);
    }

    public void StageClear() // �¸�
    {
        victory.gameObject.SetActive(true);
    }

    public void PlayerDefeat() // �й�
    {
        defeat.gameObject.SetActive(true);
    }

    public void OnClickLobbyButton() // �κ� �̵�
    {
        SceneManager.LoadScene("Lobby");
    }

    public void onClickCloseOptionUI() // �ɼ� �ݱ�
    {
        optionUI.gameObject.SetActive(false);
    }

    public void OnClickStage1Button() // stage1 �̵�
    {
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickStage2Button() // stage2 �̵�
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

public void OnClickGameQuit() // ���� ����
    {
        Application.Quit();
    }
}

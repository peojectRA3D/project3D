using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // ���� HP
    public Boss boss;
    public RectTransform bossHealth;
    public RectTransform bossHealthBar;

    public PlayerParent player;
    public Text playerCurrentHealth;

    // �¸� UI
    public Image victory;

    // �й� UI
    public Image defeat;

    void LateUpdate() // ���� HP��
    {
        bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);
        playerCurrentHealth.text = player.PlayerHp.ToString();
    }

    public void StageClear() // �¸�
    {
        victory.gameObject.SetActive(true);
    }

    public void PlayerDefeat() // �й�
    {
        defeat.gameObject.SetActive(true);
    }

    public void OnClickLobbyButton()
    {
        // "Lobby" ������ �̵�
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickStage1Button()
    {
        // "Stage1" ������ �̵�
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickStage2Button()
    {
        // "Stage2" ������ �̵�
        SceneManager.LoadScene("Stage2");
    }
}

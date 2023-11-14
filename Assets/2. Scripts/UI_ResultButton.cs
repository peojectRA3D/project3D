using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ResultButton : MonoBehaviour
{
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

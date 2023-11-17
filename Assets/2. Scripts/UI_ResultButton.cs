using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ResultButton : MonoBehaviour
{
    public void OnClickLobbyButton()
    {
        // "Lobby" 씬으로 이동
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickStage1Button()
    {
        // "Stage1" 씬으로 이동
        SceneManager.LoadScene("Stage1(JSH)");
    }

    public void OnClickStage2Button()
    {
        // "Stage2" 씬으로 이동
        SceneManager.LoadScene("Stage2");
    }

    public void QuitGame()
    {
        // 게임 종료
        Application.Quit();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_ResultButton : MonoBehaviour
{
    public void OnClickLobbyButton()
    {
        // "Lobby" æ¿¿∏∑Œ ¿Ãµø
        SceneManager.LoadScene("Lobby");
    }

    public void OnClickStage1Button()
    {
        // "Stage1" æ¿¿∏∑Œ ¿Ãµø
        SceneManager.LoadScene("Stage1");
    }

    public void OnClickStage2Button()
    {
        // "Stage2" æ¿¿∏∑Œ ¿Ãµø
        SceneManager.LoadScene("Stage2");
    }
}

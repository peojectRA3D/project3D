using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainStroy : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.LoadScene("Lobby", LoadSceneMode.Single);
    }
}

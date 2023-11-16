using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Option : MonoBehaviour
{
    public RectTransform bg;
    public RectTransform option;
    public RectTransform sound;
    bool soundToggle = false;

    bool ispause;
    bool pausedown;

    public void Update()
    {
        pausedown = Input.GetButtonDown("Cancel");

        if (Input.GetKeyDown(KeyCode.Escape))
        {

        }

        pause();
    }

    void pause()
    {
        if (pausedown)
        {
            if (!ispause)
            {
                Time.timeScale = 0;

            }
            else
            {
                Time.timeScale = 1;
            }
            ispause = !ispause;
            bg.gameObject.SetActive(ispause);
            option.gameObject.SetActive(ispause);
        }
    }

    public void OnClickContinue()
    {
        option.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    public void OnClickSound()
    {
        soundToggle = !soundToggle;
        sound.gameObject.SetActive(soundToggle);
    }
}

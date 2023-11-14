using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Optionbutton : MonoBehaviour
{
    public mainpanelbutton mainpanel;

    public void closeotionbuttonclick()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainpanel.butttononoff(true);
            closeotionbuttonclick();
        }
    }
}

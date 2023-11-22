using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class optionbutton : MonoBehaviour
{
    public mainpanelbutton mainpanel;

    public void closeoptionbuttonclick()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainpanel.butttononoff(true);
            closeoptionbuttonclick();
        }
    }
}

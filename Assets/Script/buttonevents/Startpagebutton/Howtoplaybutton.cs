using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howtoplaybutton : MonoBehaviour
{

    public mainpanelbutton mainpanel;
    public void closehowtoplaybuttonclick()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainpanel.butttononoff(true);
            closehowtoplaybuttonclick();
        }
    }
}

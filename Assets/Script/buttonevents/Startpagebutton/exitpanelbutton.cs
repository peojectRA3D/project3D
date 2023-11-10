using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitpanelbutton : MonoBehaviour
{
    public mainpanelbutton mainpanel;
    public void yesbuttonclick()
    {
        Application.Quit();
    }
    public void nobuttonclick()
    {
        gameObject.SetActive(false);
    
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainpanel.butttononoff(true);
            nobuttonclick();
        }
    }
}

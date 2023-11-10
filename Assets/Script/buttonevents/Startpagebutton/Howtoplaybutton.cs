using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Howtoplaybutton : MonoBehaviour
{
   
    
    public void closehowtoplaybuttonclick()
    {
        gameObject.SetActive(false);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            closehowtoplaybuttonclick();
        }
    }
}

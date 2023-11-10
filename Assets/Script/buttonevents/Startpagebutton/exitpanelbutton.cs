using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitpanelbutton : MonoBehaviour
{
    
    public void yesbuttonclick()
    {
        Application.Quit();
    }
    public void nobuttonclick()
    {
        gameObject.SetActive(false);
    
    }
}

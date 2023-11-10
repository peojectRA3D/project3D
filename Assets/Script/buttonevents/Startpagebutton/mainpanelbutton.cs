using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainpanelbutton: MonoBehaviour
{
    public GameObject exitpanel;
    public GameObject optionpanel;
    public GameObject howtoplaypanel;
    public Button[] buttons;
    public Text title;
    public void startbuttonclick()
    {
        SceneManager.LoadScene("Default_Scene");
    }
    public void howtoplaybuttononclick()
    {
        butttononoff(false);
    }
    public void optionbuttonclick()
    {
        butttononoff(false);
    }
    public void exitclick()
    {
        butttononoff(false);
        exitpanel.SetActive(true);
      
    }


    public void butttononoff(bool tufal)
    {
        title.gameObject.SetActive(tufal);
        for (int index  = 0; index < buttons.Length;index++)
        {
            buttons[index].gameObject.SetActive(tufal);
        }
    }
 


}

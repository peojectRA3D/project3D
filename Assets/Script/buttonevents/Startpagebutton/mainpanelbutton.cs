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
    public Image titleimage;
    public Image[] imgesbar;
    public void startbuttonclick()
    {
        SceneManager.LoadScene("MainStroyPage");
    }
    public void howtoplaybuttononclick()
    {
        butttononoff(false);
        howtoplaypanel.SetActive(true);
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
        if (tufal == true)
        {
            titleimage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            titleimage.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
        }
        for (int index  = 0; index < buttons.Length;index++)
        {
            buttons[index].gameObject.SetActive(tufal);
        }

        for (int index = 0; index < imgesbar.Length; index++)
        {
            imgesbar[index].gameObject.SetActive(tufal);
        }
    }
 
    


}

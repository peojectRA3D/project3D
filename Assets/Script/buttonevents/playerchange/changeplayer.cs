using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeplayer : MonoBehaviour
{
    public int modeltype;
    public GameObject player;
    public void yesyesyes()
    {
      
        Time.timeScale = 1;
    }
    public void nonono()
    {
        Time.timeScale = 1;
        gameObject.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movesky : MonoBehaviour
{
    public Transform tr;
    public GameObject booms;
    private int count;
    private float delay;
    private float delatdel;
    // Start is called before the first frame update
    void Start()
    {
        delatdel = 0.1f;
        tr = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        delay += Time.deltaTime;
        
        if (count >80)
        {
            
            Destroy(gameObject);
            
        }
        GameObject[] temp = new GameObject[5];
        tr.position += transform.forward.normalized * 1f * Time.timeScale;
        if (delay > delatdel)
        {
            delay = 0;

        }
        else
        {
            return;
        }
        for (int i = 0; i < temp.Length; i++)
        {
            temp[i] = Instantiate(booms);
            count++;
            temp[i].transform.parent = gameObject.transform;
            temp[i].transform.position = tr.position - new Vector3(i * 2.5f - 5f, -2, 0);
            temp[i].transform.parent = null; 
        }
    }
}

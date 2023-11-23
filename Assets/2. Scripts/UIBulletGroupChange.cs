using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBulletGroupChange : MonoBehaviour
{
    ConfigReader configreaders;
    int ModelType;
    int deModelType = 0;
    public GameObject[] bulletgroup;

    void Start()
    {
        configreaders = new ConfigReader("Player");
        ModelType = configreaders.Search<int>("Model");
    }

    void Update()
    {
        configreaders = new ConfigReader("Player");
        ModelType = configreaders.Search<int>("Model");
        
        if (deModelType == ModelType)
        {
          
            return;
        }
        else
        {
            deModelType = ModelType;
        }

        if (ModelType == 0)
        {
            bulletgroup[0].SetActive(true);
            bulletgroup[1].SetActive(false);
            bulletgroup[2].SetActive(false);

        }
        else if (ModelType ==1)
        {
            bulletgroup[0].SetActive(false);
            bulletgroup[1].SetActive(true);
            bulletgroup[2].SetActive(false);
        }
        else if (ModelType == 2)
        {
            bulletgroup[0].SetActive(false);
            bulletgroup[1].SetActive(false);
            bulletgroup[2].SetActive(true);
        }
        else
        {
            bulletgroup[0].SetActive(false);
            bulletgroup[1].SetActive(false);
            bulletgroup[2].SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunlightsettersc : MonoBehaviour
{
    Light mainsun;
    // Start is called before the first frame update
    void Start()
    {
        mainsun = GetComponent<Light>();
        mainsun.shadowStrength = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

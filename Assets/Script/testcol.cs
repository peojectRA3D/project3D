using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testcol : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.tag);
        if (other.tag == "bullet")
        {
            Debug.Log("¾Æ¾ß");
        }
    }
   
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmeraMove : MonoBehaviour
{
    public GameObject player;
    private Vector3 playerpovec; // 11 0 12
    public Transform TR; //1,10,2
    // Start is called before the first frame update
    void Start()
    {
       
        TR = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        playerpovec = player.transform.position;
        TR.position = playerpovec - new Vector3(10 ,-10 , 10);
        
    }
}

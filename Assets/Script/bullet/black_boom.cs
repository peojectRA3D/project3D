using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class black_boom : MonoBehaviour
{
    public GameObject boompub;
    public float speed = 1f;
    float vibrationSpeed = 50f; // 진동 속도
    float vibrationMagnitude = 0.02f; // 진동 크기
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += -transform.forward * speed * Time.timeScale;

        if (speed == 0f)
        {
            float vibration = Mathf.Sin(Time.time * vibrationSpeed) * vibrationMagnitude;
            float vib2 = Mathf.Cos(Time.time * vibrationSpeed) * vibrationMagnitude;
            transform.position += new Vector3(0f, vibration, vib2);
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag =="Enemy")
        {
            speed = 0f;
            Debug.Log("작동!"); 
            //GameObject boom = Instantiate(boompub);
            gameObject.transform.parent = collision.transform;

        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {

        }
    }
}

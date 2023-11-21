using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class movebullet_boom : MonoBehaviour
{
    private float moveSpeed = 60.0f; // 이동 속도 설정
    public ParticleSystem boompaticle;
    Vector3 forwords;
    Vector3 thisplayerpos;
    public bool crush = false;
    public bool crush_viv = false;
    Transform tr;

    void Awake()
        
    {
        tr = GetComponent<Transform>();
    }

    public void getvec(Vector3 vec, Vector3 playerpos)
    {
        tr = GetComponent<Transform>();
        transform.position = playerpos;
        thisplayerpos = vec;
        forwords = vec - playerpos;
        forwords = forwords.normalized;
        tr.rotation = Quaternion.LookRotation(-forwords);// .LookAt(transform.position + forwords);
    }

    void Update()
    {
        // 메시 이동
        transform.position += forwords.normalized * moveSpeed * Time.deltaTime;

        if (crush)
        {
            moveSpeed = 0;
            // Visual Effect 재생
           

            // 일정 시간 후에 메시 파괴
            StartCoroutine(endbullet("endbul", 1f));
            
            crush = !crush;
            crush_viv = true;
        }
    }

   

    public void OnCollisionEnter(Collision collision)
    {
      
        crush = true;
        if (collision.transform.tag == "Enemy")
        {
            
           
            //GameObject boom = Instantiate(boompub);
            gameObject.transform.parent = collision.transform;

        }
    }

    IEnumerator endbullet(string aniname, float delay)
    {
        yield return new WaitForSeconds(delay);
        boompaticle.Play();

        StartCoroutine(endbullet_02("endbul", 0.5f));
    }
   
    IEnumerator endbullet_02(string aniname, float delay)
    {
        yield return new WaitForSeconds(delay);
       
        Destroy(gameObject);
    }
}
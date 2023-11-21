using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class movebullet_viv : MonoBehaviour
{
    private float moveSpeed = 60.0f; // 이동 속도 설정
    public VisualEffect viveff;
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
            viveff.SendEvent("OnPlay");

            // 일정 시간 후에 메시 파괴
            StartCoroutine(endbullet("endbul", 3f));

            crush = !crush;
            crush_viv = true;
        }
    }

    private void FixedUpdate()
    {
        if (crush_viv)
        {
            int spawnRateValue = viveff.GetInt("Spawnrate");
            spawnRateValue -= 50;
            if (spawnRateValue < 2)
            {
                spawnRateValue = 0;
            }
            viveff.SetInt("Spawnrate", spawnRateValue);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        Debug.Log("충돌!");
        crush = true;
    }

    IEnumerator endbullet(string aniname, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
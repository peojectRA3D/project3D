using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movebullet : MonoBehaviour
{
    public float countdownTime = 5.0f;
    private bool isCounting = false;
    public float moveSpeed = 5.0f; // 이동 속도 설정
    public ParticleSystem[] pars;
    public Vector3 forword;
    public Vector3 thisplayerpos;
    // Start is called before the first frame update
    void Start()
    {
        StartCountdown();
    }
   

    public void getvec(Vector3 vec, Vector3 playerpos)
    {
        //Debug.Log(vec);
        //vec += new Vector3(0, 1.5f, 0);
        transform.position = playerpos;
        thisplayerpos = vec;
         forword = vec-playerpos;
    }
    public void Awake()
    {
      
    }
    // Update is called once per frame
    void Update()
    {
      
        if (isCounting)
        {
            countdownTime -= Time.deltaTime;
            
            transform.position += forword.normalized * moveSpeed * Time.deltaTime; 
            if (countdownTime <= 0)
            {
                // 세기가 끝났을 때 실행할 작업을 여기에 추가하십시오.
                //Debug.Log("카운트다운 종료");
                isCounting = false;
                for (int indexer = 0; indexer < pars.Length; indexer++) {
                    if (pars[indexer] == null)
                    {
                         Debug.Log("스킬시스템2 파티클 빔 ");
                    }
                    else
                    {
                        pars[indexer].Stop();
                    }
                }
                Destroy(gameObject);
            }
        }
    }
    public void StartCountdown()
    {
        isCounting = true;
    }
}

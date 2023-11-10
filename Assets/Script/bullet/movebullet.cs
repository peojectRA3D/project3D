using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movebullet : MonoBehaviour
{
    private float countdownTime = 5.0f;
    private bool isCounting = false;
    public float moveSpeed = 5.0f; // �̵� �ӵ� ����
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
        Debug.Log(vec);
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
            
            transform.position += forword.normalized * moveSpeed * Time.deltaTime; ;
            if (countdownTime <= 0)
            {
                // ���Ⱑ ������ �� ������ �۾��� ���⿡ �߰��Ͻʽÿ�.
                Debug.Log("ī��Ʈ�ٿ� ����");
                isCounting = false;
                for (int indexer = 0; indexer < pars.Length; indexer++) {
                    if (pars[indexer] == null)
                    {
                        Debug.Log("��ų�ý���2 ��ƼŬ �� ");
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

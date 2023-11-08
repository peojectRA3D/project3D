using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    public int maxHealth;               // 최대 체력
    public int curHealth;               // 현재 체력
    public Transform target;            // 추적 대상
    public BoxCollider attackArea;      // 근접 공격
    public GameObject earthquake;       // 지진 공격
    public bool isChase;                // 추적 여부
    public bool isAttack;               // 공격 여부
    public bool isCooldown;
    private float coodownTime = 6f;

    Rigidbody rigid;
    NavMeshAgent nav;
    Animator anim;

    // 사운드
    AudioSource audioSource;
    public AudioClip response;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip attack4;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        Invoke("ChaseStart", 2);
    }

    void ChaseStart()
    {
        audioSource.clip = response;
        audioSource.Play();

        isChase = true; // 추적 상태로 변경
        anim.SetBool("isRun", true);
    }

    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position); // 대상의 위치로 이동 목표 설정
            nav.isStopped = !isChase;            // 추적 중이 아닐 때 이동 중지
        }
    }

    void FreezeVelocity() // 물리력이 NavAgent 이동을 방해하지 않는 메서드
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting()
    {
        // 공격 1, 2
        float targetRadius = 4.5f; 
        float targetRange = 3f;   

        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                        targetRadius,
                                                        transform.forward,
                                                        targetRange,
                                                        LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack) 
        {
            StartCoroutine(Attack()); 
        }

        // 공격 3
        if (!isCooldown)
        {
            targetRadius = 9f;
            targetRange = 9f;    

            rayHits = Physics.SphereCastAll(transform.position,
                                                            targetRadius,
                                                            transform.forward,
                                                            targetRange,
                                                            LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack3()); 
            }
        }

        // 공격 4
    }

    IEnumerator Attack()
    {
        isChase = false;         // 추적 중지
        isAttack = true;         // 공격 상태로 설정

        int attackNumber = Random.Range(1, 3);
        if (attackNumber == 1)
        {
            anim.SetBool("isAttack1", true);
            audioSource.clip = attack1;
            audioSource.Play();

        }
        else if (attackNumber == 2)
        {
            anim.SetBool("isAttack2", true);
            audioSource.clip = attack2;
            audioSource.volume = 0.3f;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.8f);
        attackArea.enabled = true;  

        yield return new WaitForSeconds(0.5f);
        attackArea.enabled = false; 

        isChase = true;          
        isAttack = false;        

        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
    }

    IEnumerator Attack3()
    {
        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack3", true);

        yield return new WaitForSeconds(1.55f);
        audioSource.clip = attack3;
        audioSource.volume = 0.4f;
        audioSource.Play();
        earthquake.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        earthquake.SetActive(false);

        isChase = true;
        isAttack = false;

        anim.SetBool("isAttack3", false);

        isCooldown = true;
        yield return new WaitForSeconds(coodownTime);
        isCooldown = false;
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B, C }; // 몬스터 A, B, C 구역
    public Type enemyType;

    public int maxHealth;           // 최대 체력
    public int curHealth;           // 현재 체력
    public Transform target;        // 추적 대상
    public float distance = 20f;    // 감지 범위
    public BoxCollider attackArea;  // 공격 범위
    public bool isChase;            // 추적 여부
    public bool isAttack;           // 공격 여부

    Rigidbody rigid;
    NavMeshAgent nav;
    Animator anim;

    // 사운드
    AudioSource audioSource;
    public AudioClip response;
    public AudioClip attack1;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        Invoke("ChaseStart", 1);
    }

    void ChaseStart()
    {
        isChase = false;
        anim.SetBool("isRun", false);

        /*
        audioSource.clip = response;
        audioSource.volume = 0.1f;
        audioSource.Play();
        */
    }

    void Update()
    {
        if (nav.enabled)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) < distance)
            {
                isChase = true;
                anim.SetBool("isRun", true);

                nav.SetDestination(target.position); // 대상의 위치로 이동 목표 설정
                nav.isStopped = !isChase;            // 추적 중이 아닐 때 이동 중지
            }
        }
    }

    void FreezeVelocity() // 물리력이 NavAgent 이동을 방해하지 않는 메서드
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting() // 타겟을 찾는 메서드
    {
        float targetRadius = 1.5f; // 타겟을 찾을 스피어 캐스트 반지름
        float targetRange = 1f;    // 스피어 캐스트의 범위

        // 플레이어를 탐지하기 위한 스피어 캐스트를 수행
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                        targetRadius,
                                                        transform.forward,
                                                        targetRange,
                                                        LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack) // rayHit 변수에 데이터가 들어오면 공격 코루틴 실행
        {
            StartCoroutine(Attack()); // 공격 코루틴 실행
        }
    }

    IEnumerator Attack()
    {
        isChase = false;         // 추적 중지
        isAttack = true;         // 공격 상태로 설정
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.1f);
        audioSource.clip = attack1;
        audioSource.Play();           // 공격 사운드 재생

        yield return new WaitForSeconds(0.2f);
        attackArea.enabled = true;    // 공격 범위 활성화

        yield return new WaitForSeconds(1f);
        attackArea.enabled = false; // 공격 범위 비활성화

        isChase = true;          // 추적 다시 시작
        isAttack = false;        // 공격 상태 해제
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        Targeting();
        FreezeVelocity();
    }
}

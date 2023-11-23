using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy2 : MonoBehaviour
{
    public enum Type { A, B, C }; // 몬스터 A, B, C
    public Type enemyType;

    public float maxHealth;           // 최대 체력
    public float curHealth;           // 현재 체력
    public Transform target;        // 추적 대상
    public float distance = 25f;    // 감지 범위
    public BoxCollider attackArea;  // 공격 범위
    public bool isChase;            // 추적 여부
    public bool isAttack;           // 공격 여부
    private bool isDead;
    public ParticleSystem crabThorn;

    float targetRadius;     // 타겟을 찾을 스피어 캐스트 반지름
    float targetRange;      // 스피어 캐스트의 범위

    bool isCooldownA;
    float cooldownTimeA = 6f;

    Rigidbody rigid;
    NavMeshAgent nav;
    Animator anim;

    // 사운드
    AudioSource audioSource;
    public AudioClip response;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip die;

    Bullet scriptbullet;
    ConfigReader configreaders;
    private float damageTimer = 0f;
   
    private float damageDuration = 1.5f;
    float takedamagesit;
    void Awake()
    {
        if (enemyType == Type.A)
        {
            configreaders = new ConfigReader("Moster_Type_A");
        }
        else if (enemyType == Type.B)
        {
            configreaders = new ConfigReader("Moster_Type_B");
        }
        else
        {
            configreaders = new ConfigReader("Moster_Type_C");
        }

        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isDead = false;
        scriptbullet = attackArea.GetComponent<Bullet>();

        scriptbullet.damage = configreaders.Search<float>("Damage");
        ChaseStart();
    }

    void ChaseStart()
    {
        /*
        audioSource.clip = response;
        audioSource.volume = 0.1f;
        audioSource.Play();
        */

        isChase = true; // 추적 상태로 변경
        anim.SetBool("isRun", false);
    }

    void Update()
    {
        if (nav.enabled)
        {
            if (target != null && Vector3.Distance(transform.position, target.position) < distance || curHealth < maxHealth)
            {
                nav.SetDestination(target.position);
                nav.isStopped = !isChase;

                anim.SetBool("isRun", true);
            }
        }

        if (isDead)
        {
            StopChasing();
        }
    }

    void FreezeVelocity() // 물리력이 NavAgent 이동을 방해하지 않는 메서드
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting() // 타겟을 찾는 메서드
    {


        if (enemyType == Type.A)
        {
            targetRadius = 1.5f;
            targetRange = 1.3f;
        }
        else
        {
            targetRadius = 1.5f;
            targetRange = 1.7f;
        }

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
        
        if(enemyType == Type.C)
        {
            if (!isCooldownA)
            {
                targetRadius = 3f;
                targetRange = 15f;

                rayHits = Physics.SphereCastAll(transform.position,
                                                                targetRadius,
                                                                transform.forward,
                                                                targetRange,
                                                                LayerMask.GetMask("Player"));

                if (rayHits.Length > 0 && !isAttack)
                {
                    StartCoroutine(Attack2());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        isChase = false;         // 추적 중지
        isAttack = true;         // 공격 상태로 설정

        if(enemyType == Type.A)
        {
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
                audioSource.Play();
            }        
        } else
        {
            anim.SetBool("isAttack1", true);
            audioSource.clip = attack1;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.5f);
        attackArea.enabled = true;    

        yield return new WaitForSeconds(1f);
        attackArea.enabled = false; 

        isChase = true;          
        isAttack = false;        

        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
    }

    IEnumerator Attack2() // Crab 원거리 공격
    {
        isChase = false;         
        isAttack = true;

        anim.SetBool("isAttack2", true);

        yield return new WaitForSeconds(1.0f);
        crabThorn.Play();
        /*
        GameObject instantCrabThorn = Instantiate(crabThorn, crabThornArea.transform.position, transform.rotation);
        Rigidbody rigidCrabThorn = instantCrabThorn.GetComponent<Rigidbody>();
        rigidCrabThorn.velocity = transform.forward * 10;

        Destroy(instantCrabThorn, 5.0f);
        */
        yield return new WaitForSeconds(0.8f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack2", false);

        isCooldownA = true;
        yield return new WaitForSeconds(cooldownTimeA);
        isCooldownA = false;
    }

    void FixedUpdate()
    {
        if (damageTimer < damageDuration)
        {
            damageTimer += Time.fixedDeltaTime;

            // 고정된 간격으로 데미지를 입히는 로직 수행
            curHealth -= takedamagesit;

            if (curHealth <= 0)
            {
                if (isDead)
                    return;

                isChase = false;
                isDead = true;
                anim.SetTrigger("doDie");

                audioSource.clip = die;
                audioSource.volume = 0.1f;
                audioSource.Play();

                Destroy(gameObject, 3f);
            }
        }
        if (!isDead)
        {
            Targeting();
        }
        FreezeVelocity();
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.tag == "bullet")
        {
            curHealth -= other.GetComponent<bulletStatus>().Damage;// other.GetComponent<ParticleSystem>().forceOverLifetime.xMultiplier;

            if (curHealth <= 0)
            {
                if (isDead)
                    return;

                isChase = false;
                isDead = true;
                anim.SetTrigger("doDie");

                audioSource.clip = die;
                audioSource.volume = 0.1f;
                audioSource.Play();

                Destroy(gameObject, 3);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            curHealth -= other.GetComponent<bulletStatus>().Damage; //other.transform.rotation.eulerAngles.y;

            if (curHealth <= 0)
            {
                if (isDead)
                    return;

                isChase = false;
                isDead = true;
                anim.SetTrigger("doDie");

                audioSource.clip = die;
                audioSource.volume = 0.1f;
                audioSource.Play();

                Destroy(gameObject, 3);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "spebullet")
        {
            damageTimer = 0f; // 충돌이 발생했을 때 타이머 초기화
            takedamagesit = collision.transform.GetComponent<bulletStatus>().Damage;
        }
    }
   
    void StopChasing()
    {
        isChase = false;
    }
}


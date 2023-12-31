using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss2 : MonoBehaviour
{
    public float maxHealth;               // 최대 체력
    public float curHealth;               // 현재 체력
    public Transform target;              // 추적 대상
    public float distance = 20f;          // 감지 범위
    public BoxCollider attackArea;        // 근접 공격
    public GameObject[] fireBallPrefabs;  // 파이어볼 공격
    public GameObject[] fireBallSpawner;  // 파이어볼 스폰
    public ParticleSystem flameStream;    // 플레임 공격
    public bool isChase;                  // 추적 여부
    public bool isAttack;                 // 공격 여부
    private bool isDead;

    [SerializeField]
    private Image Victory;
    [SerializeField]
    private Slider HPbar;

    bool isCooldownA;
    bool isCooldownB;
    float cooldownTimeA = 7f;
    float cooldownTimeB = 9f;

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
    public AudioClip die;
    ConfigReader configreaders;
    Bullet scriptbullet;
    private float damageTimer = 0f;
    
    private float damageDuration = 1.5f;
    float takedamagesit;
    void Awake()
    {
        configreaders = new ConfigReader("Boss_Type_One");
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isDead = false;
        scriptbullet = attackArea.GetComponent<Bullet>();
        scriptbullet.damage = configreaders.Search<float>("Damage");

        ChaseStart();

        HPbar.value = (float)curHealth / (float)maxHealth;
    }

    void ChaseStart()
    {
        /*
        audioSource.clip = response;
        audioSource.Play();
        */

        isChase = true; // 추적 상태로 변경
        anim.SetBool("isRun", false);
    }

    void Update()
    {
        if (nav != null && nav.enabled)
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

        HandleHP();
    }

    void HandleHP() // 보스 HP
    {
        HPbar.value = (float)curHealth / (float)maxHealth;
    }

    void FreezeVelocity() // 물리력이 NavAgent 이동을 방해하지 않는 메서드
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting()
    {
        // 공격 1, 2
        float targetRadius = 1.5f;
        float targetRange = 4f;
        
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
                StartCoroutine(Attack3());
            }
        }

        // 공격 4
        if (!isCooldownB)
        {
            targetRadius = 10f;
            targetRange = 18f;

            rayHits = Physics.SphereCastAll(transform.position,
                                                            targetRadius,
                                                            transform.forward,
                                                            targetRange,
                                                            LayerMask.GetMask("Player"));

            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack4());
            }
        }
    }

    IEnumerator Attack()
    {
        if (isDead)
            yield break;

        isChase = false;         // 추적 중지
        isAttack = true;         // 공격 상태로 설정

        int attackNumber = Random.Range(1, 3);
        if (attackNumber == 1)
        {
            anim.SetBool("isAttack1", true);

            yield return new WaitForSeconds(0.2f);
            audioSource.clip = attack1;
            audioSource.volume = 1.0f;
            audioSource.Play();

        }
        else if (attackNumber == 2)
        {
            anim.SetBool("isAttack2", true);
            audioSource.clip = attack2;
            audioSource.volume = 1.0f;
            audioSource.Play();
        }

        yield return new WaitForSeconds(0.4f);
        attackArea.enabled = true;

        yield return new WaitForSeconds(0.7f);
        attackArea.enabled = false;

        isChase = true;
        isAttack = false;

        anim.SetBool("isAttack1", false);
        anim.SetBool("isAttack2", false);
    }

    IEnumerator Attack3()
    {
        if (isDead)
            yield break;

        isChase = false;
        isAttack = true;
        anim.SetBool("isAttack3", true);

        yield return new WaitForSeconds(0.2f);
        audioSource.clip = attack3;
        audioSource.volume = 0.3f;
        audioSource.Play();

        yield return new WaitForSeconds(0.5f);

        Vector3[] directions = new Vector3[] // 각기 다른 파이어볼
        {
            Quaternion.Euler(0, 50, 0) * transform.forward,
            Quaternion.Euler(0, 20, 0) * transform.forward,
            transform.forward,
            Quaternion.Euler(0, -20, 0) * transform.forward,
            Quaternion.Euler(0, -50, 0) * transform.forward
        };

        for (int i = 0; i < fireBallPrefabs.Length; i++)
        {
            GameObject instantFireBall = Instantiate(fireBallPrefabs[i], fireBallSpawner[i].transform.position, transform.rotation);
            Rigidbody rigidFireBall = instantFireBall.GetComponent<Rigidbody>();
            // rigidFireBall.velocity = transform.forward * 15;
            rigidFireBall.velocity = directions[i] * 15;

            Destroy(instantFireBall, 5.0f); // 5초 뒤 파이어볼 삭제

            // 각 파이어볼 생성 사이에 시간 간격을 두기 위해 잠시 대기
            yield return new WaitForSeconds(0.2f); // 예시: 0.2초 간격으로 발사
        }
        /*
        GameObject instantFireBall = Instantiate(fireBall, fireBallSpawner.transform.position, transform.rotation);
        Rigidbody rigidFireBall = instantFireBall.GetComponent<Rigidbody>();
        rigidFireBall.velocity = transform.forward * 20;
        */

        yield return new WaitForSeconds(1.5f);

        isChase = true;
        isAttack = false;

        anim.SetBool("isAttack3", false);

        isCooldownA = true;
        yield return new WaitForSeconds(cooldownTimeA);
        isCooldownA = false;
    }

    IEnumerator Attack4()
    {
        if (isDead)
            yield break;

        isChase = false;
        isAttack = true;
        anim.SetBool("isFly", true);

        yield return new WaitForSeconds(0.02f);
        audioSource.clip = attack4;
        audioSource.volume = 3.5f;
        audioSource.Play();

        yield return new WaitForSeconds(1.0f);
        anim.SetBool("isFly", false);
        anim.SetBool("isAttack4", true);
        flameStream.Play();

        yield return new WaitForSeconds(2.8f);
        isChase = true;
        isAttack = false;
        anim.SetBool("isAttack4", false);

        isCooldownB = true;
        yield return new WaitForSeconds(cooldownTimeB);
        isCooldownB = false;
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
                curHealth = 0;

                if (isDead)
                    return;

                isChase = false;
                isDead = true;
                anim.SetTrigger("doDie");

                audioSource.clip = die;
                audioSource.volume = 1f;
                audioSource.Play();

                Victory.gameObject.SetActive(true);
            }
        }
        if (isChase && !isDead)
        {
            Targeting();
        }
        FreezeVelocity();
    }

    void OnParticleCollision(GameObject other)
    {
        //Debug.Log(other.GetComponent<ParticleSystem>().forceOverLifetime.xMultiplier);
        if (other.tag == "bullet")
        {
            curHealth -= other.GetComponent<bulletStatus>().Damage;

            if (curHealth <= 0)
            {
                curHealth = 0;

                if (isDead)
                    return;

                isChase = false;
                isDead = true;
                anim.SetTrigger("doDie");

                audioSource.clip = die;
                audioSource.volume = 1f;
                audioSource.Play();

                Victory.gameObject.SetActive(true);
            }
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "bullet")
        {
            curHealth -= other.GetComponent<bulletStatus>().Damage;

            if (curHealth <= 0)
            {
                curHealth = 0;

                if (isDead)
                    return;

                isChase = false;
                isDead = true;
                anim.SetTrigger("doDie");

                audioSource.clip = die;
                audioSource.volume = 1f;
                audioSource.Play();

                Victory.gameObject.SetActive(true);
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
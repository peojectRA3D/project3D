using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    public float maxHealth;               // �ִ� ü��
    public float curHealth;               // ���� ü��
    public Transform target;            // ���� ���
    public float distance = 20f;        // ���� ����
    public BoxCollider attackArea;      // ���� ����
    public GameObject earthquake;       // ���� ����
    public GameObject fireBreath;       // �� ����
    public bool isChase;                // ���� ����
    public bool isAttack;               // ���� ����
    private bool isDead;

    [SerializeField]
    private Image Victory;

    bool isCooldownA;
    bool isCooldownB;
    float cooldownTimeA = 6f;
    float cooldownTimeB = 8f;


    Rigidbody rigid;
    NavMeshAgent nav;
    Animator anim;

    // ����
    AudioSource audioSource;
    public AudioClip response;
    public AudioClip attack1;
    public AudioClip attack2;
    public AudioClip attack3;
    public AudioClip attack4;
    public AudioClip die;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        isDead = false;

        ChaseStart();
    }

    void ChaseStart()
    {
        /*
        audioSource.clip = response;
        audioSource.Play();
        */

        isChase = true; // ���� ���·� ����
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

    void FreezeVelocity() // �������� NavAgent �̵��� �������� �ʴ� �޼���
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting()
    {
        // ���� 1, 2
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

        // ���� 3
        if (!isCooldownA)
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

        // ���� 4
        if (!isCooldownB)
        {
            targetRadius = 10f;
            targetRange = 12f;

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

        isChase = false;         // ���� ����
        isAttack = true;         // ���� ���·� ����

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
        if (isDead)
            yield break;

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
        anim.SetBool("isAttack4", true);

        yield return new WaitForSeconds(0.5f);
        audioSource.clip = attack4;
        audioSource.volume = 0.4f;
        audioSource.Play();

        yield return new WaitForSeconds(0.7f);
        fireBreath.SetActive(true);

        yield return new WaitForSeconds(1.5f);
        fireBreath.SetActive(false);

        isChase = true;
        isAttack = false;

        anim.SetBool("isAttack4", false);

        isCooldownB = true;
        yield return new WaitForSeconds(cooldownTimeB);
        isCooldownB = false;
    }

    void FixedUpdate()
    {
        if (isChase && !isDead)
        {
            Targeting();
        }
        FreezeVelocity();
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log(other.GetComponent<ParticleSystem>().forceOverLifetime.xMultiplier);
        if (other.tag == "bullet")
        {
            curHealth -= other.GetComponent<ParticleSystem>().forceOverLifetime.xMultiplier;

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
        Debug.Log(other.transform.rotation.eulerAngles.y);
        // 2�� źȯ - ���� �ǰ�
    }
    void StopChasing()
    {
        isChase = false;
    }
}
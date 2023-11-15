using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type { A, B }; // ���� A, B
    public Type enemyType;

    public float maxHealth;           // �ִ� ü��
    public float curHealth;           // ���� ü��
    public Transform target;        // ���� ���
    public float distance = 25f;    // ���� ����
    public BoxCollider attackArea;  // ���� ����
    public bool isChase;            // ���� ����
    public bool isAttack;           // ���� ����
    private bool isDead;

    float targetRadius;     // Ÿ���� ã�� ���Ǿ� ĳ��Ʈ ������
    float targetRange;      // ���Ǿ� ĳ��Ʈ�� ����

    Rigidbody rigid;
    NavMeshAgent nav;
    Animator anim;

    // ����
    AudioSource audioSource;
    public AudioClip response;
    public AudioClip attack1;
    public AudioClip die;
    Bullet scriptbullet;
    ConfigReader configreaders;
    void Awake()
    {
        if (enemyType == Type.A) {
            configreaders = new ConfigReader("Moster_Type_A");
        }else if (enemyType == Type.B)
        {
            configreaders = new ConfigReader("Moster_Type_B");
        }
        else
        {
            configreaders = new ConfigReader("Moster_Type_A");
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

    void Targeting() // Ÿ���� ã�� �޼���
    {


        if(enemyType == Type.A)
        {
            targetRadius = 1.5f;
            targetRange = 1.3f;
        } else if (enemyType == Type.B)
        {
            targetRadius = 2.5f;
            targetRange = 2.5f;
        }

        // �÷��̾ Ž���ϱ� ���� ���Ǿ� ĳ��Ʈ�� ����
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position,
                                                        targetRadius,
                                                        transform.forward,
                                                        targetRange,
                                                        LayerMask.GetMask("Player"));

        if (rayHits.Length > 0 && !isAttack) // rayHit ������ �����Ͱ� ������ ���� �ڷ�ƾ ����
        {
            StartCoroutine(Attack()); // ���� �ڷ�ƾ ����
        }
    }

    IEnumerator Attack()
    {
        isChase = false;         // ���� ����
        isAttack = true;         // ���� ���·� ����
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(0.1f);
        audioSource.clip = attack1;
        audioSource.Play();           // ���� ���� ���

        yield return new WaitForSeconds(0.2f);
        attackArea.enabled = true;    // ���� ���� Ȱ��ȭ

        yield return new WaitForSeconds(1f);
        attackArea.enabled = false; // ���� ���� ��Ȱ��ȭ

        isChase = true;          // ���� �ٽ� ����
        isAttack = false;        // ���� ���� ����
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
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
            curHealth -= other.GetComponent<ParticleSystem>().forceOverLifetime.xMultiplier;

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
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.transform.rotation.eulerAngles.y);
        // 2�� źȯ - ���� �ǰ�
    }
    void StopChasing()
    {
        isChase = false;
    }
}

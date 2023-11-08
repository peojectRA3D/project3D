using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int maxHealth;       // �ִ� ü��
    public int curHealth;       // ���� ü��
    public Transform target;    // ���� ���
    public BoxCollider attackArea; // ���� ����
    public bool isChase;        // ���� ����
    public bool isAttack;       // ���� ����

    Rigidbody rigid;
    NavMeshAgent nav;
    Animator anim;

    // ����
    AudioSource audioSource;
    public AudioClip response;
    public AudioClip attack1;

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
        audioSource.volume = 0.1f;
        audioSource.Play();

        isChase = true; // ���� ���·� ����
        anim.SetBool("isRun", true);
    }
 
    void Update()
    {
        if (nav.enabled)
        {
            nav.SetDestination(target.position); // ����� ��ġ�� �̵� ��ǥ ����
            nav.isStopped = !isChase;            // ���� ���� �ƴ� �� �̵� ����
        }
    }

    void FreezeVelocity() // �������� NavAgent �̵��� �������� �ʴ� �޼���
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }

    void Targeting() // Ÿ���� ã�� �޼���
    {
        float targetRadius = 1.5f; // Ÿ���� ã�� ���Ǿ� ĳ��Ʈ ������
        float targetRange = 1f;    // ���Ǿ� ĳ��Ʈ�� ����

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
        Targeting();
        FreezeVelocity();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // �⺻ ����
    float hAxis;
    float vAxis;
    public float speed;
    public float jumpPower = 7.0f;
    Vector3 moveVec;

    // ź��, ����, ü��, ����ź
    public int ammo;
    public int coin;
    public int health;
    public int hasGrenade;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenade;

    // Ű �ٿ�
    bool runDown;
    bool jumpDown;

    bool isJump;    // ���� ���� ������ ����
    bool isDamage;  // ������ �Ծ����� ����

    Rigidbody rigid;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        GetInput();
        Move();
        Jump();
    }

    void GetInput()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        runDown = Input.GetButton("Run");
        jumpDown = Input.GetButtonDown("Jump");
    }

    void Move()
    {
        moveVec = new Vector3(hAxis, 0, vAxis).normalized;

        // �޸��� ���¿� ���� �̵��ӵ� ����
        transform.position += moveVec * speed * (runDown ? 1.0f : 0.4f) * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", runDown);

        transform.LookAt(transform.position + moveVec); // �̵� �������� �÷��̾� ȸ��
    }

    void Jump()
    {
        if (jumpDown && !isJump)
        {
            rigid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isJump = true;
        }
    }

    void FreezeRotation() // ȸ�� ���� �����Ӽ� �ʱ�ȭ
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void FixedUpdate()
    {
        FreezeRotation();    
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            anim.SetBool("isJump", false);
            isJump = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHitbox")
        {
            if (!isDamage)
            {
                Bullet enemyBullet = other.GetComponent<Bullet>();
                health -= enemyBullet.damage;

                if (other.GetComponent<Rigidbody>() != null)
                    Destroy(other.gameObject);

                StartCoroutine(OnDamage()); // �������� �Ծ��� ���� ó��
            }
        }
    }
    IEnumerator OnDamage()
    {
        isDamage = true;

        yield return new WaitForSeconds(1f);

        isDamage = false;
    }
}

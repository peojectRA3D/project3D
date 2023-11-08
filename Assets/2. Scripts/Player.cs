using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // 기본 동작
    float hAxis;
    float vAxis;
    public float speed;
    public float jumpPower = 7.0f;
    Vector3 moveVec;

    // 탄약, 동전, 체력, 수류탄
    public int ammo;
    public int coin;
    public int health;
    public int hasGrenade;

    public int maxAmmo;
    public int maxCoin;
    public int maxHealth;
    public int maxHasGrenade;

    // 키 다운
    bool runDown;
    bool jumpDown;

    bool isJump;    // 현재 점프 중인지 여부
    bool isDamage;  // 데미지 입었는지 여부

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

        // 달리기 상태에 따라 이동속도 변경
        transform.position += moveVec * speed * (runDown ? 1.0f : 0.4f) * Time.deltaTime;

        anim.SetBool("isWalk", moveVec != Vector3.zero);
        anim.SetBool("isRun", runDown);

        transform.LookAt(transform.position + moveVec); // 이동 방향으로 플레이어 회전
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

    void FreezeRotation() // 회전 관련 물리속성 초기화
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

                StartCoroutine(OnDamage()); // 데미지를 입었을 때의 처리
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{

    public Transform tr;
    public Rigidbody rid;
    public  GetYZeroInCamera Mousepo;
    public float speed = 5f;
    public float humppower = 5f;
    public float dash = 5f;
    private Vector3 dir = Vector3.zero;
    private Animator aniter;

    public ParticleSystem[] particles;
    // �̵�
    float hAxis;
    float vAxis;
    float jumpPower = 15.0f;

    // Ű�ٿ�
    bool walkDown;
    bool jumpDown;
    bool fireDown;
    bool grenDown;
    bool reloadDown;
    bool itemDown;
    bool swapDown1;
    bool swapDown2;
    bool swapDown3;

    // �ִ�
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder; // �� �浹 �÷��� bool ����
    bool isDamage; // ���� Ÿ���� ���� ����

    Vector3 dodgeVec; // ȸ���ϴ� ���� ������ ������ ���� ����
    float fireDelay;

    // Start is called before the first frame update
    void Start()
    {
        aniter = GetComponent<Animator>();
        rid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        
        GetInput();
        Move();
        Turn();
        Jump();
       
        Attack();
       
        Dodge();
        /*
        Interation();
        Swap();
        Grenade();
        Reload();
        */
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            aniter.SetBool("atttack",!aniter.GetBool("atttack"));
            
        }

      
      

        if (Vector3.Angle(dir, transform.forward) <= 45.0f)
        {
            aniter.SetInteger("vecterval", 0);
        }
        else if  (Vector3.Angle(dir, transform.forward) >= 135.0f) {
            aniter.SetInteger("vecterval",1);
            dir = dir / 2;
        }  
        else
        {
            dir = dir / 1.3f;

            aniter.SetInteger("vecterval", 2);
          
            //aniter.SetInteger("vecterval", 1);
            
        }

        

        if (dir.Equals(Vector3.zero))
        {
            aniter.SetInteger("vecterval", 5);
        }







    }
    void GetInput()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        //walkDown = Input.GetButton("Walk");
        jumpDown = Input.GetButtonDown("Jump");
        fireDown = Input.GetButton("Fire1");
        grenDown = Input.GetButton("Fire2");
        //reloadDown = Input.GetButtonDown("Reload");
        //itemDown = Input.GetButtonDown("Interation");
       // swapDown1 = Input.GetButtonDown("Swap1");
       // swapDown2 = Input.GetButtonDown("Swap2");
       // swapDown3 = Input.GetButtonDown("Swap3");
    }
    void Move()
    {

        float angle = -45.0f;
        angle = Mathf.Deg2Rad * angle;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        dir = Quaternion.AngleAxis(45, Vector3.up) * dir;
        dir = dir.normalized;

        tr.position += dir * speed * Time.deltaTime;

        // moveVec = new Vector3( dir.x, 0,  dir.z ).normalized; // normalized - ���� ���� 1�� ������ ����

        if (isDodge)
            dir = dodgeVec;

        if (isSwap || isReload || !isFireReady)
            dir = Vector3.zero;

        if (!isBorder)
            transform.position += dir * speed * (walkDown ? 0.3f : 1f) * Time.deltaTime; // �ȱ� 0.3f �ӵ�

        //�̰� ���� 
        aniter.SetFloat("speed", dir.magnitude);

        aniter.SetBool("isRun", dir != Vector3.zero);
        aniter.SetBool("isWalk", walkDown);
        //
    }
    void Turn()
    {
        // # 1. Ű���忡 ���� ȸ��
        transform.LookAt(transform.position + dir); // LookAt() - ������ ���͸� ���ؼ� ȸ�������ִ� �Լ�

        // # 2. ���콺�� ���� ȸ��
        if (fireDown) // ���콺 Ŭ�� ���� ���� ȭ���ϵ��� ���� �߰�
        {
            transform.forward = Vector3.Lerp(transform.forward, Mousepo.getMousePosition() - transform.position, Time.deltaTime * speed);
        }
    }
    void Jump()
    {
        if (jumpDown && dir == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            rid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            aniter.SetBool("isJump", true);
            aniter.SetTrigger("doJump");
            isJump = true;
        }
    }
    void Attack()
    {
       
        fireDelay += Time.deltaTime;
        isFireReady = 0.2 < fireDelay;  // equipWeapon.rate < fireDelay;
       
        if (fireDown && isFireReady && !isDodge && !isSwap)
        {
            particles[0].Simulate(1.01f);
            particles[0].Play();
          
            Debug.Log("�߻�ƾ�");
            //�߻� ���� �߰�  equipWeapon.Use();
            //�߻� �ִϸ��̼� �߰�  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
            fireDelay = 0;
           
        }
    }
    void Dodge()
    {
        if (jumpDown && dir != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            dodgeVec = dir;
            speed *= 2;
            aniter.SetTrigger("doDodge");
            isDodge = true;

            Invoke("DodgeOut", 0.4f);
        }
    }

    void DodgeOut()
    {
        speed *= 0.5f;
        isDodge = false;
    }
}

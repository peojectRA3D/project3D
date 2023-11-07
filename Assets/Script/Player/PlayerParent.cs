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
    // 이동
    float hAxis;
    float vAxis;
    float jumpPower = 15.0f;

    // 키다운
    bool walkDown;
    bool jumpDown;
    bool fireDown;
    bool grenDown;
    bool reloadDown;
    bool itemDown;
    bool swapDown1;
    bool swapDown2;
    bool swapDown3;

    // 애니
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isFireReady = true;
    bool isBorder; // 벽 충돌 플래그 bool 변수
    bool isDamage; // 무적 타임을 위한 변수

    Vector3 dodgeVec; // 회피하는 동안 움직임 방지를 위한 변수
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

        // moveVec = new Vector3( dir.x, 0,  dir.z ).normalized; // normalized - 방향 값이 1로 보정된 벡터

        if (isDodge)
            dir = dodgeVec;

        if (isSwap || isReload || !isFireReady)
            dir = Vector3.zero;

        if (!isBorder)
            transform.position += dir * speed * (walkDown ? 0.3f : 1f) * Time.deltaTime; // 걷기 0.3f 속도

        //이거 수정 
        aniter.SetFloat("speed", dir.magnitude);

        aniter.SetBool("isRun", dir != Vector3.zero);
        aniter.SetBool("isWalk", walkDown);
        //
    }
    void Turn()
    {
        // # 1. 키보드에 의한 회전
        transform.LookAt(transform.position + dir); // LookAt() - 지정된 벡터를 향해서 회전시켜주는 함수

        // # 2. 마우스에 의한 회전
        if (fireDown) // 마우스 클릭 했을 때만 화전하도록 조건 추가
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
          
            Debug.Log("발사아아");
            //발사 한줄 추가  equipWeapon.Use();
            //발사 애니메이션 추가  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
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

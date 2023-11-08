using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParent : MonoBehaviour
{

    Transform tr;
    Rigidbody rid;
    public  GetYZeroInCamera Mousepo;
    public float speed = 5f;
    public float humppower = 5f;
    public float dash = 5f;
    private Vector3 dir = Vector3.zero;
    private Animator aniter;
    public int  attacktype;
    public ParticleSystem[] particles;
    // 이동
    float hAxis;
    float vAxis;
    float jumpPower = 15.0f;

    // 키다운
    bool RunDown;
    bool RunUp;
    bool jumpDown;
    bool fireDown;
    bool fireUP;
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
    bool isRun = false;
    bool isFireReady = true;
    bool isBorder; // 벽 충돌 플래그 bool 변수
    bool isDamage; // 무적 타임을 위한 변수

    Vector3 dodgeVec; // 회피하는 동안 움직임 방지를 위한 변수
    float fireDelay;
     public GameObject  gunpos;
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
        Runcheck();
        Move();
        Turn();

        Attack();

        //Jump();       
        Swap();
        Dodge();
        /*
        Interation();
        Grenade();
        Reload();
        */
    }
    void GetInput()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
        RunDown = Input.GetButtonDown("Run");
        RunUp = Input.GetButtonUp("Run");
        jumpDown = Input.GetButtonDown("Jump");
        fireDown = Input.GetButton("Fire1");
        fireUP = Input.GetButtonUp("Fire1");
        grenDown = Input.GetButton("Fire2");
        //reloadDown = Input.GetButtonDown("Reload");
        //itemDown = Input.GetButtonDown("Interation");
        // swapDown1 = Input.GetButtonDown("Swap1");
        // swapDown2 = Input.GetButtonDown("Swap2");
        // swapDown3 = Input.GetButtonDown("Swap3");
    }
    void Runcheck()
    {
        if (RunDown)
        {
            if (!isRun)
            {
                speed = 10.0f;

            }
            else
            {
                speed = 5.0f;
            }
            isRun = !isRun;
        }    
    }
    void Move()
    {

        float angle = -45.0f;
        angle = Mathf.Deg2Rad * angle;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        dir = Quaternion.AngleAxis(45, Vector3.up) * dir;
        dir = dir.normalized;

        if (isDodge)
        {
           
            dir = dodgeVec;
        }
        if (isSwap || isReload )
            dir = Vector3.zero;
        /*
        if (!isBorder)
            transform.position += dir * speed * (walkDown ? 0.3f : 1f) * Time.deltaTime; // 걷기 0.3f 속도
            aniter.SetInteger("vecterval", 5);
        */
        Debug.Log(dir);
        tr.position += dir * speed * Time.deltaTime;
    }
    void Turn()
    {
        // # 1. 키보드에 의한 회전
        // LookAt() - 지정된 벡터를 향해서 회전시켜주는 함수

        // # 2. 마우스에 의한 회전
        if (fireDown) // 마우스 클릭 했을 때만 화전하도록 조건 추가
        {
            transform.forward = Vector3.Lerp(transform.forward, Mousepo.getMousePosition() - transform.position, Time.deltaTime * speed);
        }
        else
        {
            transform.LookAt(transform.position + dir);
        }

        if (Vector3.Angle(dir, transform.forward) <= 45.0f)
        {
            aniter.SetInteger("vecterval", 0);
        }
        else if (Vector3.Angle(dir, transform.forward) >= 135.0f)
        {
            aniter.SetInteger("vecterval", 1);
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

    void Swap()
    {
      /*  
        if (swapDown1 && (!particles[0] || equipWeaponIndex == 0))
            return;
        if (swapDown2 && (!hasWeapons[1] || equipWeaponIndex == 1))
            return;
        if (swapDown3 && (!hasWeapons[2] || equipWeaponIndex == 2))
            return;
        */
        int weaponIndex = -1;
        if (swapDown1) weaponIndex = 0;
        if (swapDown2) weaponIndex = 1;
        if (swapDown3) weaponIndex = 2;

        /*
        if ((swapDown1 || swapDown2 || swapDown3) && !isJump && !isDodge)
        {
            if (equipWeapon != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            anim.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }*/
    }

    void SwapOut()
    {
        isSwap = false;
    }



    /*
    void Jump()
    {
        if (jumpDown && dir == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            rid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            aniter.SetBool("isJump", true);
            aniter.SetTrigger("doJump");
            isJump = true;
        }
    }*/
    void Attack()
    {
       
        fireDelay += Time.deltaTime;
        isFireReady = 0.33< fireDelay;  // equipWeapon.rate < fireDelay;
        
       
        if (fireDown && isFireReady && !isDodge && !isSwap)
        {
            if (attacktype == 0 ) {
                particles[0].Simulate(1.01f);
                particles[0].Play();

                Debug.Log("발사아아");


                aniter.SetBool("onattack", true);
                //발사 한줄 추가  equipWeapon.Use();
                //발사 애니메이션 추가  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                fireDelay = 0;

            }
            else if (attacktype == 1)
            {
               
                ParticleSystem temp =  Instantiate(particles[1]);
                temp.GetComponent<movebullet>().getvec(Mousepo.getMousePosition(),gunpos.transform.position);
                
                aniter.SetBool("onattack", true);
                fireDelay = 0;

            }
            else
            {
                Debug.Log("error 무기 공격 시스템 ");
            }

        }
        else
        {
            aniter.SetBool("onattack", false);
        }
  
    }
    void Dodge()
    {
     

        if (jumpDown && dir != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            dodgeVec = dir;
            speed = 5.0f;
            speed *= 3f;
            aniter.SetTrigger("isroll");
            isDodge = true;

            Invoke("DodgeOut", 0.67f);
        }
    }

    void DodgeOut()
    {
        speed = 5.0f;
        isDodge = false;
        isJump = false;
        isSwap = false;
    }
    
}

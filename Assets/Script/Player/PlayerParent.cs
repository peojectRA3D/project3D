using System;
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
    float jumpPower = 5.0f;

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
    bool isJumpReady = true;
    bool isRun = false;
    bool isFireReady = true;
    bool isSkillReady= true;
    bool isdogeReady = true;
    bool isdead;
    bool isBorder; // 벽 충돌 플래그 bool 변수
    bool isDamage; // 무적 타임을 위한 변수

    Vector3 dodgeVec; // 회피하는 동안 움직임 방지를 위한 변수
    public float PlayerHp = 100.0f;
    float fireDelay;
    float jumpDelay = 6f;
    float dogeDelay = 6f;
    float skillDelay = 6f;
    float dogespeed;
    int[] magcount = {20,5};
    int equipWeaponIndex = 0;
    int weaponIndex = 0;
    public GameObject  gunpos;
    // Start is called before the first frame update

    //효과음
    AudioSource audioSource;
    public AudioClip dead;
    public AudioClip hit;
    public AudioClip roll;
    public AudioClip walk;
    public AudioClip run;
    public AudioClip swap;
    public AudioClip rifle1;

    void Start()
    {
        aniter = GetComponent<Animator>();
        rid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        if (isdead) {
            return;
        }
        GetInput();
        HpCheck();
        Runcheck();
        Move();
        Turn();
        //reload();
        Attack();
        
        Jump();       
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
        jumpDown = Input.GetButton("Jump");
        //reloadDown = Input.GetButtonDown("reload");
        fireDown = Input.GetButton("Fire1");
        fireUP = Input.GetButtonUp("Fire1");
        grenDown = Input.GetButton("Fire2");
        //reloadDown = Input.GetButtonDown("Reload");
        //itemDown = Input.GetButtonDown("Interation");
        swapDown1 = Input.GetButtonDown("Swap1");
        swapDown2 = Input.GetButtonDown("Swap2");
        // swapDown3 = Input.GetButtonDown("Swap3");
    }
    void HpCheck()
    {
        if (PlayerHp <= 0)
        {
            aniter.SetBool("isdie",true);
            isdead = true;

        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "EnemyHitbox" && !isDamage)
        {
            Debug.Log("접촉");
            PlayerHp -= 5;
            isDamage = true;
            StartCoroutine(endaniWithDelay("damage", 0.5f));

            audioSource.clip = hit;
            audioSource.volume = 0.4f;
            audioSource.Play();
        }

        Debug.Log("Collision Detected");

        if (collision.gameObject.CompareTag("CameraSwitch"))
        {
            CameraSwitch cameraSwitch = collision.gameObject.GetComponent<CameraSwitch>();
            if (cameraSwitch != null)
            {
                cameraSwitch.SwitchCamera();
            }
        }
    }
    void Runcheck()
    {
        
        if (RunDown)
        {
                if (!isRun)
                {
                    speed = 10.0f;
                    aniter.SetBool("isrun", true);
                }
                else
                {
                    speed = 5.0f;
                    aniter.SetBool("isrun", false);
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
        if (isJump)
        {
            dir = Vector3.zero;
        }


        // Walk 상태에서 소리를 재생합니다.
       /* if (!isRun && dir != Vector3.zero)
        {
            if (!audioSource.isPlaying || audioSource.clip != walk)
            {
                audioSource.Stop();
                audioSource.clip = walk;
                audioSource.volume = 0.2f;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (isRun && dir != Vector3.zero)
        {
            if (!audioSource.isPlaying || audioSource.clip != run)
            {
                audioSource.Stop();
                audioSource.clip = run;
                audioSource.volume = 0.2f;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else
        {
            // Walk 상태가 아니거나 dir이 zero인 경우에는 소리를 중지합니다.
            audioSource.Stop();
        }*/


        /*
        if (!isBorder)
            transform.position += dir * speed * (walkDown ? 0.3f : 1f) * Time.deltaTime; // 걷기 0.3f 속도
            aniter.SetInteger("vecterval", 5);
        */

        tr.position += dir * speed * Time.deltaTime;
    }
    void Turn()
    {
        // # 1. 키보드에 의한 회전
        // LookAt() - 지정된 벡터를 향해서 회전시켜주는 함수

        // # 2. 마우스에 의한 회전
        if (fireDown && !isDodge) // 마우스 클릭 했을 때만 화전하도록 조건 추가
        {
            transform.forward = Vector3.Lerp(transform.forward, Mousepo.getMousePosition() - transform.position, Time.deltaTime *30f);
       
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
        
        if (swapDown1 && (!particles[0] || weaponIndex == 0))
            return;
        if (swapDown2 && (!particles[1] || weaponIndex == 1))
            return;
        if (swapDown3 && (!particles[2] || weaponIndex == 2))
            return;
        
        
        if (swapDown1) weaponIndex = 0;
        if (swapDown2) weaponIndex = 1;
        if (swapDown3) weaponIndex = 2;

        /*
        if ((swapDown1 || swapDown2 || swapDown3) && !isJump && !isDodge)
        {
            if (equipWeaponIndex != null)
                equipWeapon.gameObject.SetActive(false);

            equipWeaponIndex = weaponIndex;
            equipWeapon = weapons[weaponIndex].GetComponent<Weapon>();
            equipWeapon.gameObject.SetActive(true);

            aniter.SetTrigger("doSwap");

            isSwap = true;

            Invoke("SwapOut", 0.4f);
        }*/
    }
    void SwapOut()
    {
        isSwap = false;
    }
    void Jump()
    {
        jumpDelay += Time.deltaTime;
        isJumpReady = 1.0 < jumpDelay;
       
        if (isJumpReady &&jumpDown && dir == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
            Debug.Log("점프작동 ");
            rid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            //aniter.SetBool("isjump", true);
            aniter.SetTrigger("isjump");
            isJump = true;
            jumpDelay = 0f;
            StartCoroutine(endaniWithDelay("jump", 0.6f));
        }
    }
    void Attack()
    {
       
        fireDelay += Time.deltaTime;
        isFireReady = 0.33< fireDelay;  // equipWeapon.rate < fireDelay;
      
        skillDelay += Time.deltaTime;
        isSkillReady = 5.0f < skillDelay;
        if (fireDown && isFireReady && !isDodge && !isSwap)
        {
            RunDown = true;
            isRun = true;
            Runcheck();
            if (0 == weaponIndex) {
                /*
                if (magcount[0] == 0)
                {
                    return;
                }
               
                magcount[0]--;
                */
                particles[0].Simulate(1.01f);
                particles[0].gameObject.transform.LookAt(Mousepo.gettargetpostion());
                particles[0].gameObject.transform.Rotate(Vector3.up, -90f);
                particles[0].Play();


                audioSource.Stop();
                audioSource.clip = rifle1;
                audioSource.volume = 0.3f;
                audioSource.loop = false;
                audioSource.Play();


                aniter.SetBool("onattack", true);
                //발사 한줄 추가  equipWeapon.Use();
                //발사 애니메이션 추가  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                fireDelay = 0;

                StartCoroutine(endaniWithDelay("onattack", 0.33f));

            }
            else if (1 == weaponIndex )
            {
                if (isSkillReady)
                {

                    ParticleSystem temp = Instantiate(particles[1]);
                    temp.GetComponent<movebullet>().getvec(Mousepo.gettargetpostion(), gunpos.transform.position);
                    aniter.speed = 0.25f;
                    aniter.SetBool("onattack", true);
                    skillDelay = 0;
                    weaponIndex = 0;
                    fireDelay = 0;
                    StartCoroutine(endaniWithDelay("onattack", 1.0f));
                }
                else
                {
                    aniter.SetBool("onattack", true);
                    StartCoroutine(endaniWithDelay("onattack", 0.33f));
                }
            }
            else
            {
                Debug.Log("error 무기 공격 시스템 ");
            }

        }
       
    }
    void reload()
    {
        if (reloadDown) {

            StartCoroutine(endaniWithDelay("reload",0.67f));
         
        }
    }
    void Dodge()
    {
       
        dogeDelay += Time.deltaTime;
        isdogeReady = 5.0 < dogeDelay;
        if (isdogeReady && jumpDown && dir != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
        
            dodgeVec = dir;
            dogespeed = speed;
            speed = 15f;
            aniter.SetTrigger("isroll");
            isDodge = true;
            dogeDelay = 0f;
            StartCoroutine(endaniWithDelay("doge", 0.67f));

            audioSource.Stop();
            audioSource.clip = roll;
            audioSource.volume = 0.3f;
            audioSource.loop = false;
            audioSource.Play();

        }
    }
    IEnumerator endaniWithDelay(string aniname, float delay)
    {
        yield return new WaitForSeconds(delay);
       
       
       
        switch (aniname)
        {
            case "doge":
                isDodge = false;
                speed = dogespeed;  
                break;
            case "onattack":
                aniter.SetBool("onattack", false);
                aniter.speed = 1;
                break;
            case "jump":

                isJump = false;
                break;
            case "reload":
                isSwap = false;
                magcount[0] = 20;
                break;


            case "damage":
                isDamage = false;
                break;
            default:
                // 어떤 경우에도 위의 조건에 해당하지 않을 때의 처리
                break;
        }
    }
}

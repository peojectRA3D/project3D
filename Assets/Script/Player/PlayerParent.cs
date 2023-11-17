using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerParent : MonoBehaviour
{

    Transform tr;
    Rigidbody rid;
    public  GetYZeroInCamera Mousepo;
     float speed;
    float defaultspeed;
    private float RunSpeed;
    public float humppower = 5f;
    public float dash = 5f;
    private Vector3 dir = Vector3.zero;
    private Animator aniter;
    public int  attacktype;
    public ParticleSystem[] particles;
    // �̵�
    float hAxis;
    float vAxis;
    float jumpPower;

    // Ű�ٿ�
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
    bool heal;
    // �ִ�
    bool isJump;
    bool isDodge;
    bool isSwap;
    bool isReload;
    bool isJumpReady = true;
    bool isRun = false;
    bool isFirstSkillReady = true;
    bool isSecondSkillReady= true;
    bool isThirdSkillReady = true;
    bool isFourSkillReady = true;
    bool isdogeReady = true;
    bool isdead;
    bool isBorder; // �� �浹 �÷��� bool ����
    bool isDamage; // ���� Ÿ���� ���� ����
    bool ispause;
    bool healReady;
    bool isstaying;

    bool[] isSkillDamagein= {false ,false };
    bool[] SkillDamageReady= {false, false };
    float[] skillDamageDelay = {0f,0f };
    float[] damagedelayatskill = { 0.5f, 0.1f };
    bool pausedown;

    float FirstSkillDamage;
    float SecondSkillDamage;
    float ThirdSkillDamage;
    float FouredSkillDamage;

    float healPower;

    float FirstSkillDelay;
    float SecondSkillDelay;

    float ThirdSkillDelay;
    float fourSkillDelay;

    Vector3 dodgeVec; // ȸ���ϴ� ���� ������ ������ ���� ����
    public float PlayerHp;
    float MaxHp;
   
    float JumpDelay;
    float DogeDelay;
    float FirstSkillDelay_time;
    float SecondSkillDelay_time;
    float ThirdSkillDelay_time;
    float FourSkillDelay_time;
    float HealDelay;
    float jumpDelayMax;
    float dogeDelayMax;
    float skillDelayMax;
    float HealDelayMax;
    float dogespeed;
    
    int equipWeaponIndex = 0;
    int weaponIndex = 0;
    public GameObject  gunpos;
    public GameObject[] canvas;
    // Start is called before the first frame update

    //ȿ����
    AudioSource audioSource;
    public AudioClip dead;
    public AudioClip hit;
    public AudioClip roll;
    public AudioClip walk;
    public AudioClip run;
    public AudioClip heal_sfx;
    public AudioClip swap;
    public AudioClip rifle1;
    public AudioClip rifle2;

    //���׸���

    public Material[] maters;
    public SkinnedMeshRenderer[] skins;

    float potalDelay = 5f;
    public Text guidetext;

    public RectTransform defeat;

    int ModelType;
    int[] magcount = { 20, 5 };

    private bool isSwitching = false;
    ConfigReader configreaders;
    void Start()
    {
        aniter = GetComponent<Animator>();
        rid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        configreaders = new ConfigReader("Player");
        ModelType = configreaders.Search<int>("Model");
        for (int index = 0; index < skins.Length; index++)
        {
            try
            {
                skins[index].material = maters[ModelType];
            }
            catch
            {
                Debug.LogError("모델 에러  " + ModelType);
            }
        }
        try
        {
            modelsetup();
        }
        catch
        {
            Debug.LogError("모델 에러  " + ModelType);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (isdead) {
            return;
        }
        if (!isSwitching)
        {
            GetInput();
            HpCheck();
            Runcheck();
            Move();
            Turn();
            //reload();
            Attack();
            healplayer();
            Jump();
            Swap();
            Dodge();
            pause();
            skilldamaged();
            
            /*
            Interation();
            Grenade();
            Reload();
            */
        }
    }

    public void SetSwitching(bool value)
    {
        // 카메라 전환 중인지 여부를 설정하는 메소드
        isSwitching = value;
    }
    public void modelsetup()
    {
        configreaders = new ConfigReader("Model_" + ModelType.ToString());

        PlayerHp = configreaders.Search<float>("Hp");
        MaxHp = configreaders.Search<float>("Hp");
        defaultspeed = configreaders.Search<float>("Speed");
        speed = configreaders.Search<float>("Speed");
        RunSpeed = configreaders.Search<float>("RunSpeed");

        FirstSkillDamage = configreaders.Search<float>("FirstSkill");
        SecondSkillDamage = configreaders.Search<float>("SecondSkill");
        ThirdSkillDamage = configreaders.Search<float>("ThirdSkill");
        FouredSkillDamage = configreaders.Search<float>("FouredSkill");

        jumpPower = configreaders.Search<float>("JumpPower");
        dogespeed = configreaders.Search<float>("DogeSpeed");
        healPower = configreaders.Search<float>("HealPower");

        FirstSkillDelay = configreaders.Search<float>("FirstSkillDelay");
        SecondSkillDelay = configreaders.Search<float>("SecondSkillDelay");
        ThirdSkillDelay = configreaders.Search<float>("ThirdSkillDelay");
        fourSkillDelay = configreaders.Search<float>("fourSkillDelay");

        jumpDelayMax = configreaders.Search<float>("JumpDelay");
        dogeDelayMax = configreaders.Search<float>("DogeDelay");
        HealDelayMax = configreaders.Search<float>("HealDelay");

        FirstSkillDelay_time = FirstSkillDelay + 1f;
        SecondSkillDelay_time = SecondSkillDelay + 1f;
        ThirdSkillDelay_time = ThirdSkillDelay + 1f;
        FourSkillDelay_time = fourSkillDelay + 1f;
        JumpDelay = jumpDelayMax + 1f;
        DogeDelay = dogeDelayMax + 1f;
        HealDelay = HealDelayMax + 1f;
    }
    void skilldamaged()
    {
        for (int index = 0; index < isSkillDamagein.Length; index++)
        {
            if (isSkillDamagein[index])
            {
                skillDamageDelay[index] = Time.deltaTime;
            }
            SkillDamageReady[index] = skillDamageDelay[index] > damagedelayatskill[index];
        }
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
        // pausedown = Input.GetButtonDown("Cancel");
        heal = Input.GetButtonDown("heal");
        swapDown3 = Input.GetButtonDown("Swap3");
    }
    void healplayer()
    {
        HealDelay += Time.deltaTime;
        healReady = HealDelay > HealDelayMax;
        if (healReady && heal)
        {
            PlayerHp += healPower;
            if (PlayerHp >= MaxHp)
            {
                PlayerHp = MaxHp;
            }
            HealDelay = 0;
            particles[0].Play();

            audioSource.Stop();
            audioSource.clip = heal_sfx;
            audioSource.volume = 0.3f;
            audioSource.loop = false;
            audioSource.Play();
        }
    }
    
    void pause()
    {
        if (pausedown)
        {
            if (!ispause)
            {
                Time.timeScale = 0;
               
            }
            else
            {
                Time.timeScale = 1;
            }
            ispause = !ispause;
            canvas[0].SetActive(ispause);
            canvas[1].SetActive(ispause);
        }
    }
    void HpCheck()
    {
        if (PlayerHp <= 0)
        {
            aniter.SetBool("isdie",true);
            isdead = true;
            PlayerHp = 0;

            defeat.gameObject.SetActive(true);
        }
    }
    void OnParticleCollision(GameObject other)
    {
        
        if (other.tag == "Breath")
        {
           
          
                takedamge(0.3f);
            
        }
        else if (other.tag == "EnemyMagic"&& !isDamage)
        {
            takedamge(20);
           
           
        }
     
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EnemyHitbox" && !isDamage)
        {
            try
            {
                takedamge(other.GetComponent<Bullet>().damage);
            }
            catch
            {
                Debug.LogError("�Ҹ� ����");
            }
        }
        if (other.tag == "charactorChage")
        {
            for (int index = 0; index < skins.Length; index++)
            {
                try
                {
                    skins[index].material = maters[int.Parse(other.gameObject.name)];
                    configreaders = new ConfigReader("Player");
                    configreaders.UpdateData("Model", other.gameObject.name.ToLower());
                    ModelType = configreaders.Search<int>("Model");
                    modelsetup();
                }
                catch
                {
                    Debug.LogError("이름에러 "+ other.gameObject.name);
                }

            }
        }
        if (other.tag == "Potal")
        {
            guidetext.gameObject.SetActive(true);

        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Potal")
        {
            potalDelay -= Time.deltaTime;
            guidetext.text = "전송까지 앞으로 "+ potalDelay.ToString("F1") + " 초!";
        }
        if (potalDelay < 0)
        {
            SceneManager.LoadScene("Stage1(JSH)");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Potal")
        {
            potalDelay = 5f;
            guidetext.gameObject.SetActive(false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
       
    }
    private void takedamge(float damage)
    {

        PlayerHp -= damage;
        isDamage = true;
        StartCoroutine(endaniWithDelay("damage", 0.3f));

        audioSource.Stop();
        audioSource.clip = hit;
        audioSource.volume = 0.3f;
        audioSource.loop = false;
        audioSource.Play();
    }
    void Runcheck()
    {     
        if (RunDown)
        {
            if (!isRun)
            {
                speed = RunSpeed;
                aniter.SetBool("isrun", true);
            }
            else
            {
                speed = defaultspeed;
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
        {/*
            if(rid.velocity.magnitude > 5f)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 20f;
            }
            else if(rid.velocity.magnitude < 5f)
            {
                GetComponent<Rigidbody>().velocity = GetComponent<Rigidbody>().velocity.normalized * 10f;
            }*/
            dir = dodgeVec;           
        }
        if (isJump || isstaying)
        {
            dir = Vector3.zero;
        }

        // Walk ���¿��� �Ҹ��� ����մϴ�.
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
             // Walk ���°� �ƴϰų� dir�� zero�� ��쿡�� �Ҹ��� �����մϴ�.
             audioSource.Stop();
         }*/
        //if (!isBorder)
        tr.position += dir * speed * Time.deltaTime;
    }
    void Turn()
    {
        // # 1. Ű���忡 ���� ȸ��
        // LookAt() - ������ ���͸� ���ؼ� ȸ�������ִ� �Լ�
        if (ispause || isstaying)
        {
            return;
        }
        // # 2. ���콺�� ���� ȸ��
        if (fireDown && !isDodge) // ���콺 Ŭ�� ���� ���� ȭ���ϵ��� ���� �߰�
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

    void FixedUpdate()
    {
        StopToWall();
    }
    void StopToWall()
    {
        //Debug.DrawRay(transform.position, transform.forward * 3, Color.green); // DrawRay - Scene내에서 Ray를 보여주는 함수
        isBorder = Physics.Raycast(transform.position, transform.forward, 3, LayerMask.GetMask("Wall"));
    }
    void Swap()
    {
        // ���� ���� �ε���
        int previousWeaponIndex = weaponIndex;


        if (swapDown1 && (!particles[1] || weaponIndex == 0))
            return;
        if (swapDown2 && (!particles[2] || weaponIndex == 1))
            return;
        if (swapDown3 && (!particles[3] || weaponIndex == 2))
            return;
        
        
        if (swapDown1) weaponIndex = 0;
        if (swapDown2) weaponIndex = 1;
        if (swapDown3) weaponIndex = 2;

        // ���� ����� ���� ���Ⱑ �ٸ� ���� �Ҹ� ���
        if (previousWeaponIndex != weaponIndex)
        {
            PlayWeaponSwapSound();
        }



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
    void PlayWeaponSwapSound()
    {
        // ���⿡ ���⸦ �ٲ� �� ����� �Ҹ� ��� �ڵ� �߰�
        audioSource.Stop();
        audioSource.clip = swap;
        audioSource.volume = 0.3f;
        audioSource.loop = false;
        audioSource.Play();
    }
    void SwapOut()
    {
        isSwap = false;
    }
    void Jump()
    {
        JumpDelay += Time.deltaTime;
        isJumpReady =jumpDelayMax < JumpDelay;
       
        if (isJumpReady &&jumpDown && dir == Vector3.zero && !isJump && !isDodge && !isSwap)
        {
          
            rid.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
            //aniter.SetBool("isjump", true);
            aniter.SetTrigger("isjump");
            isJump = true;
            JumpDelay = 0f;
            StartCoroutine(endaniWithDelay("jump", 0.6f));
        }
    }
    void Attack()
    {
       
        FirstSkillDelay_time += Time.deltaTime;
        isFirstSkillReady =FirstSkillDelay< FirstSkillDelay_time;  // equipWeapon.rate < fireDelay;
      
        SecondSkillDelay_time += Time.deltaTime;
        isSecondSkillReady = SecondSkillDelay < SecondSkillDelay_time;

        ThirdSkillDelay_time += Time.deltaTime;
        isThirdSkillReady = ThirdSkillDelay < ThirdSkillDelay_time;

        FourSkillDelay_time += Time.deltaTime;
        isFourSkillReady = fourSkillDelay < FourSkillDelay_time;

        if (fireDown && !isDodge && !isSwap)
        {
            RunDown = true;
            isRun = true;
            Runcheck();
            if (ModelType == 0) {
                if (0 == weaponIndex) {
                    if (isFirstSkillReady)
                    {
                        /*
                        if (magcount[0] == 0)
                        {
                            return;
                        }

                        magcount[0]--;
                        */
                        particles[1].Simulate(1.01f);
                        particles[1].gameObject.transform.LookAt(Mousepo.gettargetpostion());
                        particles[1].gameObject.transform.Rotate(Vector3.up, -90f);
                        particles[1].Play();


                        audioSource.Stop();
                        audioSource.clip = rifle1;
                        audioSource.volume = 0.3f;
                        audioSource.loop = false;
                        audioSource.Play();


                        aniter.SetBool("onattack", true);
                        //�߻� ���� �߰�  equipWeapon.Use();
                        //�߻� �ִϸ��̼� �߰�  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                        FirstSkillDelay_time = 0;

                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }
                else if (1 == weaponIndex)
                {
                    if (isSecondSkillReady)
                    {

                        ParticleSystem temp = Instantiate(particles[2]);
                        temp.GetComponent<movebullet>().getvec(Mousepo.gettargetpostion(), gunpos.transform.position);
                        aniter.speed = 0.25f;
                        aniter.SetBool("onattack", true);
                        SecondSkillDelay_time = 0;
                        weaponIndex = 0;
                        FirstSkillDelay_time = 0;
                        StartCoroutine(endaniWithDelay("onattack", 1.0f));

                        audioSource.Stop();
                        audioSource.clip = rifle2;
                        audioSource.volume = 0.3f;
                        audioSource.loop = false;
                        audioSource.Play();
                    }
                    else
                    {
                        aniter.SetBool("onattack", true);
                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }
                else if (2 == weaponIndex)
                {
                    if (isThirdSkillReady)
                    {
                        isstaying = true;
                        //particles[3].Simulate(1.01f);
                        StartCoroutine(endaniWithDelay("sitattack_doit", 0.2f));




                        aniter.SetBool("sitattack", true);

                        ThirdSkillDelay_time = 0;


                    }
                    else
                    {
                        aniter.SetBool("onattack", true);
                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }

                else
                {
                    Debug.Log("error ���� ���� �ý��� ");
                }
            }
            else if (ModelType == 1)
            {
                if (0 == weaponIndex)
                {
                    /*
                    if (magcount[0] == 0)
                    {
                        return;
                    }

                    magcount[0]--;
                    */
                    particles[4].gameObject.transform.LookAt(Mousepo.gettargetpostion());
                    particles[4].gameObject.transform.Rotate(Vector3.up, -90f);
                    particles[4].Play();

                    isstaying = true;
                    audioSource.Stop();
                    audioSource.clip = rifle1;
                    audioSource.volume = 0.3f;
                    audioSource.loop = false;
                    audioSource.Play();


                    aniter.SetBool("onattack", true);
                    //�߻� ���� �߰�  equipWeapon.Use();
                    //�߻� �ִϸ��̼� �߰�  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                    FirstSkillDelay_time = 0;

                    StartCoroutine(endaniWithDelay("onattack_stay", 0.33f));

                }
                else if (1 == weaponIndex)
                {
                    if (isSecondSkillReady)
                    {
                        particles[5].gameObject.transform.LookAt(Mousepo.gettargetpostion());
                        particles[5].Play();
                        Debug.Log("작동");
                        isstaying = true;
                        //aniter.speed = 0.25f;
                        aniter.SetBool("onattack", true);
                        SecondSkillDelay_time = 0;
                        weaponIndex = 0;
                        FirstSkillDelay_time = 0;
                        StartCoroutine(endaniWithDelay("onattack", 1.0f));

                        audioSource.Stop();
                        audioSource.clip = rifle2;
                        audioSource.volume = 0.3f;
                        audioSource.loop = false;
                        audioSource.Play();
                    }
                    else
                    {
                        aniter.SetBool("onattack", true);
                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }
                else if (2 == weaponIndex)
                {
                    if (isThirdSkillReady)
                    {
                        //isstaying = true;
                        //particles[3].Simulate(1.01f);
                        StartCoroutine(endaniWithDelay("sitattack_doit", 0.2f));




                        aniter.SetBool("sitattack", true);

                        ThirdSkillDelay_time = 0;


                    }
                    else
                    {
                        aniter.SetBool("onattack", true);
                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }

                else
                {
                    Debug.Log("error ���� ���� �ý��� ");
                }
            }
            else if (ModelType == 2)
            {
                if (0 == weaponIndex)
                {
                    /*
                    if (magcount[0] == 0)
                    {
                        return;
                    }

                    magcount[0]--;
                    */
                    particles[1].Simulate(1.01f);
                    particles[1].gameObject.transform.LookAt(Mousepo.gettargetpostion());
                    particles[1].gameObject.transform.Rotate(Vector3.up, -90f);
                    particles[1].Play();


                    audioSource.Stop();
                    audioSource.clip = rifle1;
                    audioSource.volume = 0.3f;
                    audioSource.loop = false;
                    audioSource.Play();


                    aniter.SetBool("onattack", true);
                    //�߻� ���� �߰�  equipWeapon.Use();
                    //�߻� �ִϸ��̼� �߰�  ani.SetTrigger(equipWeapon.type == Weapon.Type.Melee ? "doSwing" : "doShot");
                    FirstSkillDelay_time = 0;

                    StartCoroutine(endaniWithDelay("onattack", 0.33f));

                }
                else if (1 == weaponIndex)
                {
                    if (isSecondSkillReady)
                    {

                        ParticleSystem temp = Instantiate(particles[2]);
                        temp.GetComponent<movebullet>().getvec(Mousepo.gettargetpostion(), gunpos.transform.position);
                        aniter.speed = 0.25f;
                        aniter.SetBool("onattack", true);
                        SecondSkillDelay_time = 0;
                        weaponIndex = 0;
                        FirstSkillDelay_time = 0;
                        StartCoroutine(endaniWithDelay("onattack", 1.0f));

                        audioSource.Stop();
                        audioSource.clip = rifle2;
                        audioSource.volume = 0.3f;
                        audioSource.loop = false;
                        audioSource.Play();
                    }
                    else
                    {
                        aniter.SetBool("onattack", true);
                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }
                else if (2 == weaponIndex)
                {
                    if (isThirdSkillReady)
                    {
                        isstaying = true;
                        //particles[3].Simulate(1.01f);
                        StartCoroutine(endaniWithDelay("sitattack_doit", 0.2f));




                        aniter.SetBool("sitattack", true);

                        ThirdSkillDelay_time = 0;


                    }
                    else
                    {
                        aniter.SetBool("onattack", true);
                        StartCoroutine(endaniWithDelay("onattack", 0.33f));
                    }
                }

                else
                {
                    Debug.Log("error ���� ���� �ý��� ");
                }
            }
            else
            {
                Debug.LogError("모델에러_무기");
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
       
        DogeDelay += Time.deltaTime;
        isdogeReady = dogeDelayMax < DogeDelay;
        if (isdogeReady && jumpDown && dir != Vector3.zero && !isJump && !isDodge && !isSwap)
        {
        
            dodgeVec = dir;
            
            speed = dogespeed;  
          
            aniter.SetTrigger("isroll");
            isDodge = true;
            DogeDelay = 0f;

            //rid.AddForce(dir * Time.deltaTime * 500f, ForceMode.VelocityChange);
            
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
                speed = defaultspeed;

                //float dampingFactor =0f; // ���� ������ ���
                //rid.velocity *= dampingFactor;
                break;
            case "onattack":
                aniter.SetBool("onattack", false);
                aniter.speed = 1;
                break;
            case "onattack_stay":
                aniter.SetBool("onattack", false);
                isstaying = false;
                aniter.speed = 1;
                break;
            case "jump":

                isJump = false;
                break;
            case "reload":
                isSwap = false;
                magcount[0] = 20;
                break;
            case "sitattack":
                aniter.SetBool("sitattack", false);
                weaponIndex = 0;
                isstaying = false;
                break;
            case "damage":
                isDamage = false;
                break;
            case "sitattack_doit":
                particles[3].gameObject.transform.LookAt(Mousepo.gettargetpostion());
                //particles[3].gameObject.transform.Rotate(Vector3.up, -90f);
                particles[3].Play();
               
                audioSource.Stop();
                audioSource.clip = rifle1;
                audioSource.volume = 0.3f;
                audioSource.loop = false;
                audioSource.Play();
                StartCoroutine(endaniWithDelay("sitattack", 2f));
                break;
            default:
                // � ��쿡�� ���� ���ǿ� �ش����� ���� ���� ó��
                break;
        }
    }


    public float getrestcool(int Skillnum)
    {
        float returndelay=0f;
        switch (Skillnum)
        {
            case 0:
                returndelay=FirstSkillDelay - FirstSkillDelay_time;
                break;
            case 1:
                returndelay = SecondSkillDelay -SecondSkillDelay_time;
                break;
            case 2:
                returndelay = ThirdSkillDelay - ThirdSkillDelay_time;
                break;
            case 3:
                returndelay = fourSkillDelay-FourSkillDelay_time;
                break;
            case 4:
                returndelay = HealDelayMax - HealDelay;
                break;

            case 5:
                returndelay = dogeDelayMax - DogeDelay;
                break;
            default:
                return returndelay;

        }
        if (returndelay <= 0f)
            returndelay = 0f;
        return returndelay;
    }
 
}

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

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            aniter.SetBool("atttack",!aniter.GetBool("atttack"));
            
        }

        dir.x = Input.GetAxisRaw("Horizontal");
        dir.z = Input.GetAxisRaw("Vertical");
       
        transform.forward = Vector3.Lerp(transform.forward, Mousepo.getMousePosition() - transform.position, Time.deltaTime*speed);
        float angle = -45.0f;
        angle = Mathf.Deg2Rad * angle;
        Quaternion rotation = Quaternion.Euler(0, angle, 0);
        dir = Quaternion.AngleAxis(45, Vector3.up) * dir;
        dir = dir.normalized;
       

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

        tr.position += dir * speed * Time.deltaTime;
        aniter.SetFloat("speed", dir.magnitude);






    }
}

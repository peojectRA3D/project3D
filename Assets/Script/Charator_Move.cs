using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charator_Move : MonoBehaviour
{

    public Transform tr;
    public Rigidbody rid;

    public float speed = 5f;
    public float humppower = 5f;
    public float dash = 5f;
    private Vector3 dir = Vector3.zero;


    // Start is called before the first frame update
    void Start()
    {
        rid = GetComponent<Rigidbody>();
        tr = GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");
        if(dir != Vector3.zero)
        {
            transform.forward = Vector3.Lerp(transform.forward,  (dir+ transform.forward).normalized, Time.deltaTime*speed);

            Debug.Log(transform.forward);
        }
        
        tr.position += dir.normalized * speed * Time.deltaTime; //  .Translate(this.gameObject.transform.position + dir *speed*Time.deltaTime);
    }
}

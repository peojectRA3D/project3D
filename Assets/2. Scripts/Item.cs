using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type { Ammo, Coin, Grenade, Heart, Weapon } // enum - 열거형 타입 (타입 이름 지정 필요)
    public Type type;
    public int value;

    Rigidbody rigid;
    SphereCollider sphereCollider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
    }

    void Update()
    {
        transform.Rotate(Vector3.up * 20 * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision) // 물리효과 변경
    {
        if(collision.gameObject.tag == "Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }    
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    void Start()
    {
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        // ����ź ����
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        // ����ź �ǰ�
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 
                                                        15, Vector3.up, 0f, 
                                                        LayerMask.GetMask("Enemy"));
        // ����ź ���� ������ �ǰ��Լ��� ȣ��
        foreach (RaycastHit hitObj in rayHits) 
        {
            hitObj.transform.GetComponent<Enemy>().HitbyGrenade(transform.position);
        }

        Destroy(gameObject, 4);
    }
}

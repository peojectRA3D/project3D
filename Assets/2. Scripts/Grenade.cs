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
        // 수류탄 폭발
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        // 수류탄 피격
        RaycastHit[] rayHits = Physics.SphereCastAll(transform.position, 
                                                        15, Vector3.up, 0f, 
                                                        LayerMask.GetMask("Enemy"));
        // 수류탄 범위 적들의 피격함수를 호출
        foreach (RaycastHit hitObj in rayHits) 
        {
            hitObj.transform.GetComponent<Enemy>().HitbyGrenade(transform.position);
        }

        Destroy(gameObject, 4);
    }
}

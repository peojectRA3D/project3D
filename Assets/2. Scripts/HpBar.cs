using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject hpBarPrefab = null;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0f, 2f, 0f); // HP 바가 몬스터 머리 위로 얼마나 올라갈지 조절합니다.

    private Transform target; // 몬스터의 위치를 저장할 변수
    private GameObject hpBar; // 생성된 HP 바를 저장할 변수

    void Start()
    {
        target = transform; // 자신(몬스터)의 위치를 초기화

        if (hpBarPrefab != null)
        {
            hpBar = Instantiate(hpBarPrefab, transform.position + hpBarOffset, Quaternion.identity);
            // HP 바 프리팹을 생성하고 몬스터 머리 위에 올립니다.
        }
        else
        {
            Debug.LogError("HP Bar Prefab is not set!");
        }
    }

    void Update()
    {
        if (hpBar != null)
        {
            // 몬스터의 위치를 업데이트
            target = transform;
            // HP 바를 몬스터 머리 위로 따라가게 위치를 업데이트
            hpBar.transform.position = Camera.main.WorldToScreenPoint(target.position + hpBarOffset);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHpBar : MonoBehaviour
{
    public Transform monsterHead; // 몬스터 머리의 위치를 가리키는 Transform
    public GameObject hpBarPrefab; // Hp 프리팹

    private GameObject hpBarInstance; // 생성된 HP 바 인스턴스

    private void Start()
    {
        // Hp 프리팹을 인스턴스화하여 HP 바를 생성
        hpBarInstance = Instantiate(hpBarPrefab, transform);
        // 아래에 위치 조정 및 초기화 로직 추가
    }

    private void Update()
    {
        if (monsterHead == null)
        {
            // 몬스터 머리가 없거나 몬스터가 파괴된 경우 HP 바도 비활성화
            hpBarInstance.SetActive(false);
            return;
        }

        // 몬스터 머리의 월드 좌표를 스크린 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(monsterHead.position);

        // HP 바 위치 업데이트
        hpBarInstance.transform.position = screenPosition;
    }
}




/*
private Camera uiCamera;
private Canvas canvas;
private RectTransform rectParent;
private RectTransform rectHp;

[HideInInspector] public Vector3 offset = Vector3.zero;
[HideInInspector] public Transform targetTR; 

void Start()
{
    canvas = GetComponentInParent<Canvas>();
    uiCamera = canvas.worldCamera;
    rectParent = canvas.GetComponent<RectTransform>();
    rectHp = this.gameObject.GetComponent<RectTransform>();
}

void Update()
{

}

void LateUpdate()
{
    var screenPos = Camera.main.WorldToScreenPoint(targetTR.position + offset);

    if(screenPos.z < 0.0f){
        screenPos *= -1.0f;
    }
}
}

*/

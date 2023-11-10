using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHpBar : MonoBehaviour
{
    public Transform monsterHead; // ���� �Ӹ��� ��ġ�� ����Ű�� Transform
    public GameObject hpBarPrefab; // Hp ������

    private GameObject hpBarInstance; // ������ HP �� �ν��Ͻ�

    private void Start()
    {
        // Hp �������� �ν��Ͻ�ȭ�Ͽ� HP �ٸ� ����
        hpBarInstance = Instantiate(hpBarPrefab, transform);
        // �Ʒ��� ��ġ ���� �� �ʱ�ȭ ���� �߰�
    }

    private void Update()
    {
        if (monsterHead == null)
        {
            // ���� �Ӹ��� ���ų� ���Ͱ� �ı��� ��� HP �ٵ� ��Ȱ��ȭ
            hpBarInstance.SetActive(false);
            return;
        }

        // ���� �Ӹ��� ���� ��ǥ�� ��ũ�� ��ǥ�� ��ȯ
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(monsterHead.position);

        // HP �� ��ġ ������Ʈ
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpBar : MonoBehaviour
{
    [SerializeField] GameObject hpBarPrefab = null;
    [SerializeField] Vector3 hpBarOffset = new Vector3(0f, 2f, 0f); // HP �ٰ� ���� �Ӹ� ���� �󸶳� �ö��� �����մϴ�.

    private Transform target; // ������ ��ġ�� ������ ����
    private GameObject hpBar; // ������ HP �ٸ� ������ ����

    void Start()
    {
        target = transform; // �ڽ�(����)�� ��ġ�� �ʱ�ȭ

        if (hpBarPrefab != null)
        {
            hpBar = Instantiate(hpBarPrefab, transform.position + hpBarOffset, Quaternion.identity);
            // HP �� �������� �����ϰ� ���� �Ӹ� ���� �ø��ϴ�.
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
            // ������ ��ġ�� ������Ʈ
            target = transform;
            // HP �ٸ� ���� �Ӹ� ���� ���󰡰� ��ġ�� ������Ʈ
            hpBar.transform.position = Camera.main.WorldToScreenPoint(target.position + hpBarOffset);
        }
    }
}

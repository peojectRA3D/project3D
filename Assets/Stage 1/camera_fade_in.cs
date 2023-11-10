using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_fade_in : MonoBehaviour
{
    public float fadeDuration = 2f; // ���̵� �ο� �ɸ��� ��ü �ð� (��)
    public Light directionalLight; // ī�޶� ����� �� ������ �ִ� ����Ʈ

    private float currentLerpTime; // ���� ���� �ð�

    void Start()
    {
        // ���� �� ī�޶�� ����Ʈ�� �ʱ� ���·� ����
        Camera.main.backgroundColor = Color.black;
        directionalLight.intensity = 0f;
    }

    void Update()
    {
        // ���� �ð� ����
        currentLerpTime += Time.deltaTime;

        // ���� �ð��� ��ü �ð��� �ʰ��ϸ� ��ü �ð����� ����
        if (currentLerpTime > fadeDuration)
        {
            currentLerpTime = fadeDuration;
        }

        // ���� ���� ���
        float t = currentLerpTime / fadeDuration;

        // ī�޶� ��׶��� ������ �����Ͽ� ���̵� �� ȿ�� ����
        Camera.main.backgroundColor = Color.Lerp(Color.black, Color.white, t);

        // ����Ʈ ������ �����Ͽ� ���̵� �� ȿ�� ����
        directionalLight.intensity = Mathf.Lerp(0f, 1f, t);
    }
}

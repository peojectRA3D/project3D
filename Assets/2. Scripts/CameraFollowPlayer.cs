using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target; // Player
    public Vector3 offset; // ī�޶� ��ǥ �Է�
    public float zoomSpeed = 2.0f; // �� �ӵ� ����
    public float minZoom = 5.0f; // �ּ� �� �Ÿ�
    public float maxZoom = 15.0f; // �ִ� �� �Ÿ�

    private void Update()
    {
        // ���콺 �� ��ũ�� ���� ����
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // ���� ī�޶��� �Ÿ��� ���
        float distance = offset.magnitude;

        // ���ο� �Ÿ��� ���
        distance -= scroll * zoomSpeed;

        // �Ÿ��� �ּ�/�ִ� ������ ����
        distance = Mathf.Clamp(distance, minZoom, maxZoom);

        // ���ο� ī�޶� ��ġ ����
        offset = offset.normalized * distance;

        // �÷��̾� ��ġ�� ���� ī�޶� ��ġ ����
        transform.position = target.position + offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowPlayer : MonoBehaviour
{
    public Transform target; // Player
    public Vector3 offset; // 카메라 좌표 입력
    public float zoomSpeed = 2.0f; // 줌 속도 조절
    public float minZoom = 5.0f; // 최소 줌 거리
    public float maxZoom = 15.0f; // 최대 줌 거리

    private void Update()
    {
        // 마우스 휠 스크롤 값을 얻어옴
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // 현재 카메라의 거리를 계산
        float distance = offset.magnitude;

        // 새로운 거리를 계산
        distance -= scroll * zoomSpeed;

        // 거리를 최소/최대 값으로 제한
        distance = Mathf.Clamp(distance, minZoom, maxZoom);

        // 새로운 카메라 위치 설정
        offset = offset.normalized * distance;

        // 플레이어 위치에 따른 카메라 위치 설정
        transform.position = target.position + offset;
    }
}

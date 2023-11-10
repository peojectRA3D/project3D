using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_fade_in : MonoBehaviour
{
    public float fadeDuration = 2f; // 페이드 인에 걸리는 전체 시간 (초)
    public Light directionalLight; // 카메라가 밝아질 때 영향을 주는 라이트

    private float currentLerpTime; // 현재 보간 시간

    void Start()
    {
        // 시작 시 카메라와 라이트를 초기 상태로 설정
        Camera.main.backgroundColor = Color.black;
        directionalLight.intensity = 0f;
    }

    void Update()
    {
        // 보간 시간 갱신
        currentLerpTime += Time.deltaTime;

        // 보간 시간이 전체 시간을 초과하면 전체 시간으로 고정
        if (currentLerpTime > fadeDuration)
        {
            currentLerpTime = fadeDuration;
        }

        // 보간 비율 계산
        float t = currentLerpTime / fadeDuration;

        // 카메라 백그라운드 색상을 보간하여 페이드 인 효과 생성
        Camera.main.backgroundColor = Color.Lerp(Color.black, Color.white, t);

        // 라이트 강도를 보간하여 페이드 인 효과 생성
        directionalLight.intensity = Mathf.Lerp(0f, 1f, t);
    }
}

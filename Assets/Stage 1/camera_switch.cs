using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class camera_switch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera otherCamera;
    public float switchDuration = 8f;
    public Animator animator; // Animator 컴포넌트 추가
    public PlayableDirector timelineDirector; // PlayableDirector 컴포넌트 추가

    private bool isSwitching = false;
    private float switchTimer = 0f;

    void Start()
    {
        mainCamera.enabled = true;
        otherCamera.enabled = false;
        animator.enabled = false; // 애니메이터 비활성화
        timelineDirector.enabled = false; // PlayableDirector 비활성화
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSwitching)
        {
            SwitchCamera();
        }

        if (isSwitching)
        {
            switchTimer += Time.deltaTime;

            if (switchTimer >= switchDuration)
            {
                SwitchCamera();
            }
        }
    }

    void SwitchCamera()
    {
        if (!isSwitching)
        {
            mainCamera.enabled = !mainCamera.enabled;
            otherCamera.enabled = !otherCamera.enabled;
            isSwitching = true;
            switchTimer = 0f;

            // 애니메이션 시작
            animator.enabled = true;
            animator.SetTrigger("isSwitching"); // "isSwitching"는 애니메이션 클립에 정의한 트리거 이름

            // 타임라인 애니메이션 시작
            timelineDirector.enabled = true;
            timelineDirector.Play(); // 타임라인 재생
        }
        else
        {
            mainCamera.enabled = true;
            otherCamera.enabled = false;
            isSwitching = false;
            animator.enabled = false; // 애니메이터 비활성화
            timelineDirector.enabled = false; // PlayableDirector 비활성화
        }
    }
}

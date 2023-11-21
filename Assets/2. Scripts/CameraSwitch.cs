using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera otherCamera;
    public float switchDuration = 10f;
    public Animator animator; // Animator 컴포넌트 추가
    public PlayableDirector timelineDirector; // PlayableDirector 컴포넌트 추가

    public bool isSwitching = false;
    private bool isSwitched = false; // 추가된 변수

    public float switchTimer = 0f;

    PlayerParent playerParent;

    public void Start()
    {
        mainCamera.enabled = true;
        otherCamera.enabled = false;
        animator.enabled = false; // 애니메이터 비활성화
        timelineDirector.enabled = false; // PlayableDirector 비활성화
    }

    public void Update()
    {
        if (isSwitching)
        {
            switchTimer += Time.deltaTime;

            if (switchTimer >= switchDuration)
            {
                FinishSwitching();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        playerParent = other.GetComponent<PlayerParent>();

        if (playerParent != null)
        {
            SwitchCamera();
        }
    }

    public void SwitchCamera()
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

            // 타임라인 재생
            timelineDirector.enabled = true;
            timelineDirector.Play();

            // 플레이어의 움직임을 제어 (움직이지 않게 설정)
            playerParent.SetSwitching(true);
        }
    }

    public void FinishSwitching()
    {
        mainCamera.enabled = true;
        otherCamera.enabled = false;
        isSwitching = false;
        isSwitched = true; // 전환된 후에는 isSwitched를 true로 설정
        animator.enabled = false;
        timelineDirector.enabled = false;

        // 플레이어의 움직임 제어 해제 (다시 움직일 수 있게 설정)
        playerParent.SetSwitching(false);
    }
}
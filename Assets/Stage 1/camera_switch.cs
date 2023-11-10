using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class camera_switch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera otherCamera;
    public float switchDuration = 8f;
    public Animator animator; // Animator ������Ʈ �߰�
    public PlayableDirector timelineDirector; // PlayableDirector ������Ʈ �߰�

    private bool isSwitching = false;
    private float switchTimer = 0f;

    void Start()
    {
        mainCamera.enabled = true;
        otherCamera.enabled = false;
        animator.enabled = false; // �ִϸ����� ��Ȱ��ȭ
        timelineDirector.enabled = false; // PlayableDirector ��Ȱ��ȭ
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

            // �ִϸ��̼� ����
            animator.enabled = true;
            animator.SetTrigger("isSwitching"); // "isSwitching"�� �ִϸ��̼� Ŭ���� ������ Ʈ���� �̸�

            // Ÿ�Ӷ��� �ִϸ��̼� ����
            timelineDirector.enabled = true;
            timelineDirector.Play(); // Ÿ�Ӷ��� ���
        }
        else
        {
            mainCamera.enabled = true;
            otherCamera.enabled = false;
            isSwitching = false;
            animator.enabled = false; // �ִϸ����� ��Ȱ��ȭ
            timelineDirector.enabled = false; // PlayableDirector ��Ȱ��ȭ
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CameraSwitch : MonoBehaviour
{
    public Camera mainCamera;
    public Camera otherCamera;
    public float switchDuration = 10f;
    public Animator animator; // Animator ������Ʈ �߰�
    public PlayableDirector timelineDirector; // PlayableDirector ������Ʈ �߰�

    public bool isSwitching = false;
    public float switchTimer = 0f;

    PlayerParent playerParent;

    public void Start()
    {
        mainCamera.enabled = true;
        otherCamera.enabled = false;
        animator.enabled = false; // �ִϸ����� ��Ȱ��ȭ
        timelineDirector.enabled = false; // PlayableDirector ��Ȱ��ȭ
    }

    public void Update()
    {
        if (isSwitching)
        {
            switchTimer += Time.deltaTime;

            if (switchTimer >= switchDuration)
            {
                SwitchCamera();
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SwitchCamera();
            GetComponent<Collider>().enabled = false;
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

            // �ִϸ��̼� ����
            animator.enabled = true;
            animator.SetTrigger("isSwitching"); // "isSwitching"�� �ִϸ��̼� Ŭ���� ������ Ʈ���� �̸�

            // Ÿ�Ӷ��� ���
            timelineDirector.enabled = true;
            timelineDirector.Play();

            // �÷��̾��� �������� ���� (�������� �ʰ� ����)
            playerParent.SetSwitching(true);
        }
        else
        {
            mainCamera.enabled = true;
            otherCamera.enabled = false;
            isSwitching = false;
            animator.enabled = false; // �ִϸ����� ��Ȱ��ȭ
            timelineDirector.enabled = false; // PlayableDirector ��Ȱ��ȭ

            // �÷��̾��� ������ ���� ���� (�ٽ� ������ �� �ְ� ����)
            playerParent.SetSwitching(false);
        }
    }
}
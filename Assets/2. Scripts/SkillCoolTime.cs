using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SkillCoolTime : MonoBehaviour
{
    // 각 스킬 버튼, 텍스트, 이미지 등을 담을 배열들
    public GameObject[] hideSkillButtons; // 스킬 버튼들
    public GameObject[] textPros; // 텍스트 요소들
    public Text[] hideSkillTimeTexts; // 스킬 쿨타임 텍스트들
    public Image[] hideSkilIlmages; // 스킬 쿨타임 이미지들

    public GameObject plr;
    private PlayerParent playerParent;

    bool heal;
    bool roll;
    bool healButtonPressed = false;
    bool rollButtonPressed = false;
    private Vector3 dir = Vector3.zero;


    // 각 스킬의 상태를 추적하기 위한 변수들
    private bool[] isHideSkills = { false, false, false, false }; // 각 스킬의 사용 여부를 저장하는 배열
    private float[] skillTimes = { 6, 6, 31, 6 }; // 각 스킬의 쿨타임을 정의하는 배열
    private float[] getSkillTimes = { 0, 0, 0, 0 }; // 각 스킬의 현재 쿨타임을 저장하는 배열

    // Start is called before the first frame update
    void Start()
    {
    
        try
        {
            playerParent = plr.GetComponent<PlayerParent>();
        }
        catch
        {
            Debug.LogError("플레이어 입력 안됨");
        }
    }

    void Update()
    {
        for (int indexer = 1; indexer < hideSkillTimeTexts.Length; indexer++)
        {
            
            hideSkillTimeTexts[indexer].text = playerParent.getrestcool(indexer).ToString("F1");
            if (playerParent.getrestcool(indexer) < 0.1f)
            {
                hideSkillTimeTexts[indexer].text = "";
                hideSkillButtons[indexer].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
            }
            else
            {
                hideSkillButtons[indexer].GetComponent<Image>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 100f / 255f);
            }
        }

        // 공격 2번 쿨타임
        // HideSkillSetting(0);

        // 공격 3번 쿨타임
        // HideSkillSetting(1);
/*
        heal = Input.GetButtonDown("heal"); // 힐 쿨타임
        if (heal && !isHideSkills[2])
        {
            healButtonPressed = true;
            HideSkillSetting(2);
        }
        

        roll = Input.GetButton("Jump"); // 구르기 쿨타임 << 현재는 제자리 점프도 쿨 돌아서 수정 필요
        if (roll && !isHideSkills[3])
        {
            rollButtonPressed = true;
            HideSkillSetting(3);
        }

        HideSkillCheck();
*/
    }
    
    public void HideSkillSetting(int skillNum) // 버튼을 누를 때 호출되는 함수: 해당 스킬의 쿨타임을 시작
    {
        hideSkillButtons[skillNum].SetActive(true); // 해당 스킬 버튼 활성화
        getSkillTimes[skillNum] = skillTimes[skillNum]; // 해당 스킬의 쿨타임을 시작할 값으로 설정
        isHideSkills[skillNum] = true; // 해당 스킬을 사용 중인 상태로 변경
    }

    private void HideSkillCheck() // 각 스킬의 쿨타임을 확인하는 함수
    {
        if (isHideSkills[0])
        {
            StartCoroutine(SkillTimeCheck(0));
        }

        if (isHideSkills[1])
        {
            StartCoroutine(SkillTimeCheck(1));
        }

        if (isHideSkills[2])
        {
            StartCoroutine(SkillTimeCheck(2));
        }

        if (isHideSkills[3])
        {
            StartCoroutine(SkillTimeCheck(3));
        }
    }

    IEnumerator SkillTimeCheck(int skillNum) // 각 스킬의 쿨타임을 관리하는 코루틴 함수
    {
        yield return null; // 한 프레임을 기다림

        if (getSkillTimes[skillNum] > 0) // 현재 쿨타임이 남아있는지 확인
        {
            getSkillTimes[skillNum] -= Time.deltaTime; // 쿨타임 감소

            if (getSkillTimes[skillNum] <= 0) // 쿨타임이 끝나면
            {
                getSkillTimes[skillNum] = 0; // 쿨타임이 0 미만으로 가지 않도록 보정
                isHideSkills[skillNum] = false; // 해당 스킬의 사용 상태를 해제
                hideSkillButtons[skillNum].SetActive(false); // 해당 스킬 버튼 비활성화

                // 힐 버튼이 눌렸다면 다시 누를 수 있도록 허용
                if (skillNum == 2)
                {
                    healButtonPressed = false;
                }
                // 점프(구르기) 버튼
                if (skillNum == 3)
                {
                    rollButtonPressed = false;
                }
            }

            // 쿨타임이 1초 미만일 때 공백을 출력, 그렇지 않으면 시간을 표시
            if (getSkillTimes[skillNum] > 0)
            {
                hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00");
            }
            else
            {
                hideSkillTimeTexts[skillNum].text = "";
            }

            // UI 이미지의 쿨타임 표시를 업데이트 (0에서 1 사이의 값으로 변환)
            float time = getSkillTimes[skillNum] / skillTimes[skillNum];
            hideSkilIlmages[skillNum].fillAmount = time;
        }
    }

    void LateUpdate()
    {
        // 힐 버튼이 눌렸고, 쿨타임이 끝났을 때 힐 버튼을 다시 누를 수 있도록 버튼 활성화
        if (healButtonPressed && !isHideSkills[2])
        {
            healButtonPressed = false;
        }

        if (rollButtonPressed && !isHideSkills[3])
        {
            rollButtonPressed = false;
        }
    }
}
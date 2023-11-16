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

    bool heal;

    // 각 스킬의 상태를 추적하기 위한 변수들
    private bool[] isHideSkills = { false, false, false }; // 각 스킬의 사용 여부를 저장하는 배열
    private float[] skillTimes = { 6, 6, 31 }; // 각 스킬의 쿨타임을 정의하는 배열
    private float[] getSkillTimes = { 0, 0, 0 }; // 각 스킬의 현재 쿨타임을 저장하는 배열

    // Start is called before the first frame update
    void Start()
    {
        // 초기화: UI 요소들을 설정하고 모든 스킬 버튼을 비활성화 상태로 초기화
        for (int i = 0; i < textPros.Length; i++)
        {
            hideSkillTimeTexts[i] = textPros[i].GetComponent<Text>(); // 각 텍스트 요소에 연결
            hideSkillButtons[i].SetActive(false); // 모든 스킬 버튼 비활성화
        }
    }

    void Update()
    {
        for (int indexer = 0; indexer < skillTimes.Length; indexer++)
        {
            //skillTimes[indexer] = getskillcool(indexer);
        }

        heal = Input.GetButtonDown("heal");
        if (heal)
        {
            HideSkillSetting(2);
        }
       
        HideSkillCheck();
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
    }

    IEnumerator SkillTimeCheck(int skillNum) // 각 스킬의 쿨타임을 관리하는 코루틴 함수
    {
        yield return null; // 한 프레임을 기다림

        if (getSkillTimes[skillNum] > 0) // 현재 쿨타임이 남아있는지 확인
        {
            getSkillTimes[skillNum] -= Time.deltaTime; // 쿨타임 감소

            // 쿨타임이 끝나면 해당 스킬의 상태를 초기화하고 UI 업데이트
            if (getSkillTimes[skillNum] < 0)
            {
                getSkillTimes[skillNum] = 0; // 쿨타임이 0 미만으로 가지 않도록 보정
                isHideSkills[skillNum] = false; // 해당 스킬의 사용 상태를 해제
                hideSkillButtons[skillNum].SetActive(false); // 해당 스킬 버튼 비활성화
            }

            hideSkillTimeTexts[skillNum].text = getSkillTimes[skillNum].ToString("00"); // UI 텍스트 업데이트

            // UI 이미지의 쿨타임 표시를 업데이트 (0에서 1 사이의 값으로 변환)
            float time = getSkillTimes[skillNum] / skillTimes[skillNum];
            hideSkilIlmages[skillNum].fillAmount = time;
        }
    }
}
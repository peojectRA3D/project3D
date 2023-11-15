using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHPUI : MonoBehaviour
{
    public RectTransform bossHP;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            bossHP.gameObject.SetActive(true);
        }
    }
}

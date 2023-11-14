using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Boss boss;
    public RectTransform bossHealth;
    public RectTransform bossHealthBar;

    void LateUpdate()
    {
        bossHealthBar.localScale = new Vector3(boss.curHealth / boss.maxHealth, 1, 1);
    }
}

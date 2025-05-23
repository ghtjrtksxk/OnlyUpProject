using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;
    public TextMeshProUGUI healthText;

    void Start()
    {
        curValue = startValue;
    }

    // Update is called once per frame
    void Update()
    {
        uiBar.fillAmount = GetPercentage();
        healthText.text = $"{(int)curValue} / {maxValue}";
    }

    float GetPercentage()
    {
        return curValue / maxValue;
    }

    public void Add(float value) //체력 증가 (최대 체력 초과 방지 적용)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Substract(float value) //체력 감소 (체력 0 유지 적용)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}

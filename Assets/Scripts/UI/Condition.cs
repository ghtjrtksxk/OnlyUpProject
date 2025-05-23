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

    public void Add(float value) //ü�� ���� (�ִ� ü�� �ʰ� ���� ����)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    public void Substract(float value) //ü�� ���� (ü�� 0 ���� ����)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}

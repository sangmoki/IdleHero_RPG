using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// IPointerHandler�� ���콺 �̺�Ʈ
public class LevelUp_Button : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{ 
    // ����ġ �����̴�
    [SerializeField] private Image m_EXP_Slider;   
    // �� object �� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI EXP_Text, ATK_Text, GoldText, HP_Text, Get_EXP_Text;
    // ���콺 ���� ����
    bool isPush = false;
    // ������ �ð�
    float timer = 0.0f;
    // ���� ��ġ�� ������ �ڷ�ƾ
    Coroutine coroutine;

    void Update()
    {
        // ���콺�� ������ ��
        if (isPush)
        {
            // 0.01�ʸ��� ���� ��ġ�� Ȯ��
            timer += Time.deltaTime;
            if (timer >= 0.01f)
            {
                timer = 0.0f;
                Debug.Log("���� ��ġ!");
            }
        }
    }

    // ��÷�� ������ �Լ�
    public void EXP_UP()
    {

    }

    // ���콺�� ������ ��
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("��ġ !");
        coroutine = StartCoroutine(Push_Coroutine());
    }

    // ���콺�� ���� ��
    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("��ġ �׸�!");
        isPush = false;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        timer = 0.0f;
    }

    // ���콺 ���� ���θ� Ȯ���ϴ� �ڷ�ƾ
    IEnumerator Push_Coroutine()
    {
        // 1�ʰ� ����ϴ� ������ ���콺�� ��� ���� �������� �� �������� �����ϱ� ����
        yield return new WaitForSeconds(1.0f);
        isPush = true;
    }
}

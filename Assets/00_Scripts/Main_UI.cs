using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Main_UI : MonoBehaviour
{
    public static Main_UI instance = null; 

    [SerializeField] private TextMeshProUGUI m_Level_Text;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        TextCheck();
    }

    // �������� �� ������ UI ��� ���� �ؽ�Ʈ�� ����
    public void TextCheck()
    {
        m_Level_Text.text = "LV." + (Base_Mng.Player.Level + 1).ToString();
    }
}

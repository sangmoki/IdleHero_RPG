using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Main_Part : MonoBehaviour
{
    [SerializeField]
    private GameObject Lock, Plus;
    [SerializeField]
    private Image Icon, FillImage;
    [SerializeField]
    private  TextMeshProUGUI HP, MP;

    //[Space(20f)]
    //[SerializeField]
    //private int value;  // value를 불러와 InitData를 맞춰준다.

    Character_Scriptable m_Data = null;

    private void Start()
    {
        Initalize();
    }

    public void Initalize()
    {
        if (m_Data == null)
        {
            HP.gameObject.SetActive(false);
            FillImage.transform.parent.gameObject.SetActive(false);
            Icon.gameObject.SetActive(false);
            Plus.gameObject.SetActive(true);
        }
        else
        {
            InitData(m_Data);
        }
    }

    public void GetHeroSetPopup()
    {
        Base_Canvas.instance.Get_UI("#Heroes");
    }

    public void InitData(Character_Scriptable data)
    {
        m_Data = data;

        Lock.SetActive(false);
        Plus.SetActive(false);
        Icon.gameObject.SetActive(true);
        HP.gameObject.SetActive(true);
        FillImage.transform.parent.gameObject.SetActive(true);

        Icon.sprite = Utils.Get_Atlas(data.m_Character_Name);
    }

    public void StateCheck(Player player)
    {
        FillImage.fillAmount = (float)player.MP / (float)m_Data.MaxMP;
        HP.text = StringMethod.ToCurrencyString(player.HP);
        MP.text = player.MP.ToString() + "/" + m_Data.MaxMP;
    }
}

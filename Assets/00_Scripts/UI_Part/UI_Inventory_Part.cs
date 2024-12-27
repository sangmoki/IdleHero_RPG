using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory_Part : MonoBehaviour
{
    [SerializeField]
    private Image RarityImage, IconImage;

    [SerializeField]
    private TextMeshProUGUI CountText;

    // 아이템 초기 정의
    public void Init(Item item)
    {
        RarityImage.sprite = Utils.Get_Atlas(item.data.rarity.ToString());
        IconImage.sprite = Utils.Get_Atlas(item.data.name);
        CountText.text = item.Count.ToString();
    }
}

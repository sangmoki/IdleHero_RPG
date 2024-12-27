using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Manager
{
    public Dictionary<string, Item> m_Items = new Dictionary<string, Item>();

    public void GetItem(Item_Scriptable item)
    {
        // 아이템이 인벤토리에 존재한다면 아이템 갯수 증가.
        if (m_Items.ContainsKey(item.Item_Name))
        {
            m_Items[item.Item_Name].Count++;
        }

        // 아이템이 없다면 아이템의 수량을 1로 새로 생성
        m_Items.Add(item.Item_Name, new Item { data = item, Count = 1 });
    }
}

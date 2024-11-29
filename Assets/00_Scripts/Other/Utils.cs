using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    // Resources폴더에 있는 SpriteAtlas 불러오기
    public static SpriteAtlas m_Atlas = Resources.Load<SpriteAtlas>("Atlas");

    // UI를 담아둘 Stack - LIFO : 가장 늦게 들어온 요소가 가장 먼저 나간다.
    public static Stack<UI_Base> UI_Holder = new Stack<UI_Base>();

    public static void CloseAllPopupUI()
    {
        while(UI_Holder.Count > 0)
            ClosePopupUI();
    }

    public static void ClosePopupUI()
    {
        if (UI_Holder.Count == 0)
            return;

        // Push(스택에 값 삽입)
        // Peek(스택에 마지막 들어온 값 반환)
        // Pop(마지막 들어온 값 반환하면서 Stack에서 제거)
        UI_Base popup = UI_Holder.Peek();
        popup.DisableOBJ();
    }

    // Sprite 아틀라스 불러오는 함수
    public static Sprite Get_Atlas(string temp)
    {
        return m_Atlas.GetSprite(temp);
    }

    // 레어도에 따른 컬러 조정
    public static string String_Color_Rarity(Rarity rare)
    {
        switch(rare)
        {
            case Rarity.Common:
                return "<color=#B3C0C4>";
            case Rarity.Uncommon:
                return "<color=#34AD42>";
            case Rarity.Rare:
                return "<color=#38B9D6>";
            case Rarity.Epic:
                return "<color=#571994>";
            case Rarity.Legendary:
                return "<color=#DB8A33>";
        }
        return "<color=#B3C0C4>";
    }

    // 지수 증가 공식
    // 값이 일정한 비율로 지속적으로 증가한다.
    // 레벨이 높아질 수록 추후에 증가할 값도 높아진다.
    // baseValue : 기본값
    // Level : 레벨
    // value : 지수
    public static float CalculatedValue(float baseValue, int Level, float value)
    {
        // Mathf.Pow(Single, Single) 거듭제곱
        // 즉, baseValue * Level^value
        return baseValue * Mathf.Pow(Level, value);
    }
}

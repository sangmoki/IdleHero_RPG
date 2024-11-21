using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Utils
{
    // Resources폴더에 있는 SpriteAtlas 불러오기
    public static SpriteAtlas m_Atlas = Resources.Load<SpriteAtlas>("Atlas"); 

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
}

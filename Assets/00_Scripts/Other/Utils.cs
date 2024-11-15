using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour
{
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

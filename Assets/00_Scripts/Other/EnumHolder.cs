

// 아이템의 등급
public enum Rarity
{
    Common,     // 흔함
    Uncommon,   // 안흔함
    Rare,       // 레어
    Epic,       // 에픽
    Legendary   // 전설
}

// 현재 스테이지의 상태
public enum Stage_State
{
    Ready,      // FadeIn상태 (준비상태)
    Play,       // 전투 스테이지  
    Boss,       // 보스 스테이지
    Clear,      // 보스 처치
    Dead        // 플레이어 사망
}

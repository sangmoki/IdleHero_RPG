using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 매니저 script를 관리하는 부모
// 어느 곳에서나 접근이 사능하다.
public class Base_Manager : MonoBehaviour
{
    // 싱글톤 패턴을 위한 변수
    public static Base_Manager instance = null;

    #region Parmeter
    private static Pool_Manager s_Pool = new Pool_Manager();
    private static Player_Manager s_Player = new Player_Manager();
    private static Data_Manager s_Data = new Data_Manager();
    private static Item_Manager s_Item = new Item_Manager();
    public static Inventory_Manager m_Inventory = new Inventory_Manager();
    private static Character_Manager s_Character = new Character_Manager();
    private static ADS_Manager s_ADS = new ADS_Manager();
    private static Firebase_Manager s_Firebase = new Firebase_Manager();

    public static bool isFast = false;

    float Save_Timer = 0.0f;

    public static Pool_Manager Pool { get { return s_Pool; } }
    public static Player_Manager Player { get { return s_Player; } }
    public static Data_Manager Data { get { return s_Data; } }
    public static Item_Manager Item { get { return s_Item; } }
    public static Inventory_Manager Inventory { get { return m_Inventory; } }
    public static Character_Manager Character { get { return s_Character; } }
    public static ADS_Manager ADS { get { return s_ADS; } }
    public static Firebase_Manager Firebase { get { return s_Firebase; } }
    #endregion

    private void Awake()
    {
        Initalize();
    }

    private void Update()
    {
        Save_Timer += Time.unscaledDeltaTime;

        if (Save_Timer >= 10.0f)
        {
            Save_Timer = 0.0f;
            Firebase.WriteData();
        }

        for (int i = 0; i < Data.Buff_timers.Length; i++)
        {
            // 배속 적용중일 시 
            if (Data.Buff_timers[i] > 0.0f)
            {
                // fill과 timertext를 갱신
                // 배속 영향을 받을 수 없게 unscaledDeltaTime을 사용
                Data.Buff_timers[i] -= Time.unscaledDeltaTime;
            }
            if (Data.Buff_x2 > 0.0f) Data.Buff_x2 -= Time.unscaledDeltaTime;
        }
    }

    private void Initalize()
    {
        // 씬을 이동해도 파고되지 않고 계속 유지
        if (instance == null)
        {
            instance = this;
            // 해당 베이스 매니저를 가지고 있는 오브젝트가 된다.
            Pool.Initalize(transform);
            ADS.Init();
            //StartCoroutine(Ad_Coroutine());
            Data.Init();
            Item.Init();
            Firebase.Init();

            Character.GetCharacter(0, "Hunter");
            StartCoroutine(Action_Coroutine(() => Stage_Manager.State_Change(Stage_State.Ready), 0.3f));
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            // 복제가 된다면 복제가 된 것을 삭제한다.
            Destroy(this.gameObject);
        }
    }

    /*  광고 실행 코루틴
    IEnumerator Ad_Coroutine()
    {
        // 3초 뒤 전면광고 실행
        yield return new WaitForSeconds(3.0f);

        // 보상형 광고 실행
        ADS.ShowRewardedAds(GetReward);

        // 전면형 광고
        // ADS.ShowInterstitialAds();
    }*/

    private void GetReward() => Debug.Log("보상형 광고를 시청하여 아이템을 획득하였습니다.");

    // Resources 폴더에 있는 Prefab을 불러오기 위한 함수
    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    // 반환 코루틴 호출 함수
    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    public void Coroutine_Action(float timer, Action action)
    {
        StartCoroutine(Action_Coroutine(action, timer));
    }

    // 사용 후 오브젝트 풀에 반환하는 함수
    IEnumerator Return_Pool_Coroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }

    IEnumerator Action_Coroutine(Action action, float timer)
    {
        yield return new WaitForSeconds(timer);
        action?.Invoke();
    }

    // 게임이 종료될 때 데이터를 저장
    private void OnDestroy()
    {
        Firebase.WriteData();
    }
}

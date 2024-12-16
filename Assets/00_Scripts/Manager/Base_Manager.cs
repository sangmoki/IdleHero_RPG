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
    public static Pool_Manager s_Pool = new Pool_Manager();
    public static Player_Manager s_Player = new Player_Manager();
    public static Data_Manager s_Data = new Data_Manager();
    public static Item_Manager s_Item = new Item_Manager();
    public static Character_Manager s_Character = new Character_Manager();

    public static Pool_Manager Pool { get { return s_Pool; } }
    public static Player_Manager Player { get { return s_Player; } }
    public static Data_Manager Data { get { return s_Data; } }
    public static Item_Manager Item { get { return s_Item; } }
    public static Character_Manager Character { get { return s_Character; } }
    #endregion

    private void Awake()
    {
        Initalize();
    }

    private void Initalize()
    {
        // 씬을 이동해도 파고되지 않고 계속 유지
        if (instance == null)
        {
            instance = this;
            // 해당 베이스 매니저를 가지고 있는 오브젝트가 된다.
            Pool.Initalize(transform);
            Data.Init();
            Item.Init();
            Character.GetCharacter(0, "Barbarian");

            StartCoroutine(Action_Coroutine(() => Stage_Manager.State_Change(Stage_State.Ready), 0.3f));
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            // 복제가 된다면 복제가 된 것을 삭제한다.
            Destroy(this.gameObject);
        }
    }

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
}

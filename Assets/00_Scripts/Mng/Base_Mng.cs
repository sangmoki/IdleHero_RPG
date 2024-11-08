using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 모든 매니저 script를 관리하는 부모
// 어느 곳에서나 접근이 사능하다.
public class Base_Mng : MonoBehaviour
{
    // 싱글톤 패턴을 위한 변수
    public static Base_Mng instance = null;

    public static Pool_Mng s_Pool = new Pool_Mng();
    public static Pool_Mng Pool { get { return s_Pool; } }

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

    // 사용 후 오브젝트 풀에 반환하는 함수
    IEnumerator Return_Pool_Coroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }
}

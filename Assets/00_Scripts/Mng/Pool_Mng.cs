using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// 몬스터, 투사체 등등 여러 오브젝트를 풀링 기법으로 사용하기 위한 인터페이스
// 매번 새롭게 생성하는 것보다 효율적이다.
public interface IPool
{
    // Hierachy 상의 부모 오브젝트를 만들기 위함
    // 가독성을 용이하기 위해 사용
    Transform parentTransform { get; set; }

    // Queue : 선입선출(FIFO) 구조의 자료구조
    Queue<GameObject> pool { get; set; }

    // Get을 통해 오브젝트(몬스터 등)를 가져온다.
    GameObject Get(Action<GameObject> action = null);

    // 가져온 오브젝트(몬스터 등)를 반환한다.
    void Return(GameObject obj, Action<GameObject> action = null);
}

// 위에 생성한 Interface를 상속받아 구현
public class Object_Pool : IPool
{
    // Dequeue : Queue에서 가장 앞에 있는 요소를 제거하고 반환
    // Enqueue : Queue의 끝에 요소를 추가

    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();
    public Transform parentTransform { get; set; }
    public GameObject Get(Action<GameObject> action = null)
    {
        // pool에서 맨 앞의 요소를 꺼내온다.
        GameObject obj = pool.Dequeue();
        // 오브젝트를 활성화 한다.
        obj.SetActive(true);
        if (action != null)
        {
            action?.Invoke(obj);
        }

        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        // 매개변수로 전달된 오브젝트를 pool에 넣는다.
        pool.Enqueue(obj);
        // 정리된 상태로 보이기 위해 부모에 넣는다.
        obj.transform.parent = parentTransform;
        // 오브젝트를 비활성화 한다.
        obj.SetActive(false);
        if (action != null)
        {
            action?.Invoke(obj);
        }
    }

}

// 풀링 기법 사용 - Queue를 사용하여 구현
// 오브젝트 풀링을 사용하면 메모리를 효율적으로 사용할 수 있다.
// 예시로, 수영장에서 오브젝트 하나가 들어갔다 나갔다 하는것을 의미한다.
// 즉, 오브젝트를 껏다 켰다 하는 방법과 같은 느낌
public class Pool_Mng
{
    // Dictionary : Key와 Value로 이루어진 자료구조
    // ContainsKey : Dictionary에 키가 포함되어 있는지 확인 - return : true or false
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    // 모든 풀링들을 담기 위한 부모 오브젝트
    Transform base_Obj = null;

    // BaseMng 안으로 넣기 위한 함수
    public void Initalize(Transform T)
    {
        base_Obj = T;
    }

    public IPool Pooling_Obj(string path)
    {
        // Dictionary에 path가 없다면 Add_Pool 함수 실행
        if (m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }

        // 큐에 있는 오브젝트의 수가 1보다 작다면
        // 즉, 아무것도 없다면 새 오브젝트 생성
        if (m_pool_Dictionary[path].pool.Count <= 0) Add_Queue(path);
        return m_pool_Dictionary[path];
    }

    public GameObject Add_Pool(string path)
    {
        // 부모 오브젝트 생성
        // 이 오브젝트 하위에 Destroy된 오브젝트들을 넣어두기 위함
        GameObject obj = new GameObject(path + "##POOL");

        // 생성된 오브젝트를 base_Obj의 자식으로 설정
        obj.transform.SetParent(base_Obj);

        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;
        return obj;
    }

    // 새로운 오브젝트 생성 함수
    public void Add_Queue(string path)
    {
        // 새로운 Object 생성
        // Pool_Mng는 상속을 받지 않으면 기본 제공하는 함수인
        // Instantiate를 사용할 수 없기 때문에
        // Base_Mng의 Instantiate_Path 함수를 통해 생성
        var go = Base_Mng.instance.Instantiate_Path(path);
        go.transform.parent = m_pool_Dictionary[path].parentTransform;

        // 생성한 Object 리턴
        m_pool_Dictionary[path].Return(go);
    }
}

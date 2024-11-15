using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��� �Ŵ��� script�� �����ϴ� �θ�
// ��� �������� ������ ����ϴ�.
public class Base_Mng : MonoBehaviour
{
    // �̱��� ������ ���� ����
    public static Base_Mng instance = null;

    public static Pool_Mng s_Pool = new Pool_Mng();
    public static Player_Mng s_Player = new Player_Mng();

    public static Pool_Mng Pool { get { return s_Pool; } }
    public static Player_Mng Player { get { return s_Player; } }


    private void Awake()
    {
        Initalize();
    }

    private void Initalize()
    {
        // ���� �̵��ص� �İ���� �ʰ� ��� ����
        if (instance == null)
        {
            instance = this;
            // �ش� ���̽� �Ŵ����� ������ �ִ� ������Ʈ�� �ȴ�.
            Pool.Initalize(transform);
            DontDestroyOnLoad(this.gameObject);
        }
        else if (instance != this)
        {
            // ������ �ȴٸ� ������ �� ���� �����Ѵ�.
            Destroy(this.gameObject);
        }
    }

    // Resources ������ �ִ� Prefab�� �ҷ����� ���� �Լ�
    public GameObject Instantiate_Path(string path)
    {
        return Instantiate(Resources.Load<GameObject>(path));
    }

    // ��ȯ �ڷ�ƾ ȣ�� �Լ�
    public void Return_Pool(float timer, GameObject obj, string path)
    {
        StartCoroutine(Return_Pool_Coroutine(timer, obj, path));
    }

    // ��� �� ������Ʈ Ǯ�� ��ȯ�ϴ� �Լ�
    IEnumerator Return_Pool_Coroutine(float timer, GameObject obj, string path)
    {
        yield return new WaitForSeconds(timer);
        Pool.m_pool_Dictionary[path].Return(obj);
    }
}

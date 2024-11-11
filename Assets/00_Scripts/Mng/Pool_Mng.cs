using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// ����, ����ü ��� ���� ������Ʈ�� Ǯ�� ������� ����ϱ� ���� �������̽�
// �Ź� ���Ӱ� �����ϴ� �ͺ��� ȿ�����̴�.
public interface IPool
{
    // Hierachy ���� �θ� ������Ʈ�� ����� ����
    // �������� �����ϱ� ���� ���
    Transform parentTransform { get; set; }

    // Queue : ���Լ���(FIFO) ������ �ڷᱸ��
    Queue<GameObject> pool { get; set; }

    // Get�� ���� ������Ʈ(���� ��)�� �����´�.
    GameObject Get(Action<GameObject> action = null);

    // ������ ������Ʈ(���� ��)�� ��ȯ�Ѵ�.
    void Return(GameObject obj, Action<GameObject> action = null);
}

// ���� ������ Interface�� ��ӹ޾� ����
public class Object_Pool : IPool
{
    // Dequeue : Queue���� ���� �տ� �ִ� ��Ҹ� �����ϰ� ��ȯ
    // Enqueue : Queue�� ���� ��Ҹ� �߰�

    public Queue<GameObject> pool { get; set; } = new Queue<GameObject>();
    public Transform parentTransform { get; set; }
    public GameObject Get(Action<GameObject> action = null)
    {
        // pool���� �� ���� ��Ҹ� �����´�.
        GameObject obj = pool.Dequeue();
        // ������Ʈ�� Ȱ��ȭ �Ѵ�.
        obj.SetActive(true);
        if (action != null)
        {
            action?.Invoke(obj);
        }

        return obj;
    }

    public void Return(GameObject obj, Action<GameObject> action = null)
    {
        // �Ű������� ���޵� ������Ʈ�� pool�� �ִ´�.
        pool.Enqueue(obj);
        // ������ ���·� ���̱� ���� �θ� �ִ´�.
        obj.transform.parent = parentTransform;
        // ������Ʈ�� ��Ȱ��ȭ �Ѵ�.
        obj.SetActive(false);
        if (action != null)
        {
            action?.Invoke(obj);
        }
    }

}

// Ǯ�� ��� ��� - Queue�� ����Ͽ� ����
// ������Ʈ Ǯ���� ����ϸ� �޸𸮸� ȿ�������� ����� �� �ִ�.
// ���÷�, �����忡�� ������Ʈ �ϳ��� ���� ������ �ϴ°��� �ǹ��Ѵ�.
// ��, ������Ʈ�� ���� �״� �ϴ� ����� ���� ����
public class Pool_Mng
{
    // Dictionary : Key�� Value�� �̷���� �ڷᱸ��
    // ContainsKey : Dictionary�� Ű�� ���ԵǾ� �ִ��� Ȯ�� - return : true or false
    public Dictionary<string, IPool> m_pool_Dictionary = new Dictionary<string, IPool>();

    // ��� Ǯ������ ��� ���� �θ� ������Ʈ
    Transform base_Obj = null;

    // BaseMng ������ �ֱ� ���� �Լ�
    public void Initalize(Transform T)
    {
        base_Obj = T;
    }

    public IPool Pooling_Obj(string path)
    {
        // Dictionary�� path�� ���ٸ� Add_Pool �Լ� ����
        if (m_pool_Dictionary.ContainsKey(path) == false)
        {
            Add_Pool(path);
        }

        // ť�� �ִ� ������Ʈ�� ���� 1���� �۴ٸ�
        // ��, �ƹ��͵� ���ٸ� �� ������Ʈ ����
        if (m_pool_Dictionary[path].pool.Count <= 0) Add_Queue(path);
        return m_pool_Dictionary[path];
    }

    public GameObject Add_Pool(string path)
    {
        // �θ� ������Ʈ ����
        // �� ������Ʈ ������ Destroy�� ������Ʈ���� �־�α� ����
        GameObject obj = new GameObject(path + "##POOL");

        // ������ ������Ʈ�� base_Obj�� �ڽ����� ����
        obj.transform.SetParent(base_Obj);

        Object_Pool T_Component = new Object_Pool();

        m_pool_Dictionary.Add(path, T_Component);

        T_Component.parentTransform = obj.transform;
        return obj;
    }

    // ���ο� ������Ʈ ���� �Լ�
    public void Add_Queue(string path)
    {
        // ���ο� Object ����
        // Pool_Mng�� ����� ���� ������ �⺻ �����ϴ� �Լ���
        // Instantiate�� ����� �� ���� ������
        // Base_Mng�� Instantiate_Path �Լ��� ���� ����
        var go = Base_Mng.instance.Instantiate_Path(path);
        go.transform.parent = m_pool_Dictionary[path].parentTransform;

        // ������ Object ����
        m_pool_Dictionary[path].Return(go);
    }
}

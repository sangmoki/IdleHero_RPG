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
    public static Pool_Mng Pool { get { return s_Pool; } }

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
}

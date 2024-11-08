using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject monster_Prefab;

    public int m_Count;         // ������ ��
    public float m_SpawnTime;   // �� �� ���� ������ ������

    private void Start()
    {
        // ���� ���� �ڷ�ƾ ����
        StartCoroutine(SpawnCoroutine());
    }


    // ���� ���� �ڷ�ƾ �Լ�
    // ������ ���� ����
    // 1. ���ʹ� ���� ������ �� �� ���� ���÷� �����Ǿ�� �Ѵ�.
    IEnumerator SpawnCoroutine()
    {
        Vector3 pos;

        // Random.insideUnitSphere == Vector3(x, y, z) <- Vector3 ���� ������ǥ
        // Random.insideUnitCircle == Vector2(x, y) <- Vector2 ���� ������ǥ
        for (int i = 0; i < m_Count; i++)
        {
            // ���ʹ� ����� �������� �������� �����Ǿ�� �Ѵ�.
            // 5~10�� �� = �����Ǵ� �ݿ��� ��ġ ��
            pos = Vector3.zero + Random.insideUnitSphere * 5.0f; 
            // pos�� y�� ���� -�� +�� �Ǹ� �ȵǱ� ������ 0���� �ʱ�ȭ
            pos.y = 0.0f;

            // ���Ͱ� ������ ��ġ�� �߾����� ������ pos��ǥ�� �ٽ� �����Ѵ�.
            // 3~5�� �� = �߾����� �������� �����Ǵ� ��ġ ���� �ݰ�
            while (Vector3.Distance(pos, Vector3.zero) <= 3.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 10.0f;
                pos.y = 0.0f;
            }

            // ȸ������ ������ ���� ���·� pos��ǥ�� ���� ����
            // Instantiate (������) Destroy (�ı���)
            // �� �ΰ��� GC(Garbage Collection)�� �߻��Ѵ�.
            // ��� �����Ǵٺ��� �޸𸮰� ����ȭ �ȴ�.
            // �̰��� �����ϱ� ���� Ǯ�� ���(Pool_Mng)�� ����Ѵ�.
            var goObj = Base_Mng.Pool.Pooling_Obj("Monster").Get((value) =>
            {
                // ������ Object ���� ����
                value.GetComponent<Monster>().Init();
                // ������ ��ġ�� pos�� ����
                value.transform.position = pos;
                // ���Ͱ� ����� �ٶ󺸵��� ����
                value.transform.LookAt(Vector3.zero);
            });

            // ���� Ǯ�� �ڷ�ƾ ����
            StartCoroutine(ReturnCoroutine(goObj));
        }

        // ���� �ð���ŭ ��� �� ���� ����
        yield return new WaitForSeconds(m_SpawnTime);
        StartCoroutine(SpawnCoroutine());
    }

    // ���͸� Ǯ���ϱ� ���� �ڷ�ƾ �Լ�
    IEnumerator ReturnCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(1.0f);
        Base_Mng.Pool.m_pool_Dictionary["Monster"].Return(obj);
    }
}

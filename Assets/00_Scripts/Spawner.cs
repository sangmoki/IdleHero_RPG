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

    // ���Ͱ� ������ �� �߻��ϴ� �ڷ�ƾ �Լ�
    IEnumerator Spawn_Start()
    {
        float current = 0.0f;
        float percent = 0.0f;   
        float start = 0.0f;
        float end = transform.localScale.x;

        // percent�� 1���� ���� ���� ����
        while(percent < 1)
        {
            current += Time.deltaTime;  // 1�ʿ� �ɸ��� �ð�
            percent = current / 1.0f;   // 1�ʿ� �ɸ��� �ð��� 1�� ������ 0~1 ������ ������ ��ȯ

            // ���۰�(0.0f)���� ����(����.x) ������ �ɸ��� Ư�� �ð� �ӵ��� �̵�
            float LerpPos = Mathf.Lerp(start, end, percent); 
            transform.localScale = new Vector3(LerpPos, LerpPos, LerpPos);
            yield return null;
        }
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
            pos = Vector3.zero + Random.insideUnitSphere * 10.0f; 
            // pos�� y�� ���� -�� +�� �Ǹ� �ȵǱ� ������ 0���� �ʱ�ȭ
            pos.y = 0.0f;

            // ���Ͱ� ������ ��ġ�� �߾����� ������ pos��ǥ�� �ٽ� �����Ѵ�.
            // 3~5�� �� = �߾����� �������� �����Ǵ� ��ġ ���� �ݰ�
            while (Vector3.Distance(pos, Vector3.zero) <= 5.0f)
            {
                pos = Vector3.zero + Random.insideUnitSphere * 10.0f;
                pos.y = 0.0f;
            }

            // ȸ������ ������ ���� ���·� pos��ǥ�� ���� ����
            var go = Instantiate(monster_Prefab, pos, Quaternion.identity);
        }

        // ���� �ð���ŭ ��� �� ���� ����
        yield return new WaitForSeconds(m_SpawnTime);
        StartCoroutine(SpawnCoroutine());
    }

}

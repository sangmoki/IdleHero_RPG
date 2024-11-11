using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COIN_PARENT : MonoBehaviour
{
    Vector3 target;
    Camera cam;                                     
    RectTransform[] childs = new RectTransform[5]; // ���� ���� ���� ����

    [Range(0.0f, 500.0f)]
    [SerializeField] private float m_Distance_Range, speed; // ������ Ƣ����� ����, �ӵ�

    private void Awake()
    {
        cam = Camera.main;

        // �ڽ� ������Ʈ�� RectTransform ������Ʈ�� �����´�.
        for (int i = 0; i < childs.Length; i++)
        {
            childs[i] = transform.GetChild(i).GetComponent<RectTransform>();
        }
    }

    public void Init(Vector3 pos)
    {
        target = pos;

        // ĵ���� ��ǥ�� ����
        transform.position = cam.WorldToScreenPoint(target);

        // ���ε� ������ ��ġ�� �ʱ�ȭ���ش�.
        for (int i = 0; i < 5; i++)
        {
            childs[i].anchoredPosition = Vector2.zero;
        }

        // ĵ������ �ڽ����� ����
        transform.parent = Base_Canvas.instance.HOLDER_LAYER(0);

        StartCoroutine(Coin_Effect());
    }

    IEnumerator Coin_Effect()
    {
        Vector2[] RandomPos = new Vector2[childs.Length];
        for (int i = 0; i < childs.Length; i++)
        {
            // Ÿ���� ��ġ + ������ ��ġ�� ������ ����
            RandomPos[i] = new Vector2(target.x, target.y) + Random.insideUnitCircle * Random.Range(-m_Distance_Range, m_Distance_Range);
        }

        // ��� ���ε��� ���������� �����ϴ� ���
        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                // ���� RectTransform�� �����´�.
                RectTransform rect = childs[i];
                // rect�� anchoredPosition�� RandomPos[i]�� �̵���Ų��.
                rect.anchoredPosition = Vector2.MoveTowards(rect.anchoredPosition, RandomPos[i], Time.deltaTime * speed);
            }

            // ��� ���ε��� ���������� �����ߴٸ� break
            if (Distance_Boolean(RandomPos, 0.5f)) break;

            // �� ���� �������� ���
            yield return null;
        }

        yield return new WaitForSeconds(0.3f);

        // ���ε��� ���������� ���� �Ŀ� �Ѱ����� �̵��ϴ� ���
        while (true)
        {
            for (int i = 0; i < childs.Length; i++)
            {
                RectTransform rect = childs[i];
                // ���� anchoredPosition�� �θ��� ���� ��ǥ �����̱� ������
                // World ��ǥ�� position�� ���
                rect.position = Vector2.MoveTowards(rect.position, Base_Canvas.instance.COIN.position, Time.deltaTime * (speed * 15));
            }

            if (Distance_Boolean_World(0.5f))
            {
                Base_Mng.Pool.m_pool_Dictionary["COIN_PARENT"].Return(this.gameObject);
                break;
            }
            yield return null;
        }
        yield return null;
    }

    // ���� ��ǥ�� ���� (anchoredPosition)
    private bool Distance_Boolean(Vector2[] end, float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            // ������ �Ÿ� ���� Range���� ũ�� false�� ��ȯ�Ѵ�.
            // ��, �ϳ��� ���������� �������� ���ߴٸ� false, �ƴ϶�� true
            float distance = Vector2.Distance(childs[i].anchoredPosition, end[i]);
            if (distance > range)
            {
                return false;
            }
        }
        return true;
    }

    // ���� ��ǥ�� ���� - ������ �θ� �ٶ󺸰� �������� ������
    // ������ ������ �𿩼� ������ ���ο� ���� �ϱ� ����
    private bool Distance_Boolean_World (float range)
    {
        for (int i = 0; i < childs.Length; i++)
        {
            // ��, �ϳ��� ���������� �������� ���ߴٸ� false, �ƴ϶�� true
            float distance = Vector2.Distance(childs[i].position, Base_Canvas.instance.COIN.position);
            if (distance > range)
            {
                return false;
            }
        }
        return true;
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private float m_Speed;   // �Ѿ� �ӵ�
    Transform m_Target;      // Ÿ��
    Vector3 m_TargetPos;     // Ÿ���� ��ġ
    double m_DMG;            // ������
    string m_Character_Name; // Resources ���� ������Ʈ �̸�
    bool GetHit = false;     // Ÿ���� �¾Ҵ��� Ȯ�� - ������ ������ �ߺ��Ǵ� �� ����

    Dictionary<string, GameObject> m_Projectiles = new Dictionary<string, GameObject>();
    Dictionary<string, ParticleSystem> m_Muzzles = new Dictionary<string, ParticleSystem>();

    // ������ ���ڸ��� �ڽĵ��� �����´�.
    private void Awake()
    {
        // GetChild(index) - ������� �ڽ� ������Ʈ�� �����´�.
        Transform projectiles = transform.GetChild(0);
        Transform muzzles = transform.GetChild(1);

        // �ݺ��� ���� �ڽ� ������Ʈ�� Dictionary�� ����
        for (int i = 0; i < projectiles.childCount; i++) 
        {
            m_Projectiles.Add(projectiles.GetChild(i).name, projectiles.GetChild(i).gameObject);
        }

        for (int i = 0; i < muzzles.childCount; i++)
        {
            m_Muzzles.Add(muzzles.GetChild(i).name, muzzles.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    public void Init(Transform target, double dmg, string Character_Name)
    {
        m_Target = target;
        transform.LookAt(m_Target);
        GetHit = false;
        m_TargetPos = m_Target.position;
        m_DMG = dmg;
        m_Character_Name = Character_Name;
        // �Ѿ� �߻� ����Ʈ Ȱ��ȭ
        m_Projectiles[m_Character_Name].gameObject.SetActive(true);
    }

    private void Update()
    {
        // ���� ���¶�� return
        if (GetHit) return;
        m_TargetPos.y = 0.5f; // �Ѿ� ���� ����

        // �Ѿ��� Ÿ���� ��ġ�� �̵�
        transform.position = Vector3.MoveTowards(transform.position, m_TargetPos, Time.deltaTime * m_Speed);

        // �Ѿ˰� Ÿ���� �Ÿ��� 0.1f���� ���� ���
        if (Vector3.Distance(transform.position, m_TargetPos) <= 0.1f)
        {
            if (m_Target != null)
            {
                GetHit = true;
                // Ÿ���� HP�� ��������ŭ ����
                m_Target.GetComponent<Monster>().GetDamage(10);
                //m_Target.GetComponent<Character>().HP -= m_DMG;
                // �Ѿ� �߻� ����Ʈ ��Ȱ��ȭ
                m_Projectiles[m_Character_Name].gameObject.SetActive(false);
                // ���� Ȱ��ȭ
                m_Muzzles[m_Character_Name].Play();

                //  Muzzle�� ����Ʈ�� ����ִ� �ð����� ��ٸ� �� �Ѿ� ��ȯ
                StartCoroutine(ReturnObject(m_Muzzles[m_Character_Name].duration));
            }
        }
    }

    // �ҷ� ���� �Լ�
    IEnumerator ReturnObject(float timer)
    {
        yield return new WaitForSeconds(timer);
        Base_Mng.Pool.m_pool_Dictionary["Bullet"].Return(this.gameObject);
    }
}

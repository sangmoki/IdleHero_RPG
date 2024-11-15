using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    private Character_Scriptable CH_Data; // ĳ���� ������
    public string CH_Name;                // ĳ���� �̸�
    Vector3 startPos;                     // �÷��̾��� ���� ��ġ
    Quaternion rot;                       // �÷��̾��� ȸ����

    protected override void Start()
    {
        base.Start();

        // ĳ������ �̸��� ���� Scriptable�� ������ ĳ������ ������ ���� 
        Data_Set(Resources.Load<Character_Scriptable>("Scriptable/" + CH_Name));

        // �÷��̾��� �ʱ� ���� ��ġ�� ȸ������ ����
        // ���͸� �����ϴٰ� ���Ͱ� ���� ������ �������
        // �ٽ� ���ڸ��� ���ƿ��� ���� ����
        startPos = transform.position;
        rot = transform.rotation;
    }

    private void Data_Set(Character_Scriptable data)
    {
        CH_Data = data;
        Attack_Range = data.m_Attack_Range;
    }

    private void Update()
    {
        // ���� ����� ���͸� ã�Ƽ� Ÿ������ ����
        FindClosetTarget(Spawner.m_Monsters.ToArray());

        if (m_Target == null)
        {
            // ���� ��ġ�� ���� ��ġ�� ����� ��
            float targetPos = Vector3.Distance(transform.position, startPos);
            if (targetPos > 0.1f)
            {
                // ���ڸ��� �̵�
                transform.position = Vector3.MoveTowards(transform.position, startPos, Time.deltaTime);
                transform.LookAt(startPos);
                AnimatorChange("isMOVE");
            } 
            else
            {
                // �ٽ� ���ƿԴٸ� �����·� ��ȯ
                transform.rotation = rot;
                AnimatorChange("isIDLE");
            }
            return;
        }

        // ���� Ÿ���� ���°� ��� ���¶�� �ٽ� Ÿ���� ã�´�.
        if (m_Target.GetComponent<Character>().isDead) 
            FindClosetTarget(Spawner.m_Monsters.ToArray());

        // ���� Ÿ���� ��ġ�� �÷��̾��� ��ġ�� ����� ��
        float targetDistance = Vector3.Distance(transform.position, m_Target.position);
        // ���� Ÿ���� ���� ���� �ȿ� ������ ���ݹ��� �ȿ� �������� ���� ���
        if (targetDistance <= Target_Range && targetDistance > Attack_Range && isATTACK == false)
        {
            // Ÿ���� ���� �̵�
            AnimatorChange("isMOVE");
            transform.LookAt(m_Target.position);
            transform.position = Vector3.MoveTowards(transform.position, m_Target.position, Time.deltaTime);
        }
        else if (targetDistance <= Attack_Range && isATTACK == false)
        {
            // Ÿ���� ���� ���� �ȿ� ������ ���
            // ����
            isATTACK = true;
            AnimatorChange("isATTACK");

            // ���� �� ���� ���� �ʱ�ȭ �Լ� ȣ��
            Invoke("InitAttack", 1.0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Item_OBJ : MonoBehaviour
{
    [SerializeField] private Transform ItemTextRect;      // ������ ����
    [SerializeField] private TextMeshProUGUI m_Text;      // �������� �̸�
    [SerializeField] private GameObject[] Raritys;        // �������� ���
    [SerializeField] private ParticleSystem m_Loot;       // �������� ȹ���� �� ������ ����Ʈ
    [SerializeField] private float firingAngle = 45.0f;   // �
    [SerializeField] private float gravity = 9.8f;        // ridigd(default 9.8)�߷� 
    
    Rarity rarity;                                        // �������� ���
    bool isCheck = false;                                 // �������� �̵��Ǿ����� üũ�ϴ� �÷���


    // ��� Ȯ��
    void RarityCheck()
    {
        // ������ �̵� Ȯ��
        isCheck = true;

        // ������ ȿ�� ������ ���� ȸ���� ���ѳ��ұ� ������
        // �������� ���� �ʱ�ȭ(0, 0, 0)
        transform.rotation = Quaternion.identity;

        // �������� ������ ����� ���� �̹��� Ȱ��ȭ
        Raritys[(int)rarity].SetActive(true);

        ItemTextRect.gameObject.SetActive(true);
        ItemTextRect.parent = Base_Canvas.instance.HOLDER_LAYER(2);

        // �ؽ�Ʈ�� color�� �ؽ�Ʈ ���� ����
        // <color=#00FF00>TEST ITEM</color> �� ����
        m_Text.text = Utils.String_Color_Rarity(rarity) + "TEST ITEM" + "</color>";

        StartCoroutine(LootItem());
    }

    // ������ �ݱ� ����Ʈ Ȱ��ȭ
    IEnumerator LootItem()
    {
        yield return new WaitForSeconds(Random.Range(1.0f, 1.5f));

        // ������ ����Ǹ� ���Ƽ ���� ����Ʈ ��Ȱ��ȭ
        for (int i = 0; i < Raritys.Length; i++)
            Raritys[i].SetActive(false);

        // �� ������Ʈ�� ĵ������ �����´�.
        ItemTextRect.transform.parent = this.transform;
        // �������� ��Ȱ��ȭ
        ItemTextRect.gameObject.SetActive(false);

        // �������� ��Ȱ��ȭ�Ǹ� ���� ����Ʈ�� ����
        m_Loot.Play();
        // 0.5�� �ڿ� ���� ����Ʈ�� Ǯ�� ��ȯ�Ѵ�.
        yield return new WaitForSeconds(0.5f);
        Base_Mng.Pool.m_pool_Dictionary["Item_OBJ"].Return(this.gameObject);
    }

    private void Update()
    {
        if (isCheck == false) return;

        // �������� �̹��� �̱� ������ ĵ������ ������ �Ǿ�� �Ѵ�.
        ItemTextRect.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    public void Init(Vector3 pos)
    {
        // �������� ��� ����
        rarity = (Rarity)Random.Range(0, 5);

        // �̵� Ȯ�� �ʱ�ȭ
        isCheck = false;
        // ���Ͱ� ����� ��ġ�� ����
        transform.position = pos;
        // ������ ��ġ�� �ƴ� ���� ��ġ �ֺ��� ������ ��ǥ�� �������� �������� ���� ��ġ ��
        Vector3 Target_Pos = new Vector3(pos.x + (Random.insideUnitSphere.x * 1.0f), 0.5f, pos.z + (Random.insideUnitSphere.z * 1.0f));
        // ��� ��ġ�� �̵����ϸ鼭 ��� �׸� ���̳�
        StartCoroutine(SimulateProjectile(Target_Pos));
    }

    // ��� �׸��� �Լ�
    IEnumerator SimulateProjectile(Vector3 pos)
    {
        // Mathf.Abs(���밪), Mathf.Min,Max(�ּڰ�, �ִ�), Mathf.Sign(���� -1, ��� +1)

        // Mathf.Sin(����)
        // Mathf.Cos(�ڽ�)
        // Mathf.Deg2Rad (����(degree) -> ȣ��(radian)) : Degree to Radian �� ���
        // Mathf.Rad2Deg(ȣ��(radian)->����(degree)) : Radian to Degree �� ���
        // Mathf.Sqrt(������)

        // �������� ������ ��ġ ��
        float target_Distance = Vector3.Distance(transform.position, pos);

        // �������� �ӵ� 
        float projectile_Velocity = target_Distance / (Mathf.Sin(2 * firingAngle * Mathf.Deg2Rad) / gravity);

        // �����ร Cos�� Sin���� ���� ���� ��
        float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * Mathf.Deg2Rad);
        float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * Mathf.Deg2Rad);

        // Ÿ���� ��ġ(����) ��
        float flightDuration = target_Distance / Vx;

        // �������� �Ѱ����� �ٶ󺸰� ��������
        // �̵��ϴ� �������� ȸ��
        transform.rotation = Quaternion.LookRotation(pos - transform.position);

        // ���̸� �̵��ϱ� ���� �ð�
        float time = 0.0f;

        // time�� flightDuration���� ������ ������ �ݺ�
        while (time < flightDuration)
        {
            // 1�ʸ��� �����ش�.
            // ���� flightDuration�� ���� 2.5��� 2.5�ʿ� ������ ������ �ݺ��Ͽ� x��� y���� �̵��ҰŴ�.
            // y�� �Ʒ��� �������� x�� �̵��Ѵ�.
            transform.Translate(0, (Vy - (gravity * time)) * Time.deltaTime, Vx * Time.deltaTime);
            time += Time.deltaTime;
            yield return null;
        }
        RarityCheck();
    }
}

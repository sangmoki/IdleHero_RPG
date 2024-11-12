using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HIT_TEXT : MonoBehaviour
{
    // Ÿ�� ����� ��ġ
    Vector3 target;
    // ī�޶� �������� ������ ��ġ�� �޾ƿ��� ���� ����
    Camera cam;
    public TextMeshProUGUI m_Text;

    [SerializeField] private GameObject m_Critical;

    // Inspector�� �ٹ̴� �뵵
    // [Header("�׽�Ʈ!")] - ���
    // [Space(20f)]        - �Ÿ�
    // [Range(0.0f, 5.0f)] // - ����
    float UpRange = 0.0f; // �ؽ�Ʈ�� ���� �ö󰡴� �ӵ�
    // ���� �κ��� ���� �����ش�. (���� ����)

    private void Start()
    {
        cam = Camera.main;
    }

    public void Init(Vector3 pos, double dmg, bool Critical = false)
    {
        // �ؽ�Ʈ �ؽ�Ʈ ��ġ ���� ���� - ���ü� ������ ����
        pos.x += Random.Range(-0.3f, 0.3f);
        pos.z += Random.Range(-0.3f, 0.3f);

        target = pos;  // Ÿ���� ��ġ ����
        m_Text.text = dmg.ToString(); // DMG �ؽ�Ʈ ǥ��
        transform.parent = Base_Canvas.instance.HOLDER_LAYER(1); // ĵ������ �ڽ����� ����

        m_Critical.SetActive(Critical);

        // ����� ������ 2�� �Ŀ� ��ȯ
        Base_Mng.instance.Return_Pool(2.0f, this.gameObject, "HIT_TEXT"); 
    }

    private void Update()
    {
        // �ؽ�Ʈ�� ���ͺ��� ���� ���̰� ����
        Vector3 tragetPost = new Vector3(target.x, target.y + UpRange, target.z);
        // ������ǥ�� ȭ�鿡 ǥ��
        transform.position = cam.WorldToScreenPoint(tragetPost); 
        if (UpRange <= 0.3f)
        {
            // �ǰ� �ؽ�Ʈ�� ���� �ö󰡴� �ӵ�
            UpRange += Time.deltaTime;
        }
    }

    // ������Ʈ Ǯ�� ��ȯ
    private void ReturnText()
    {
        Base_Mng.Pool.m_pool_Dictionary["HIT_TEXT"].Return(this.gameObject);
    }
}

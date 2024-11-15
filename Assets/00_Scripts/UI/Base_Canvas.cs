using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Canvas : MonoBehaviour
{
    public static Base_Canvas instance = null;

    // ������ ��ġ ������ �����ϱ� ���� �Լ�
    public Transform COIN;

    // ������ �����ϱ� ���� ���̾� ����
    [SerializeField] private Transform LAYER;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    // ���̾��� ���� ���� - value�� �������� �Ʒ��ʿ� �򸰴�.
    public Transform HOLDER_LAYER(int value)
    {
        return LAYER.GetChild(value);
    }
}

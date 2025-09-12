using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("Ҫ������ɵ�Prefab")]
    public GameObject[] prefabs;

    [Header("��������")]
    public int count = 20;

    [Header("���������С (X=��, Y=��, Z=��)")]
    public Vector3 areaSize = new Vector3(10, 0, 10);

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            // ������Χ������һ�����ƫ��
            Vector3 randomOffset = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2),
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            // ����λ�� = �������ĵ�(�ű�����λ��) + ���ƫ��
            Vector3 pos = transform.position + randomOffset;

            // �����ѡһ��Prefab������
            Instantiate(
                prefabs[Random.Range(0, prefabs.Length)],
                pos,
                Quaternion.identity
            );
        }
    }

    // ��Scene��ͼ����������������ɫ���飨ֻ��ѡ������ʱ��ʾ��
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, areaSize);
    }
}

using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [Header("要随机生成的Prefab")]
    public GameObject[] prefabs;

    [Header("生成数量")]
    public int count = 20;

    [Header("生成区域大小 (X=宽, Y=高, Z=深)")]
    public Vector3 areaSize = new Vector3(10, 0, 10);

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            // 在区域范围内生成一个随机偏移
            Vector3 randomOffset = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2),
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );

            // 生成位置 = 区域中心点(脚本物体位置) + 随机偏移
            Vector3 pos = transform.position + randomOffset;

            // 随机挑选一个Prefab并生成
            Instantiate(
                prefabs[Random.Range(0, prefabs.Length)],
                pos,
                Quaternion.identity
            );
        }
    }

    // 在Scene视图里绘制生成区域的绿色方块（只在选中物体时显示）
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, areaSize);
    }
}

using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject[] prefabs;
    public int count = 20;
    public Vector3 areaSize = new Vector3(10, 0, 10);

    void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Vector3 randomOffset = new Vector3(
                Random.Range(-areaSize.x / 2, areaSize.x / 2),
                Random.Range(-areaSize.y / 2, areaSize.y / 2),
                Random.Range(-areaSize.z / 2, areaSize.z / 2)
            );
            
            Vector3 pos = transform.position + randomOffset;
            
            Vector3 randomEuler = new Vector3(
                Random.Range(0f, 360f),   // X轴随机角度
                Random.Range(0f, 360f),   // Y轴随机角度
                Random.Range(0f, 360f)    // Z轴随机角度
            );

            Instantiate(
                prefabs[Random.Range(0, prefabs.Length)],
                pos,
                Quaternion.Euler(randomEuler) // 欧拉角转四元数
            );
        }
    }
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.3f);
        Gizmos.DrawCube(transform.position, areaSize);
    }
}

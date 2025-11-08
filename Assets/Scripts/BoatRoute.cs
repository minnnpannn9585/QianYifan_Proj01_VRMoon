using UnityEngine;

public class BoatRoute : MonoBehaviour
{
    Transform boat; 
    Transform[] points;
    public float speed;
    public float rotateSpeed = 90f; 
    private int index = 0;
    public bool canMove = true;
    public GameObject[] fishes;

    void Start()
    {
        points = new Transform[6];
        boat = transform.GetChild(0); 
        for(int i = 0; i < transform.childCount - 1 ; i++)
        {
            points[i] = transform.GetChild(i + 1);
        }
    }

    void Update()
    {
        if (index >= transform.childCount - 1)
        {
            return;
        }

        if (!canMove)
        {
            return;
        }
	    
        Vector3 dir = points[index].position - boat.position; 

        // 移动逻辑
        if (dir.sqrMagnitude > 0.000001f)
        {
            boat.position += dir.normalized * speed * Time.deltaTime;
        }

        // 旋转逻辑
        Vector3 horizontalDir = new Vector3(dir.x, 0, dir.z).normalized;
        
        if (horizontalDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalDir, Vector3.up);
            boat.rotation = Quaternion.RotateTowards( // 旋转boat
                boat.rotation, 
                targetRotation, 
                rotateSpeed * Time.deltaTime
            );
        }

        // 到达目标点切换索引
        if (Vector3.Magnitude(dir) < 0.1f)
        {
            canMove = false;
            fishes[index].SetActive(true);
            index++;
        }
    }
}
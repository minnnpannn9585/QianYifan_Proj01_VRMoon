using UnityEngine;

public class RandomFloatMovement : MonoBehaviour
{
    [Header("运动参数")]
    public Vector2 moveRange = new Vector2(3f, 2f); // x:左右范围, y:上下范围
    public float moveSpeed = 1f;
    public float waitTime = 1f;
    public float arriveDistance = 0.1f;

    [Header("旋转参数")]
    [Tooltip("每个轴的随机旋转速度范围（度/秒）")]
    public Vector2 rotationSpeedRange = new Vector2(-5f, 5f); // 负数值为反向旋转
    [Tooltip("旋转速度变化的间隔时间（秒）")]
    public float rotationChangeInterval = 2f;

    private Vector3 originalPos;
    private Vector3 targetPos;
    private float waitTimer;
    
    private Vector3 currentRotationSpeed; // 当前旋转速度（x,y,z轴分别的速度）
    private float rotationChangeTimer;    // 旋转速度变化计时器

    void Start()
    {
        originalPos = transform.position;
        GenerateNewTarget();
        
        // 初始化旋转速度
        GenerateNewRotationSpeed();
    }

    void Update()
    {
        // 旋转逻辑：定时更新旋转速度并应用旋转
        UpdateRotation();

        // 移动逻辑
        if (waitTimer > 0)
        {
            waitTimer -= Time.deltaTime;
            return;
        }

        float distanceToTarget = Vector3.Distance(transform.position, targetPos);
        if (distanceToTarget < arriveDistance)
        {
            waitTimer = waitTime;
            GenerateNewTarget();
            return;
        }

        transform.position = Vector3.MoveTowards(
            transform.position, 
            targetPos, 
            moveSpeed * Time.deltaTime
        );
    }

    // 处理旋转逻辑：定时更新旋转速度并旋转物体
    void UpdateRotation()
    {
        // 累计计时器，到达间隔时间则更新旋转速度
        rotationChangeTimer += Time.deltaTime;
        if (rotationChangeTimer >= rotationChangeInterval)
        {
            GenerateNewRotationSpeed();
            rotationChangeTimer = 0;
        }

        // 应用旋转（绕自身轴旋转，使用当前旋转速度）
        transform.Rotate(
            currentRotationSpeed.x * Time.deltaTime,
            currentRotationSpeed.y * Time.deltaTime,
            currentRotationSpeed.z * Time.deltaTime,
            Space.Self // 绕自身坐标系旋转
        );
    }

    // 生成新的随机旋转速度（x,y,z轴分别随机）
    void GenerateNewRotationSpeed()
    {
        currentRotationSpeed = new Vector3(
            Random.Range(rotationSpeedRange.x, rotationSpeedRange.y),
            Random.Range(rotationSpeedRange.x, rotationSpeedRange.y),
            Random.Range(rotationSpeedRange.x, rotationSpeedRange.y)
        );
    }

    void GenerateNewTarget()
    {
        float randomX = Random.Range(-moveRange.x, moveRange.x);
        float randomY = Random.Range(-moveRange.y, moveRange.y);

        targetPos = new Vector3(
            originalPos.x + randomX,
            originalPos.y + randomY,
            originalPos.z
        );
    }
}
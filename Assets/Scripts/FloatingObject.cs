using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [Header("Floating Settings")]
    [Tooltip("上下浮动的最大位移（单位：米）")]
    public float amplitude = 0.5f;

    [Tooltip("浮动频率（Hz），即每秒周期数）")]
    public float frequency = 1f;

    [Tooltip("是否使用本地坐标（勾选使用 localPosition，否则使用 world position）")]
    public bool useLocalPosition = true;

    public enum MotionType { Sine, PingPong }
    [Tooltip("浮动曲线类型：Sine 为正弦平滑，PingPong 为线性往返")]
    public MotionType motion = MotionType.Sine;

    [Tooltip("浮动沿哪个轴（默认竖直向上）")]
    public Vector3 axis = Vector3.up;

    [Tooltip("相位偏移（弧度），用于不同物体间错开动作")]
    public float phase = 0f;

    private Vector3 startPos;

    void Start()
    {
        startPos = useLocalPosition ? transform.localPosition : transform.position;
        if (axis == Vector3.zero) axis = Vector3.up;
    }

    void Update()
    {
        // Sine: 使用 2πf 将 frequency 从 Hz 转为弧度速度
        float offset = 0f;
        if (motion == MotionType.Sine)
        {
            float t = Time.time * frequency * 2f * Mathf.PI + phase;
            offset = Mathf.Sin(t) * amplitude;
        }
        else // PingPong
        {
            // PingPong 产生 [0, 2*amplitude]，减去 amplitude 得到 [-amplitude, amplitude]
            offset = Mathf.PingPong(Time.time * frequency, amplitude * 2f) - amplitude;
        }

        Vector3 delta = axis.normalized * offset;
        if (useLocalPosition)
            transform.localPosition = startPos + delta;
        else
            transform.position = startPos + delta;
    }

    void OnValidate()
    {
        // 编辑器中确保合理值
        amplitude = Mathf.Max(0f, amplitude);
        frequency = Mathf.Max(0f, frequency);
        if (axis == Vector3.zero) axis = Vector3.up;
    }
}

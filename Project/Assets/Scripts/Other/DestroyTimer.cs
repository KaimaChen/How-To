using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    [SerializeField] private float m_time = 0.5f;
    private float m_endTime;

    void Awake()
    {
        m_endTime = Time.time + m_time;
    }

    void Update()
    {
        if (Time.time >= m_endTime)
            Destroy(gameObject);
    }
}
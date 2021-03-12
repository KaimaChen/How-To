using UnityEngine;

/// <summary>
/// 传输带
/// 参考：https://www.youtube.com/watch?v=hC1QZ0h4oco
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Conveyor : MonoBehaviour
{
    public float m_speed = 2;
    private Rigidbody m_rigid;

    void Awake()
    {
        m_rigid = GetComponent<Rigidbody>();
        m_rigid.isKinematic = true;
    }

    void FixedUpdate()
    {
        Vector3 origin = m_rigid.position;
        m_rigid.position += Vector3.back * m_speed * Time.fixedDeltaTime;
        m_rigid.MovePosition(origin);
    }
}

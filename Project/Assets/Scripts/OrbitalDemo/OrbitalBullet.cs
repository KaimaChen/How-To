using UnityEngine;

public class OrbitalBullet : MonoBehaviour
{
    [SerializeField] private float m_speed = 10;
    [SerializeField] private float m_life = 0.5f;

    private float m_endTime;

    private OrbitalPos m_pos;

    public void Init(Vector3 pos, bool isForward)
    {
        m_pos = OrbitalManager.Instance.Vec3ToOrbitalPos(pos);
        OrbitalManager.Instance.UpdateMovement(0, transform, ref m_pos);

        m_endTime = Time.time + m_life;
        if (isForward == false)
            m_speed = -m_speed;
    }

    private void Update()
    {
        if(Time.time >= m_endTime)
        {
            Destroy(gameObject);
            return;
        }

        OrbitalManager.Instance.UpdateMovement(m_speed, transform, ref m_pos);
    }
}
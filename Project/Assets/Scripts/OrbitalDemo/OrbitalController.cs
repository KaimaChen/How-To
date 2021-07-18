using UnityEngine;

public class OrbitalController : MonoBehaviour
{
    [SerializeField] private float m_speed;
    [SerializeField] private float m_jumpSpeed;
    [SerializeField] private Transform m_foot;

    [SerializeField] private float m_fireCd = 0.1f;
    [SerializeField] private Transform m_emitter;
    [SerializeField] private OrbitalBullet m_bullet;

    private OrbitalPos m_pos;
    private float m_speedY;
    private float m_nextFireTime;

    public bool IsForward { get => transform.localScale.z > 0; }

    private void Update()
    {
        UpdateMovement();
        UpdateJump();
        UpdateFire();
    }

    private void UpdateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float speed = horizontal * m_speed;
        OrbitalManager.Instance.UpdateMovement(speed, transform, ref m_pos);
    }

    private void UpdateJump()
    {
        if (Physics.Raycast(m_foot.position, Vector3.down, 0.1f, LayerMask.GetMask("Ground")))
        {
            if (Input.GetKeyDown(KeyCode.Space))
                m_speedY = m_jumpSpeed;
            else
                m_speedY = 0;
        }
        else
        {
            m_speedY += -9.8f * Time.deltaTime;
        }
        m_pos.m_height += m_speedY * Time.deltaTime;
        Vector3 moveY = Vector3.up * m_pos.m_height;
        transform.Translate(moveY, Space.World);
    }


    private void UpdateFire()
    {
        if (!Input.GetMouseButton(0) || Time.time < m_nextFireTime)
            return;

        m_nextFireTime = Time.time + m_fireCd;
        var bullet = Instantiate(m_bullet.gameObject).GetComponent<OrbitalBullet>();
        bullet.gameObject.SetActive(true);
        bullet.Init(m_emitter.position, IsForward);
    }
}
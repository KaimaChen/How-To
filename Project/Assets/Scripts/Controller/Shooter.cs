using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject m_bullet;
    public Transform m_spawnPos;
    public float m_moveSpeed = 1;

    private Vector3? m_targetPos;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray, out hitInfo, float.MaxValue, LayerMask.GetMask("Ground")))
                m_targetPos = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
        }

        if (Input.GetMouseButtonDown(1))
        {
            var go = Instantiate(m_bullet);
            go.transform.rotation = transform.rotation;
            go.transform.position = m_spawnPos.position;
        }

        if (m_targetPos == null) 
            return;

        var toTarget = m_targetPos.Value - transform.position;
        transform.rotation = Quaternion.LookRotation(toTarget, Vector3.up);
        float distance = toTarget.magnitude;
        float moveDistance = m_moveSpeed * Time.deltaTime;
        if(distance <= moveDistance)
        {
            transform.position = m_targetPos.Value;
            m_targetPos = null;
        }
        else
        {
            transform.position = transform.position + toTarget.normalized * moveDistance;
        }
    }

    private void OnDrawGizmos()
    {
        if (m_targetPos != null)
            Gizmos.DrawSphere(m_targetPos.Value, 0.1f);
    }
}
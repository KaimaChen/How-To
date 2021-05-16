using UnityEngine;

public class ScaleTunnel : MonoBehaviour
{
    public Transform m_minEnd;
    public Transform m_maxEnd;
    public float m_minScale = 0.5f;

    private Transform m_player;
    private Vector3 m_originScale;
    private float m_originHeight;
    private bool m_makeSmall;

    private void Update()
    {
        if (m_player == null) return;

        float minZ = m_minEnd.localPosition.z;
        float maxZ = m_maxEnd.localPosition.z;
        float targetZ = transform.InverseTransformPoint(m_player.position).z;
        float percent = (targetZ - minZ) / (maxZ - minZ);
        percent = Mathf.Clamp01(percent);
        float scale = m_makeSmall ? Mathf.Lerp(m_minScale, 1, percent) : Mathf.Lerp(1, 1 / m_minScale, percent);

        m_player.localScale = m_originScale * scale;
        var controller = m_player.GetComponent<CharacterController>();
        controller.height = m_originHeight * scale;
        var pos = m_player.transform.position;
        pos.y = controller.height / 2f + controller.skinWidth;
        m_player.transform.position = pos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            m_player = other.transform;
            m_originScale = m_player.localScale;
            m_originHeight = m_player.GetComponent<CharacterController>().height;

            var pos = m_player.position;
            m_makeSmall = (pos - m_maxEnd.position).sqrMagnitude < (pos - m_minEnd.position).sqrMagnitude;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (m_player == other.transform)
            m_player = null;
    }
}

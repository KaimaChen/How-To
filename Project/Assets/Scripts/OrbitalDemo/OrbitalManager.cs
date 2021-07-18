using UnityEngine;

public class OrbitalManager : MonoBehaviour
{
    public static OrbitalManager Instance;

    [SerializeField] private float m_worldRadius = 5;

    public float WorldRadius => m_worldRadius;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    public void UpdateMovement(float speed, Transform trans, ref OrbitalPos pos)
    {
        float displacement = speed * Time.deltaTime;
        float deltaRadian = displacement / m_worldRadius;
        pos.m_radian += deltaRadian;

        float radian = pos.m_radian;
        float x = m_worldRadius * Mathf.Cos(radian);
        float z = m_worldRadius * Mathf.Sin(radian);
        trans.position = new Vector3(x, pos.m_height, z);

        float angle = -Mathf.Rad2Deg * radian;
        trans.rotation = Quaternion.Euler(0, angle, 0);

        Vector3 scale = trans.localScale;
        if (speed > 0) scale.z = Mathf.Abs(scale.z);
        else if (speed < 0) scale.z = -Mathf.Abs(scale.z);
        trans.localScale = scale;
    }

    public OrbitalPos Vec3ToOrbitalPos(Vector3 vec)
    {
        float radian = Vector3.SignedAngle(vec, new Vector3(1, 0, 0), Vector3.up) * Mathf.Deg2Rad;
        return new OrbitalPos(radian, vec.y);
    }
}
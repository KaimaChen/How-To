/// <summary>
/// 位置表示方式：（角度，高度）
/// </summary>
public struct OrbitalPos
{
    public float m_radian;
    public float m_height;

    public OrbitalPos(float r, float h)
    {
        m_radian = r;
        m_height = h;
    }
}
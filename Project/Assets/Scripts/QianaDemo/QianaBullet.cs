using Pathfinding.ClipperLib;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// LOL 琪亚娜子弹（碰到墙壁后产生绕墙爆炸）
/// </summary>
public class QianaBullet : MonoBehaviour 
{
	public const float k_altitude = 0.1f;

	[SerializeField] private float m_moveSpeed = 10;
	[SerializeField] private float m_lifeTime = 10;
	[SerializeField] private LineRenderer m_contourLine;
	[SerializeField] private GameObject m_explosion;
	[SerializeField] private float m_explosionDiameter = 1;

	private float m_endTime;
	private bool m_makingExplosion;
	private Vector3[] m_contour;
	private int m_curtIndex;
	private float m_curtDist;

	void Start () 
	{
		m_endTime = Time.time + m_lifeTime;
		GetComponent<Rigidbody>().velocity = transform.forward * m_moveSpeed;
	}
	
	void Update () 
	{
        if (m_makingExplosion)
        {
			if(MakeExplosion())
				Destroy(gameObject);
		}
		else
        {
			if (Time.time > m_endTime)
				Destroy(gameObject);
		}
	}

	/// <summary>
	/// 碰到墙壁后产生绕墙爆炸
	/// </summary>
	/// <param name="other"></param>
	private void OnCollisionEnter(Collision other)
    {
		if(other.gameObject.GetComponent<Wall>() != null)
        {
			ContactPoint contack = other.contacts[0];
			var hitPos = new Vector2(contack.point.x, contack.point.z);
			var list = WallManager.Instance.AroundWall(hitPos, m_explosionDiameter / 2f);
			if(list != null && list.Count > 0)
            {
				m_makingExplosion = true;
				MakeContourLine(list, hitPos);
			}
			else
            {
				Debug.LogError("Bug: 找不到墙壁的轮廓");
            }

			//停下并隐藏子弹
			GetComponent<Renderer>().enabled = false;
			Destroy(gameObject.GetComponent<Rigidbody>());
		}
    }

	/// <summary>
	/// 生成轮廓线
	/// </summary>
	private void MakeContourLine(List<IntPoint> polygon, Vector2 hitPos)
    {
		//调整起点
		var list = ClipperUtils.Convert(polygon, k_altitude);
		Vector2 minIntersect;
		int minIndex;
		FindStartPos(list, hitPos, out minIndex, out minIntersect);

		m_contour = new Vector3[list.Length + 2];
		int index = 0;
		m_contour[index++] = new Vector3(minIntersect.x, k_altitude, minIntersect.y);
        for (int i = minIndex; i < list.Length; i++)
            m_contour[index++] = list[i];
        for (int i = 0; i < minIndex; i++)
            m_contour[index++] = list[i];
		m_contour[index++] = m_contour[0]; //首尾相连

		//设置LineRenderer
		var line = Instantiate(m_contourLine);
		line.transform.SetParent(transform);
		line.transform.position = Vector3.zero;
		line.positionCount = m_contour.Length;
		line.SetPositions(m_contour);
	}

	/// <summary>
	/// 击中处对应的轮廓点作为起点
	/// </summary>
	private void FindStartPos(Vector3[] list, Vector2 hitPos, out int minIndex, out Vector2 minIntersect)
    {
		minIntersect = list[0];
		minIndex = 0;
		float minDist = float.MaxValue;
		for (int i = 0; i < list.Length; i++)
		{
			int prev = i == 0 ? list.Length - 1 : i - 1;
			Vector2 intersect;
			float dist;
			if (Utils.PointProjToSegment(hitPos, list[prev].xz(), list[i].xz(), out intersect, out dist))
			{
				if (dist < minDist)
				{
					minDist = dist;
					minIndex = i;
					minIntersect = intersect;
				}
			}
		}
	}

	/// <summary>
	/// 在轮廓处产生爆炸
	/// </summary>
	/// <returns>是否已生成所有爆炸</returns>
	private bool MakeExplosion()
    {
		if (m_contour == null || m_curtIndex >= m_contour.Length)
			return true;

		if(m_curtIndex == m_contour.Length - 1)
        {
			GameObject go = Instantiate(m_explosion);
			go.transform.position = m_contour[m_curtIndex];
			m_contour = null;
			return true;
        }

		var toNext = m_contour[m_curtIndex + 1] - m_contour[m_curtIndex];
		float length = toNext.magnitude;
		if (length <= m_curtDist)
        {
			var go = Instantiate(m_explosion);
			go.transform.position = m_contour[m_curtIndex + 1];

			m_curtIndex++;
			m_curtDist = 0;
        }
		else
        {
			var pos = m_contour[m_curtIndex] + (toNext) / length * m_curtDist;
			var go = Instantiate(m_explosion);
			go.transform.position = pos;

			m_curtDist += m_explosionDiameter;
		}

		return false;
    }
}
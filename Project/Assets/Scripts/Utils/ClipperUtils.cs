using Pathfinding.ClipperLib;
using UnityEngine;
using System.Collections.Generic;

public static class ClipperUtils
{
	/// <summary>
	/// Clipper使用的是整数，所以要将Unity的Vector浮点转换为整数
	/// </summary>
	public const float k_precision = 100000;

    public const float k_epsilon = 0.001f;

	public static IntPoint Convert(Vector3 v)
	{
		return new IntPoint(v.x * k_precision, v.z * k_precision);
	}

	public static Vector3 Convert(IntPoint p, float y)
	{
		return new Vector3(p.X / k_precision, y, p.Y / k_precision);
	}

	public static Vector2 Convert2(IntPoint p)
	{
		return new Vector2(p.X / k_precision, p.Y / k_precision);
	}

	public static Vector3[] Convert(List<IntPoint> polygon, float y)
	{
		Vector3[] vertices = new Vector3[polygon.Count];
		for (int i = 0; i < polygon.Count; i++)
			vertices[i] = Convert(polygon[i], y);
		return vertices;
	}

    public static bool IsPointOnPolygonEdge(Vector2 p, List<IntPoint> polygon)
    {
        for (int i = 0; i < polygon.Count; i++)
        {
            int j = i == polygon.Count - 1 ? 0 : i + 1;

            Vector2 pi = Convert2(polygon[i]);
            Vector2 pj = Convert2(polygon[j]);

			Vector2 intersect;
			float dist;
			if (Utils.PointProjToSegment(p, pi, pj, out intersect, out dist) && dist < k_epsilon)
				return true;
        }

        return false;
    }
}
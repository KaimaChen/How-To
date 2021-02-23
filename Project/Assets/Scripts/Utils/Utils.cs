using UnityEngine;

public static class Utils
{
	/// <summary>
	/// 点到线段的垂直投影
	/// </summary>
	/// <param name="p"></param>
	/// <param name="a"></param>
	/// <param name="b"></param>
	/// <param name="intersect"></param>
	/// <param name="dist"></param>
	/// <returns></returns>
	public static bool PointProjToSegment(Vector2 p, Vector2 a, Vector2 b, out Vector2 intersect, out float dist)
	{
		intersect = Vector2.zero;
		dist = 0;

		float u = Vector2.Dot(p - a, b - a) / Vector2.Dot(b - a, b - a);
		if (u < 0 || u > 1)
			return false;

		intersect = a + u * (b - a);
		dist = Vector2.Distance(intersect, p);
		return true;
	}

	public static Vector2 xz(this Vector3 v)
    {
		return new Vector2(v.x, v.z);
    }
}
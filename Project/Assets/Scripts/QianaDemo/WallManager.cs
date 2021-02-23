using Pathfinding.ClipperLib;
using System.Collections.Generic;
using UnityEngine;

public class WallManager : MonoBehaviour 
{
	public static WallManager Instance { get; private set; }

	private readonly List<Wall> m_walls = new List<Wall>();

	void Awake()
    {
		Instance = this;
    }

	public void Register(Wall wall)
    {
		m_walls.Add(wall);
    }

	public void UnRegister(Wall wall)
    {
		m_walls.Remove(wall);
    }

	public List<IntPoint> AroundWall(Vector2 containPoint, float offset)
    {
		//将相邻的障碍物轮廓合并在一起
		List<List<IntPoint>> unions = new List<List<IntPoint>>();
		Clipper clipper = new Clipper();
		for (int i = 0; i < m_walls.Count; i++)
			clipper.AddPolygons(m_walls[i].m_polygons, PolyType.ptSubject);

		if(!clipper.Execute(ClipType.ctUnion, unions))
        {
			Debug.LogError("无法合并顶点");
			return null;
		}

		//过滤出特定的那个轮廓
		List<IntPoint> contour = null;
		for(int i = 0; i < unions.Count; i++)
        {
			if(ClipperUtils.IsPointOnPolygonEdge(containPoint, unions[i]))
            {
				contour = unions[i];
				break;
            }
        }
		if(contour == null)
        {
			Debug.LogError("没找到包含特定点的障碍轮廓");
			return null;
        }

		//对轮廓进行偏移
		List<List<IntPoint>> result = new List<List<IntPoint>>() { contour };
		result = Clipper.OffsetPolygons(result, offset * ClipperUtils.k_precision);
		if(result.Count > 1)
        {
			Debug.LogError("轮廓偏移后产生多个轮廓了？");
			return result[0];
		}
		else if(result.Count == 0)
        {
			Debug.LogError("轮廓偏移后不产生轮廓了？");
			return null;
        }
		else
        {
			return result[0];
		}
    }
}
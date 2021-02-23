using UnityEngine;
using System.Collections.Generic;
using Pathfinding.ClipperLib;

[RequireComponent(typeof(MeshFilter))]
public class Wall : MonoBehaviour
{
    private const float k_epsilon = 0.001f;

    public List<List<IntPoint>> m_polygons = new List<List<IntPoint>>();

    void Start()
    {
        WallManager.Instance.Register(this);

        var localToWorld = transform.localToWorldMatrix;
        var mesh = GetComponent<MeshFilter>().mesh;
        Vector3[] vertices = mesh.vertices;
        var triangles = mesh.triangles;

        //找到贴紧地面的那些三角形
        for(int i = 0; i < triangles.Length; i += 3)
        {
            var v0 = localToWorld.MultiplyPoint(vertices[triangles[i]]);
            var v1 = localToWorld.MultiplyPoint(vertices[triangles[i + 1]]);
            var v2 = localToWorld.MultiplyPoint(vertices[triangles[i + 2]]);

            if(Mathf.Abs(v0.y) < k_epsilon && Mathf.Abs(v1.y) < k_epsilon && Mathf.Abs(v2.y) < k_epsilon)
            {
                var list = new List<IntPoint>();
                m_polygons.Add(list);
                list.Add(ClipperUtils.Convert(v0));
                list.Add(ClipperUtils.Convert(v1));
                list.Add(ClipperUtils.Convert(v2));
            }
        }

        //去掉重复的顶点
        m_polygons = Clipper.SimplifyPolygons(m_polygons);
    }

    void OnDestroy()
    {
        WallManager.Instance.UnRegister(this);
    }
}
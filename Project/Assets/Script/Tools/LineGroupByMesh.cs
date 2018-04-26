using UnityEngine;
using System.Collections;

public class LineGroupByMesh : MonoBehaviour 
{
    [Header("宽度")]
    public float width = 0.2f;

    MeshFilter _meshFilter;
    MeshFilter meshFilter
    {
        get
        {
            if (_meshFilter == null)
                _meshFilter = GetComponentInChildren<MeshFilter>();
            return _meshFilter;
        }
    }


    public Vector3[] testArr;

    Vector3[] vertices;
 
    void Start()
    {
        Begin(testArr);
    }

	public void Begin(Vector3[] pointArr)
    {
        var mesh = new Mesh();
        var verticesCount = 4;
        if (pointArr.Length > 2)
            verticesCount += (pointArr.Length - 2) * 4;
        vertices = new Vector3[verticesCount];
        for (int i = 0; i < pointArr.Length; ++i)
        {
            if(i == 0)
            {
                var currentDir = (pointArr[i + 1] - pointArr[i]).normalized;
                var dir = Quaternion.Euler(Vector3.up * 90) * currentDir;
                Debug.Log(" ====   " + dir);
                vertices[i] = pointArr[i] + dir * width / 2;
                vertices[i + 1] = pointArr[i] - dir * width / 2;
            }
            else if(i == pointArr.Length - 1)
            {
                var currentDir = (pointArr[i] - pointArr[i - 1]).normalized;
                var dir = Quaternion.Euler(Vector3.up * 90) * currentDir;
                vertices[vertices.Length - 2] = pointArr[i] + dir * width / 2;
                vertices[vertices.Length - 1] = pointArr[i] - dir * width / 2;
            }
            else
            {
                var dir1 = (pointArr[i] - pointArr[i - 1]).normalized;
                var dir2 = (pointArr[i + 1] - pointArr[i]).normalized;
                var currentIndex = i * 4 - 2;
                vertices[currentIndex] = pointArr[i] + (Quaternion.Euler(Vector3.up * 90) * dir1) * width / 2;
                vertices[currentIndex + 1] = pointArr[i] - (Quaternion.Euler(Vector3.up * 90) * dir1) * width / 2;
                vertices[currentIndex + 2] = pointArr[i] + (Quaternion.Euler(Vector3.up * 90) * dir2) * width / 2;
                vertices[currentIndex + 3] = pointArr[i] - (Quaternion.Euler(Vector3.up * 90) * dir2) * width / 2;
            }
        }
        mesh.vertices = vertices;

        int[] triangles = new int[(pointArr.Length - 1) * 6];
        for(int i = 0; i < pointArr.Length - 1; ++i)
        {

            var tIndex = i * 6;
            var vIndex = i * 4;
            triangles[tIndex] = vIndex;
            triangles[tIndex + 1] = vIndex + 1;
            triangles[tIndex + 2] = vIndex + 3;
            triangles[tIndex + 3] = vIndex;
            triangles[tIndex + 4] = vIndex + 3;
            triangles[tIndex + 5] = vIndex + 2;

        }
        mesh.triangles = triangles;

        meshFilter.mesh = mesh;
    }

}

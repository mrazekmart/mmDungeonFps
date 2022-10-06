using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VecPoint
{
    public Vector3 point;
    public List<Vector3> links;
    public Vector3[,] triangles;

    public VecPoint(Vector3 point, List<Vector3> links)
    {
        this.point = point;
        this.links = links;
    }
}

public class Stone
{
    public List<Vector3> listVertices;
    public List<int> listTriangles;
    public List<VecPoint> points;

    public Stone(List<VecPoint> points)
    {
        this.points = points;
        listVertices = new List<Vector3>();
        listTriangles = new List<int>();
        UpdateVerticesAndTriangles();
    }

    public void UpdateVerticesAndTriangles()
    {
        int vertices = 0;
        listVertices.Clear();
        listTriangles.Clear();
        foreach (VecPoint point in points)
        {
            foreach (Vector3 linkPoint1 in point.links)
            {
                foreach (Vector3 linkPoint2 in point.links)
                {
                    if (linkPoint1 == linkPoint2)
                    {
                        continue;
                    }
                    listVertices.Add(point.point);
                    listVertices.Add(linkPoint1);
                    listVertices.Add(linkPoint2);
                    listTriangles.AddRange(new List<int> { 0 + vertices, 1 + vertices, 2 + vertices });
                    vertices += 3;
                }
            }
        }
    }
}



public class CreateRandomStone : MonoBehaviour
{
    //private List<Vector2> listUVsCeiling = new List<Vector2>();

    private Mesh meshStone;
    private Stone stone;

    void Start()
    {
        meshStone = new Mesh();
        meshStone.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        GetComponent<MeshFilter>().mesh = meshStone;

        CreateBaseCube();
        UpdateMeshes();
    }

    private void CreateBaseCube()
    {

        Vector3 A = new Vector3(0, 0, 0);
        Vector3 B = new Vector3(Random.Range(0.1f, 3f), 0, 0);
        Vector3 C = new Vector3(Random.Range(0.1f, 3f), Random.Range(0.1f, 3f), 0);
        Vector3 D = new Vector3(0, Random.Range(0.1f, 3f), 0);
        Vector3 E = new Vector3(0, 0, Random.Range(0.1f, 3f));
        Vector3 F = new Vector3(Random.Range(0.1f, 3f), 0, Random.Range(0.1f, 3f));
        Vector3 G = new Vector3(Random.Range(0.1f, 3f), Random.Range(0.1f, 3f), Random.Range(0.1f, 3f));
        Vector3 H = new Vector3(0, Random.Range(0.1f, 3f), Random.Range(0.1f, 3f));



        VecPoint pointA = new VecPoint(A, new List<Vector3>() { B, F, E, C, D, H });
        VecPoint pointB = new VecPoint(B, new List<Vector3>() { F, E, A, G, C, D });
        VecPoint pointC = new VecPoint(C, new List<Vector3>() { D, A, B, F, G, H });
        VecPoint pointD = new VecPoint(D, new List<Vector3>() { A, B, C, E, G, H });
        VecPoint pointE = new VecPoint(E, new List<Vector3>() { A, B, F, H, D, G });
        VecPoint pointF = new VecPoint(F, new List<Vector3>() { A, E, B, G, C, H });
        VecPoint pointG = new VecPoint(G, new List<Vector3>() { H, E, F, C, B, D });
        VecPoint pointH = new VecPoint(H, new List<Vector3>() { D, C, G, A, E, F });



        List<VecPoint> points = new List<VecPoint>();
        points.Add(pointA);
        points.Add(pointB);
        points.Add(pointC);
        points.Add(pointD);
        points.Add(pointE);
        points.Add(pointF);
        points.Add(pointG);
        points.Add(pointH);


        stone = new Stone(points);


    }
    private void UpdateMeshes()
    {

        meshStone.Clear();
        meshStone.vertices = stone.listVertices.ToArray();
        meshStone.triangles = stone.listTriangles.ToArray();
        //meshStone.uv = listUVsWalls.ToArray();
        meshStone.RecalculateNormals();
        meshStone.Optimize();
        meshStone.RecalculateBounds();
        MeshCollider stoneMeshCollider = GetComponent<MeshCollider>();
        stoneMeshCollider.sharedMesh = meshStone;
    }
}

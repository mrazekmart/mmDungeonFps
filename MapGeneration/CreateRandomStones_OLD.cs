using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateRandomStones_OLD : MonoBehaviour
{
    private Mesh meshStones;

    private List<Vector3> listVerticesStones = new List<Vector3>();
    private List<int> listTrianglesStones = new List<int>();
    private List<Vector2> listUVsStones = new List<Vector2>();


    private List<Vector2> f = new List<Vector2>();
    private List<Vector2> a = new List<Vector2>();

    //public int numberOfVertices = 12;

    private void Awake()
    {
        meshStones = new Mesh();
        meshStones.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = meshStones;

        CreateStonesMesh();
        UpdateStonesMesh();
    }

    public float refresh = 5f;
    private void FixedUpdate()
    {

        //this is for realtime testing

        /*        refresh -= Time.deltaTime;
                if (refresh < 0)
                {
                    f = new List<Vector2>();
                    refresh = 5f;
                    CreateStonesMesh();
                }*/
    }
    private void CreateStonesMesh() 
    {
        int verticesHeight = 6;
        //int verticesHeight = Random.Range(3, 11);
        int[] verticesPerFloor = new int[verticesHeight];
        List<Vector2[]> sortedVertices = new List<Vector2[]>();

        for (int i = 0; i < verticesHeight; i++)
        {

            //int numberOfVertices = Random.Range(5, 20);
            int numberOfVertices = 6;
            verticesPerFloor[i] = numberOfVertices;
            Vector2[] listOfVertices = new Vector2[numberOfVertices];

            for (int j = 0; j < verticesPerFloor[i]; j++)
            {
                float x = Random.Range(0, 50);
                float z = Random.Range(0, 50);
                Vector2 currentPoint = new Vector2(x, z);
                listOfVertices[j] = currentPoint;
            }
            sortedVertices.Add(ConvexHullA(listOfVertices));
        }


        Vector3 firstPointForFace = new Vector3(sortedVertices[0][0].x, 0, sortedVertices[0][0].y);
        int verts = 0;
        int heightMult = 10;

        //Do bottom face
        for (int i = 0; i < sortedVertices[0].Length - 2; i++)
        {
            Vector3 nextPointFirst = new Vector3(sortedVertices[0][i + 1].x, 0 * heightMult, sortedVertices[0][i + 1].y);
            Vector3 nextPointSecond = new Vector3(sortedVertices[0][i + 2].x, 0 * heightMult, sortedVertices[0][i + 2].y);

            listVerticesStones.Add(firstPointForFace);
            listVerticesStones.Add(nextPointFirst);
            listVerticesStones.Add(nextPointSecond);
            listTrianglesStones.AddRange(new List<int> { 0 + verts, 1 + verts, 2 + verts });
            verts += 3;
        }


        for (int i = 0; i < sortedVertices.Count - 1; i++)
        {

            for (int j = 0; j < sortedVertices[i].Length; j++)
            {
                if (i < sortedVertices.Count - 1)
                {
                    foreach (Vector2 v1 in sortedVertices[i + 1])
                    {
                        foreach (Vector2 v2 in sortedVertices[i + 1])
                        {
                            if (v1.x == v2.x && v1.y == v2.y)
                            {
                                Debug.Log("i dont klow");
                                continue;
                            }

                            listVerticesStones.Add(new Vector3(sortedVertices[i][j].x, i * heightMult, sortedVertices[i][j].y));
                            listVerticesStones.Add(new Vector3(v1.x, (i + 1) * heightMult, v1.y));
                            listVerticesStones.Add(new Vector3(v2.x, (i + 1) * heightMult, v2.y));
                            listTrianglesStones.AddRange(new List<int> { 0 + verts, 1 + verts, 2 + verts });
                            verts += 3;
                        }
                    }

                }
                if (i > 0)
                {
                    foreach (Vector3 v1 in sortedVertices[i - 1])
                    {
                        foreach (Vector3 v2 in sortedVertices[i - 1])
                        {
                            if (v1 == v2)
                            {
                                continue;
                            }

                            listVerticesStones.Add(new Vector3(sortedVertices[i][j].x, i * heightMult, sortedVertices[i][j].y));
                            listVerticesStones.Add(new Vector3(v1.x, (i - 1) * heightMult, v1.y));
                            listVerticesStones.Add(new Vector3(v2.x, (i - 1) * heightMult, v2.y));
                            listTrianglesStones.AddRange(new List<int> { 0 + verts, 1 + verts, 2 + verts });
                            verts += 3;
                        }
                    }
                }





                /*
                                if(i < sortedVertices.Count - 1)
                                {
                                    Vector3 currentPoint = new Vector3(sortedVertices[i][j].x, i * heightMult, sortedVertices[i][j].y);
                                    Vector3 firstClosest = new Vector3(sortedVertices[i + 1][0].x, (i + 1) * heightMult, sortedVertices[i + 1][0].y);
                                    float firstClosestDistance = 100;
                                    Vector3 secondClosest = new Vector3(sortedVertices[i + 1][1].x, (i + 1) * heightMult, sortedVertices[i + 1][1].y);
                                    float secondClosestDistance = 100;

                                    for (int k = 0; k < sortedVertices[i + 1].Length; k++)
                                    {
                                        Vector3 pointToCheck = new Vector3(sortedVertices[i + 1][k].x, (i + 1) * heightMult, sortedVertices[i + 1][k].y);
                                        float distance = Vector3.Distance(currentPoint, pointToCheck);
                                        Debug.Log(distance);
                                        if(distance < secondClosestDistance)
                                        {
                                            secondClosest = pointToCheck;
                                            secondClosestDistance = distance;
                                        }
                                        if(distance < firstClosestDistance)
                                        {
                                            secondClosest = firstClosest;
                                            secondClosestDistance = firstClosestDistance;
                                            firstClosest = pointToCheck;
                                            firstClosestDistance = distance;
                                        }
                                    }

                                    listVerticesStones.Add(currentPoint);
                                    listVerticesStones.Add(firstClosest);
                                    listVerticesStones.Add(secondClosest);
                                    listTrianglesStones.AddRange(new List<int> { verts * vertsIter, 2 + verts * vertsIter, 1 + verts * vertsIter });
                                    vertsIter++;
                                }

                                if(i > 0)
                                {
                                    Vector3 currentPoint = new Vector3(sortedVertices[i][j].x, i * heightMult, sortedVertices[i][j].y);
                                    Vector3 firstClosest = new Vector3(sortedVertices[i - 1][0].x, (i - 1) * heightMult, sortedVertices[i - 1][0].y);
                                    float firstClosestDistance = 100;
                                    Vector3 secondClosest = new Vector3(sortedVertices[i - 1][1].x, (i - 1) * heightMult, sortedVertices[i - 1][1].y);
                                    float secondClosestDistance = 100;

                                    for (int k = 0; k < sortedVertices[i - 1].Length; k++)
                                    {
                                        Vector3 pointToCheck = new Vector3(sortedVertices[i - 1][k].x, (i - 1) * heightMult, sortedVertices[i - 1][k].y);
                                        float distance = Vector3.Distance(currentPoint, pointToCheck);
                                        Debug.Log(distance);
                                        if (distance < secondClosestDistance)
                                        {
                                            secondClosest = pointToCheck;
                                            secondClosestDistance = distance;
                                        }
                                        if (distance < firstClosestDistance)
                                        {
                                            secondClosest = firstClosest;
                                            secondClosestDistance = firstClosestDistance;
                                            firstClosest = pointToCheck;
                                            firstClosestDistance = distance;
                                        }
                                    }

                                    listVerticesStones.Add(currentPoint);
                                    listVerticesStones.Add(firstClosest);
                                    listVerticesStones.Add(secondClosest);
                                    listTrianglesStones.AddRange(new List<int> { verts * vertsIter, 2 + verts * vertsIter, 1 + verts * vertsIter });
                                    vertsIter++;
                                }
                */
            }

        }


    }
    private void UpdateStonesMesh()
    {
        meshStones.Clear();
        meshStones.vertices = listVerticesStones.ToArray();
        meshStones.triangles = listTrianglesStones.ToArray();
        meshStones.uv = listUVsStones.ToArray();
        meshStones.RecalculateNormals();
        meshStones.Optimize();
        meshStones.RecalculateBounds();
        MeshCollider meshColStones = GetComponent<MeshCollider>();
        meshColStones.sharedMesh = meshStones;

    }

    private void OnDrawGizmos()
    {
        for (int i = 0; i < a.Count; i++)
        {
            Gizmos.DrawSphere(new Vector3(a[0 + i].x, 1, a[0 + i].y), .1f);
        }
        if (f.Count > 0)
        {
            for (int i = 0; i < f.Count - 1; i++)
            {
                Gizmos.DrawLine(new Vector3(f[0 + i].x, 1, f[0 + i].y), new Vector3(f[1 + i].x, 1, f[1 + i].y));
            }
            Gizmos.DrawLine(new Vector3(f[f.Count - 1].x, 1, f[f.Count - 1].y), new Vector3(f[0].x, 1, f[0].y));

        }
    }
    /*private void FindPath(List<Vector2> listOfVertices)
    {
        Vector2 start = listOfVertices[0];
        List<Vector2> sorted = new List<Vector2>();
        List<Vector2> remaining = new List<Vector2>(listOfVertices);

        sorted.Add(start);
        remaining.RemoveAt(0);
        FindNext(sorted, remaining);

    }*/
    /*private void FindNext(List<Vector2> sorted, List<Vector2> remaining)
    {
        if (f.Count > 0)
        {
            return;
        }

        Vector2 curr = sorted[sorted.Count - 1];

        foreach (Vector2 c in remaining)
        {
            bool canAdd = true;
            if (sorted.Count >= 2)
            {
                for (int i = 0; i < sorted.Count - 1; i++)
                {
                    Vector2 pointA = sorted[i];
                    Vector2 pointB = sorted[i + 1];

                    if (AreLinesIntersecting(pointA, pointB, curr, c, true))
                    {
                        canAdd = false;
                        break;
                    }
                    if (AreLinesIntersecting(pointA, pointB, c, sorted[0], true))
                    {
                        canAdd = false;
                        break;
                    }
                }
            }
            if (canAdd)
            {
                List<Vector2> currentSorted = new List<Vector2>(sorted);
                currentSorted.Add(c);
                List<Vector2> currentRemaining = new List<Vector2>(remaining);
                currentRemaining.Remove(c);
                if (currentRemaining.Count == 0)
                {
                    f = currentSorted;
                    return;
                }
                else
                {
                    FindNext(currentSorted, currentRemaining);
                }
            }
            else
            {
                return;
            }
        }
        return;
    }*/


    /*public bool AreLinesIntersecting(Vector2 l1_p1, Vector2 l1_p2, Vector2 l2_p1, Vector2 l2_p2, bool shouldIncludeEndPoints)
    {
        float epsilon = 0.00001f;

        bool isIntersecting = false;

        float denominator = (l2_p2.y - l2_p1.y) * (l1_p2.x - l1_p1.x) - (l2_p2.x - l2_p1.x) * (l1_p2.y - l1_p1.y);

        if (denominator != 0f)
        {
            float u_a = ((l2_p2.x - l2_p1.x) * (l1_p1.y - l2_p1.y) - (l2_p2.y - l2_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;
            float u_b = ((l1_p2.x - l1_p1.x) * (l1_p1.y - l2_p1.y) - (l1_p2.y - l1_p1.y) * (l1_p1.x - l2_p1.x)) / denominator;

            if (shouldIncludeEndPoints)
            {
                if (u_a >= 0f + epsilon && u_a <= 1f - epsilon && u_b >= 0f + epsilon && u_b <= 1f - epsilon)
                {
                    isIntersecting = true;
                }
            }
            else
            {
                if (u_a > 0f + epsilon && u_a < 1f - epsilon && u_b > 0f + epsilon && u_b < 1f - epsilon)
                {
                    isIntersecting = true;
                }
            }
        }

        return isIntersecting;
    }*/

    public int OrientationHullA(Vector2 p, Vector2 q, Vector2 r)
    {
        int val = ((int)q.y - (int)p.y) * ((int)r.x - (int)q.x) -
                ((int)q.x - (int)p.x) * ((int)r.y - (int)q.y);

        if (val == 0) return 0;
        return (val > 0) ? 1 : 2;
    }

    public Vector2[] ConvexHullA(Vector2[] points)
    {
        int n = points.Length;

        List<Vector2> hull = new List<Vector2>();

        int l = 0;
        for (int i = 1; i < n; i++)
        {
            if (points[i].x < points[l].x)
            {
                l = i;
            }
        }

        int p = l, q;
        do
        {
            hull.Add(points[p]);
            q = (p + 1) % n;

            for (int i = 0; i < n; i++)
            {
                if (OrientationHullA(points[p], points[i], points[q]) == 2)
                {
                    q = i;
                }
            }
            p = q;

        } while (p != l);

        f.AddRange(hull);
        return hull.ToArray();
    }
}

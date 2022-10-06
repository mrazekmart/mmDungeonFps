using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

[RequireComponent(typeof(MeshFilter))]
public class SpawnGround : MonoBehaviour
{

    //TODO: list:
    // fix deficit
    public int size = 10;
    public int height = 3;
    [Range(0.1f, 1f)]
    public float cornersSize = 0.5f;
    [Range(-1f, 1f)]
    public float deficit = 0.3f;

    [Range(-1.0f, 1f)]
    public float noiseScale = 0.9f;
    [Range(-1.0f, 1f)]
    public float perlin = 0.5f;
    [Range(0f, 10f)]
    public float perlinMultiplier = 0.5f;
    [Range(-2f, 2f)]
    public float perlinCornerMultiplier = 0.5f;
    [Range(0.1f, 1f)]
    public float noiseWallsScale = 0.3f;
    [Range(0.1f, 1f)]
    public float cornersNoiseWallsScale = 0.3f;
    [Range(1, 10)]
    public int sizeOfPassage = 3;

    //private Mesh meshGround;
    private Mesh meshCeiling;
    private Mesh meshWalls;
    private Mesh meshCorners;


    //public GameObject groundObject;
    public GameObject ceilingObject;
    public GameObject wallsObject;
    public GameObject cornersObject;
    public NavMeshSurface surface;


    //private List<Vector3> listVerticesGround = new List<Vector3>();
    //private List<int> listTrianglesGround = new List<int>();
    //private List<Vector2> listUVsGround = new List<Vector2>();

    private List<Vector3> listVerticesCeiling = new List<Vector3>();
    private List<int> listTrianglesCeiling = new List<int>();
    private List<Vector2> listUVsCeiling = new List<Vector2>();

    private List<Vector3> listVerticesWalls = new List<Vector3>();
    private List<int> listTrianglesWalls = new List<int>();
    private List<Vector2> listUVsWalls = new List<Vector2>();

    private List<Vector3> listVerticesCorners = new List<Vector3>();
    private List<int> listTrianglesCorners = new List<int>();
    private List<Vector2> listUVsCorners = new List<Vector2>();

    private static readonly int CUBE_VERTS = 24;
    private int vertices = 0;

    public int[,] mapMinimap;


    int randomX = 0;
    int randomZ = 0;

    void Awake()
    {
        /*        randomX = 200;
                randomZ = 200;*/
        randomX = Mathf.FloorToInt(Random.Range(0.0f, 10000.0f));
        randomZ = Mathf.FloorToInt(Random.Range(0.0f, 10000.0f));

        //meshGround = new Mesh();
        meshCeiling = new Mesh();
        meshWalls = new Mesh();
        meshCorners = new Mesh();

        //meshGround.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCeiling.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshWalls.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshCorners.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        //groundObject.GetComponent<MeshFilter>().mesh = meshGround;
        ceilingObject.GetComponent<MeshFilter>().mesh = meshCeiling;
        wallsObject.GetComponent<MeshFilter>().mesh = meshWalls;
        cornersObject.GetComponent<MeshFilter>().mesh = meshCorners;

        mapMinimap = new int[size, size];

        //CreateShape();
        //UpdateMesh();
        CreateMaze();
        //CreateGroundMesh();
        CreateCeilingMesh();
        CreateWallsMesh();
        //SmoothCornersMesh();
        UpdateMeshes();

        surface.BuildNavMesh();
        //setting minimap in Minimap object
        Minimap.Instance.MapMinimap = mapMinimap;
    }

    public float refresh = 5f;
    float refreshC = 0;

    private void Start()
    {
        refreshC = refresh;
    }
    private void FixedUpdate()
    {


        /*        List<int[]> test = new List<int[]>();
                int[] testInt = new int[2] { 1, 3 };
                test.Add(testInt);
                test.Add(new int[2] { 1, 4 });
                Debug.Log(test.Contains(testInt));
                Debug.Log(test.Contains(new int[2] { 1, 4 }));

                int[] testInt2 = new int[2] { 1, 3 };
                Debug.Log(test.Contains(testInt2));

                */


        //this is for realtime testing

        /*        refreshC -= Time.deltaTime;
                if (refreshC < 0)
                {
                    randomX = Mathf.FloorToInt(Random.Range(0.0f, 10000.0f));
                    randomZ = Mathf.FloorToInt(Random.Range(0.0f, 10000.0f));
                    CreateMaze();
                    *//*            vertices = 0;
                                listVerticesWalls.Clear();
                                listTrianglesWalls.Clear();
                                listUVsWalls.Clear();
                                CreateWallsMesh();*//*
                    UpdateMeshes();
                    refreshC = refresh;
                }*/
    }
    private void CreateWallsMesh()
    {
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                for (int y = 1; y < height; y++)
                {
                    if (mapMinimap[x, z] == 1)
                    {

                        if (x == 0)
                        {
                            doLeftSide(x, z, y);
                        }
                        else if (mapMinimap[x - 1, z] == 0)
                        {
                            doLeftSide(x, z, y);
                        }
                        if (x == size - 1)
                        {
                            doRightSide(x, z, y);
                        }
                        else if (mapMinimap[x + 1, z] == 0)
                        {
                            doRightSide(x, z, y);
                        }
                        if (z == 0)
                        {
                            doFrontSide(x, z, y);
                        }
                        else if (mapMinimap[x, z - 1] == 0)
                        {
                            doFrontSide(x, z, y);
                        }
                        if (z == size - 1)
                        {
                            doBackSide(x, z, y);
                        }
                        else if (mapMinimap[x, z + 1] == 0)
                        {
                            doBackSide(x, z, y);
                        }
                    }
                }
            }
        }
    }
    /*private void CreateGroundMesh()
    {
        int i = 0;
        int y = 0;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                listVerticesGround.Add(new Vector3(x + 0, y + 0, z + 0)); //0
                listVerticesGround.Add(new Vector3(x + 1, y + 0, z + 0)); //1
                listVerticesGround.Add(new Vector3(x + 1, y + 1, z + 0)); //2
                listVerticesGround.Add(new Vector3(x + 0, y + 1, z + 0)); //3

                listVerticesGround.Add(new Vector3(x + 1, y + 0, z + 0)); //4
                listVerticesGround.Add(new Vector3(x + 1, y + 0, z + 1)); //5
                listVerticesGround.Add(new Vector3(x + 1, y + 1, z + 1)); //6
                listVerticesGround.Add(new Vector3(x + 1, y + 1, z + 0)); //7

                listVerticesGround.Add(new Vector3(x + 1, y + 0, z + 1)); //8
                listVerticesGround.Add(new Vector3(x + 0, y + 0, z + 1)); //9
                listVerticesGround.Add(new Vector3(x + 0, y + 1, z + 1)); //10
                listVerticesGround.Add(new Vector3(x + 1, y + 1, z + 1)); //11

                listVerticesGround.Add(new Vector3(x + 0, y + 0, z + 1)); //12
                listVerticesGround.Add(new Vector3(x + 0, y + 0, z + 0)); //13
                listVerticesGround.Add(new Vector3(x + 0, y + 1, z + 0)); //14
                listVerticesGround.Add(new Vector3(x + 0, y + 1, z + 1)); //15

                listVerticesGround.Add(new Vector3(x + 0, y + 1, z + 0)); //16
                listVerticesGround.Add(new Vector3(x + 1, y + 1, z + 0)); //17
                listVerticesGround.Add(new Vector3(x + 1, y + 1, z + 1)); //18
                listVerticesGround.Add(new Vector3(x + 0, y + 1, z + 1)); //19

                listVerticesGround.Add(new Vector3(x + 0, y + 0, z + 0)); //20
                listVerticesGround.Add(new Vector3(x + 1, y + 0, z + 0)); //21
                listVerticesGround.Add(new Vector3(x + 1, y + 0, z + 1)); //22
                listVerticesGround.Add(new Vector3(x + 0, y + 0, z + 1)); //23


                listTrianglesGround.AddRange(new List<int> { 0 + CUBE_VERTS * i, 2 + CUBE_VERTS * i, 1 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 0 + CUBE_VERTS * i, 3 + CUBE_VERTS * i, 2 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 17 + CUBE_VERTS * i, 16 + CUBE_VERTS * i, 19 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 17 + CUBE_VERTS * i, 19 + CUBE_VERTS * i, 18 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 4 + CUBE_VERTS * i, 7 + CUBE_VERTS * i, 6 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 4 + CUBE_VERTS * i, 6 + CUBE_VERTS * i, 5 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 13 + CUBE_VERTS * i, 12 + CUBE_VERTS * i, 15 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 13 + CUBE_VERTS * i, 15 + CUBE_VERTS * i, 14 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 11 + CUBE_VERTS * i, 10 + CUBE_VERTS * i, 9 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 11 + CUBE_VERTS * i, 9 + CUBE_VERTS * i, 8 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 20 + CUBE_VERTS * i, 22 + CUBE_VERTS * i, 23 + CUBE_VERTS * i });
                listTrianglesGround.AddRange(new List<int> { 20 + CUBE_VERTS * i, 21 + CUBE_VERTS * i, 22 + CUBE_VERTS * i });


                listUVsGround.Add(new Vector2(0, 0));
                listUVsGround.Add(new Vector2(1, 0));
                listUVsGround.Add(new Vector2(1, 1));
                listUVsGround.Add(new Vector2(0, 1));
                listUVsGround.Add(new Vector2(0, 0));
                listUVsGround.Add(new Vector2(1, 0));
                listUVsGround.Add(new Vector2(1, 1));
                listUVsGround.Add(new Vector2(0, 1));
                listUVsGround.Add(new Vector2(0, 0));
                listUVsGround.Add(new Vector2(1, 0));
                listUVsGround.Add(new Vector2(1, 1));
                listUVsGround.Add(new Vector2(0, 1));
                listUVsGround.Add(new Vector2(0, 0));
                listUVsGround.Add(new Vector2(1, 0));
                listUVsGround.Add(new Vector2(1, 1));
                listUVsGround.Add(new Vector2(0, 1));
                listUVsGround.Add(new Vector2(0, 0));
                listUVsGround.Add(new Vector2(1, 0));
                listUVsGround.Add(new Vector2(1, 1));
                listUVsGround.Add(new Vector2(0, 1));
                listUVsGround.Add(new Vector2(0, 0));
                listUVsGround.Add(new Vector2(1, 0));
                listUVsGround.Add(new Vector2(1, 1));
                listUVsGround.Add(new Vector2(0, 1));
                i++;
            }
        }
    }*/
    private void CreateCeilingMesh()
    {
        int i = 0;
        int y = height;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                listVerticesCeiling.Add(new Vector3(x + 0, y + 0, z + 0)); //0
                listVerticesCeiling.Add(new Vector3(x + 1, y + 0, z + 0)); //1
                listVerticesCeiling.Add(new Vector3(x + 1, y + 1, z + 0)); //2
                listVerticesCeiling.Add(new Vector3(x + 0, y + 1, z + 0)); //3

                listVerticesCeiling.Add(new Vector3(x + 1, y + 0, z + 0)); //4
                listVerticesCeiling.Add(new Vector3(x + 1, y + 0, z + 1)); //5
                listVerticesCeiling.Add(new Vector3(x + 1, y + 1, z + 1)); //6
                listVerticesCeiling.Add(new Vector3(x + 1, y + 1, z + 0)); //7

                listVerticesCeiling.Add(new Vector3(x + 1, y + 0, z + 1)); //8
                listVerticesCeiling.Add(new Vector3(x + 0, y + 0, z + 1)); //9
                listVerticesCeiling.Add(new Vector3(x + 0, y + 1, z + 1)); //10
                listVerticesCeiling.Add(new Vector3(x + 1, y + 1, z + 1)); //11

                listVerticesCeiling.Add(new Vector3(x + 0, y + 0, z + 1)); //12
                listVerticesCeiling.Add(new Vector3(x + 0, y + 0, z + 0)); //13
                listVerticesCeiling.Add(new Vector3(x + 0, y + 1, z + 0)); //14
                listVerticesCeiling.Add(new Vector3(x + 0, y + 1, z + 1)); //15

                listVerticesCeiling.Add(new Vector3(x + 0, y + 1, z + 0)); //16
                listVerticesCeiling.Add(new Vector3(x + 1, y + 1, z + 0)); //17
                listVerticesCeiling.Add(new Vector3(x + 1, y + 1, z + 1)); //18
                listVerticesCeiling.Add(new Vector3(x + 0, y + 1, z + 1)); //19

                listVerticesCeiling.Add(new Vector3(x + 0, y + 0, z + 0)); //20
                listVerticesCeiling.Add(new Vector3(x + 1, y + 0, z + 0)); //21
                listVerticesCeiling.Add(new Vector3(x + 1, y + 0, z + 1)); //22
                listVerticesCeiling.Add(new Vector3(x + 0, y + 0, z + 1)); //23


                listTrianglesCeiling.AddRange(new List<int> { 0 + CUBE_VERTS * i, 2 + CUBE_VERTS * i, 1 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 0 + CUBE_VERTS * i, 3 + CUBE_VERTS * i, 2 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 17 + CUBE_VERTS * i, 16 + CUBE_VERTS * i, 19 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 17 + CUBE_VERTS * i, 19 + CUBE_VERTS * i, 18 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 4 + CUBE_VERTS * i, 7 + CUBE_VERTS * i, 6 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 4 + CUBE_VERTS * i, 6 + CUBE_VERTS * i, 5 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 13 + CUBE_VERTS * i, 12 + CUBE_VERTS * i, 15 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 13 + CUBE_VERTS * i, 15 + CUBE_VERTS * i, 14 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 11 + CUBE_VERTS * i, 10 + CUBE_VERTS * i, 9 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 11 + CUBE_VERTS * i, 9 + CUBE_VERTS * i, 8 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 20 + CUBE_VERTS * i, 22 + CUBE_VERTS * i, 23 + CUBE_VERTS * i });
                listTrianglesCeiling.AddRange(new List<int> { 20 + CUBE_VERTS * i, 21 + CUBE_VERTS * i, 22 + CUBE_VERTS * i });


                listUVsCeiling.Add(new Vector2(0, 0));
                listUVsCeiling.Add(new Vector2(1, 0));
                listUVsCeiling.Add(new Vector2(1, 1));
                listUVsCeiling.Add(new Vector2(0, 1));
                listUVsCeiling.Add(new Vector2(0, 0));
                listUVsCeiling.Add(new Vector2(1, 0));
                listUVsCeiling.Add(new Vector2(1, 1));
                listUVsCeiling.Add(new Vector2(0, 1));
                listUVsCeiling.Add(new Vector2(0, 0));
                listUVsCeiling.Add(new Vector2(1, 0));
                listUVsCeiling.Add(new Vector2(1, 1));
                listUVsCeiling.Add(new Vector2(0, 1));
                listUVsCeiling.Add(new Vector2(0, 0));
                listUVsCeiling.Add(new Vector2(1, 0));
                listUVsCeiling.Add(new Vector2(1, 1));
                listUVsCeiling.Add(new Vector2(0, 1));
                listUVsCeiling.Add(new Vector2(0, 0));
                listUVsCeiling.Add(new Vector2(1, 0));
                listUVsCeiling.Add(new Vector2(1, 1));
                listUVsCeiling.Add(new Vector2(0, 1));
                listUVsCeiling.Add(new Vector2(0, 0));
                listUVsCeiling.Add(new Vector2(1, 0));
                listUVsCeiling.Add(new Vector2(1, 1));
                listUVsCeiling.Add(new Vector2(0, 1));
                i++;
            }
        }
    }
    private void UpdateMeshes()
    {
/*        meshGround.Clear();
        meshGround.vertices = listVerticesGround.ToArray();
        meshGround.triangles = listTrianglesGround.ToArray();
        meshGround.uv = listUVsGround.ToArray();
        meshGround.RecalculateNormals();
        meshGround.Optimize();
        meshGround.RecalculateBounds();
        MeshCollider groundMeshCollider = groundObject.GetComponent<MeshCollider>();
        groundMeshCollider.sharedMesh = meshGround;*/

        meshWalls.Clear();
        meshWalls.vertices = listVerticesWalls.ToArray();
        meshWalls.triangles = listTrianglesWalls.ToArray();
        meshWalls.uv = listUVsWalls.ToArray();
        meshWalls.RecalculateNormals();
        meshWalls.Optimize();
        meshWalls.RecalculateBounds();
        MeshCollider wallsMeshCollider = wallsObject.GetComponent<MeshCollider>();
        wallsMeshCollider.sharedMesh = meshWalls;

        meshCeiling.Clear();
        meshCeiling.vertices = listVerticesCeiling.ToArray();
        meshCeiling.triangles = listTrianglesCeiling.ToArray();
        meshCeiling.uv = listUVsCeiling.ToArray();
        meshCeiling.RecalculateNormals();
        meshCeiling.Optimize();
        meshCeiling.RecalculateBounds();
        MeshCollider ceilingMeshCollider = ceilingObject.GetComponent<MeshCollider>();
        ceilingMeshCollider.sharedMesh = meshCeiling;

        meshCorners.Clear();
        meshCorners.vertices = listVerticesCorners.ToArray();
        meshCorners.triangles = listTrianglesCorners.ToArray();
        meshCorners.uv = listUVsCorners.ToArray();
        meshCorners.RecalculateNormals();
        meshCorners.Optimize();
        meshCorners.RecalculateBounds();
        MeshCollider cornersMeshCollider = cornersObject.GetComponent<MeshCollider>();
        cornersMeshCollider.sharedMesh = meshCorners;
    }
    private void CreateMaze()
    {
        bool addCube = false;
        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {

                if ((x < 8 && z < 8) || (x > size - 8 && z > size - 8))
                {
                    addCube = false;
                }
                else if (x == 0 || x == size - 1 || z == 0 || z == size - 1)
                {
                    addCube = true;
                }
                else if (Mathf.PerlinNoise((randomX + x) * noiseScale, (randomZ + z) * noiseScale) >= perlin)
                {
                    addCube = true;
                }
                else
                {
                    addCube = false;
                }

                if (addCube)
                {
                    mapMinimap[x, z] = 1;
                }
                else
                {
                    mapMinimap[x, z] = 0;

                }
            }

        }

        DevideRegions();
    }

    struct Coord
    {
        public int x;
        public int y;

        public Coord(int _x, int _y)
        {
            x = _x;
            y = _y;
        }

        public Vector2 GetV2()
        {
            return new Vector2(x, y);
        }
        public Vector3 GetV3()
        {
            return new Vector3(x, 10, y);
        }
    }

    private void DevideRegions()
    {
        int[,] mapCopy = new int[mapMinimap.GetLength(0), mapMinimap.GetLength(1)];
        List<Room> finalRooms = new List<Room>();

        //finding the size of all segments in minimap
        for (int i = 1; i < mapMinimap.GetLength(0) - 1; i++)
        {
            for (int j = 1; j < mapMinimap.GetLength(1) - 1; j++)
            {
                if (mapMinimap[i, j] == 0 && mapCopy[i, j] == 0)
                {
                    List<Coord> tiles = new List<Coord>();
                    List<Coord> edgeTiles = new List<Coord>();
                    Queue<Coord> queueCoords = new Queue<Coord>();
                    queueCoords.Enqueue(new Coord(i, j));
                    mapCopy[i, j] = 1;
                    while (queueCoords.Count > 0)
                    {
                        bool isCorner = false;
                        Coord tile = queueCoords.Dequeue();
                        tiles.Add(tile);
                        if (mapMinimap[tile.x + 1, tile.y] == 0)
                        {
                            if (tile.x + 1 < mapMinimap.GetLength(0) - 1 && mapCopy[tile.x + 1, tile.y] == 0)
                            {
                                queueCoords.Enqueue(new Coord(tile.x + 1, tile.y));
                                mapCopy[tile.x + 1, tile.y] = 1;
                            }
                        }
                        else
                        {
                            isCorner = true;
                        }
                        if (mapMinimap[tile.x - 1, tile.y] == 0)
                        {
                            if (tile.x - 1 > 0 && mapCopy[tile.x - 1, tile.y] == 0)
                            {
                                queueCoords.Enqueue(new Coord(tile.x - 1, tile.y));
                                mapCopy[tile.x - 1, tile.y] = 1;
                            }
                        }
                        else
                        {
                            isCorner = true;
                        }
                        if (mapMinimap[tile.x, tile.y + 1] == 0)
                        {
                            if (tile.y + 1 < mapMinimap.GetLength(1) - 1 && mapCopy[tile.x, tile.y + 1] == 0)
                            {
                                queueCoords.Enqueue(new Coord(tile.x, tile.y + 1));
                                mapCopy[tile.x, tile.y + 1] = 1;
                            }
                        }
                        else
                        {
                            isCorner = true;
                        }
                        if (mapMinimap[tile.x, tile.y - 1] == 0)
                        {
                            if (tile.y - 1 > 0 && mapCopy[tile.x, tile.y - 1] == 0)
                            {
                                queueCoords.Enqueue(new Coord(tile.x, tile.y - 1));
                                mapCopy[tile.x, tile.y - 1] = 1;
                            }
                        }
                        else
                        {
                            isCorner = true;
                        }

                        if (isCorner)
                        {
                            edgeTiles.Add(tile);
                        }

                    }


                    // Delete small rooms
                    if (tiles.Count < 20)
                    {
                        foreach (Coord tile in tiles)
                        {
                            mapMinimap[tile.x, tile.y] = 1;
                        }
                    }
                    else
                    {
                        finalRooms.Add(new Room(tiles, edgeTiles));
                    }

                }

            }
        }

        finalRooms[0].isMainRoom = true;
        finalRooms[0].isAccessibleFromMainRoom = true;
        ConnectClosestRooms(finalRooms);

    }

    private void ConnectClosestRooms(List<Room> finalRooms, bool forceAccessibilityFromMainRoom = false)
    {
        List<Room> roomListA = new List<Room>();
        List<Room> roomListB = new List<Room>();

        if (forceAccessibilityFromMainRoom)
        {
            foreach (Room room in finalRooms)
            {
                if (room.isAccessibleFromMainRoom)
                {
                    roomListB.Add(room);
                }
                else
                {
                    roomListA.Add(room);
                }
            }
        }
        else
        {
            roomListA = finalRooms;
            roomListB = finalRooms;

        }

        int bestDistance = int.MaxValue;
        Coord bestTileA = new Coord();
        Coord bestTileB = new Coord();
        Room bestRoomA = new Room();
        Room bestRoomB = new Room();
        bool possibleConnectionFound = false;

        foreach (Room roomA in roomListA)
        {
            if (!forceAccessibilityFromMainRoom)
            {
                possibleConnectionFound = false;
                if (roomA.connectedRooms.Count > 0)
                {
                    continue;
                }
            }
            foreach (Room roomB in roomListB)
            {
                if (roomB == roomA || roomA.IsConnected(roomB))
                {
                    continue;
                }

                for (int i = 0; i < roomA.edgeTiles.Count; i++)
                {
                    for (int j = 0; j < roomB.edgeTiles.Count; j++)
                    {
                        Coord tileA = roomA.edgeTiles[i];
                        Coord tileB = roomB.edgeTiles[j];

                        int distaneAB = (int)(Mathf.Pow(tileA.x - tileB.x, 2) + Mathf.Pow(tileA.y - tileB.y, 2));
                        if (distaneAB < bestDistance || !possibleConnectionFound)
                        {
                            bestDistance = distaneAB;
                            possibleConnectionFound = true;
                            bestRoomA = roomA;
                            bestRoomB = roomB;
                            bestTileA = tileA;
                            bestTileB = tileB;
                        }
                    }
                }
            }
            if (possibleConnectionFound && !forceAccessibilityFromMainRoom)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }
        }
        if (possibleConnectionFound && forceAccessibilityFromMainRoom)
        {
            CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            ConnectClosestRooms(finalRooms, true);
        }

        if (!forceAccessibilityFromMainRoom)
        {
            ConnectClosestRooms(finalRooms, true);
        }
    }

    private void CreatePassage(Room roomA, Room roomB, Coord tileA, Coord tileB)
    {
        Room.ConnectRooms(roomA, roomB);
        List<Coord> line = GetLine(tileA, tileB);
        foreach (Coord c in line)
        {
            DrawCircle(c, sizeOfPassage);
        }
    }
    void DrawCircle(Coord c, int r)
    {
        for (int x = -r; x <= r; x++)
        {
            for (int y = -r; y <= r; y++)
            {
                if (x * x + y * y <= r * r)
                {
                    int drawX = c.x + x;
                    int drawY = c.y + y;
                    if (IsInMapRange(drawX, drawY))
                    {
                        mapMinimap[drawX, drawY] = 0;
                    }
                }
            }
        }
    }

    List<Coord> GetLine(Coord from, Coord to)
    {
        List<Coord> line = new List<Coord>();

        int x = from.x;
        int y = from.y;

        int dx = to.x - from.x;
        int dy = to.y - from.y;

        bool inverted = false;
        int step = (int)Mathf.Sign(dx);
        int gradientStep = (int)Mathf.Sign(dy);

        int longest = Mathf.Abs(dx);
        int shortest = Mathf.Abs(dy);

        if (longest < shortest)
        {
            inverted = true;
            longest = Mathf.Abs(dy);
            shortest = Mathf.Abs(dx);

            step = (int)Mathf.Sign(dy);
            gradientStep = (int)Mathf.Sign(dx);
        }

        int gradientAccumulation = longest / 2;
        for (int i = 0; i < longest; i++)
        {
            line.Add(new Coord(x, y));

            if (inverted)
            {
                y += step;
            }
            else
            {
                x += step;
            }

            gradientAccumulation += shortest;
            if (gradientAccumulation >= longest)
            {
                if (inverted)
                {
                    x += gradientStep;
                }
                else
                {
                    y += gradientStep;
                }
                gradientAccumulation -= longest;
            }
        }

        return line;
    }
    bool IsInMapRange(int x, int y)
    {
        return x >= 1 && x < size-1 && y >= 1 && y < size-1;
    }
    private class Room
    {
        public List<Coord> tiles;
        public List<Coord> edgeTiles;
        public List<Room> connectedRooms;
        public int roomSize;
        public bool isAccessibleFromMainRoom;
        public bool isMainRoom;

        public Room()
        {

        }
        public Room(List<Coord> _tiles, List<Coord> _edgeTiles)
        {
            tiles = _tiles;
            edgeTiles = _edgeTiles;
            roomSize = _tiles.Count;
            connectedRooms = new List<Room>();
        }

        public void SetAccessibleFromMainRoom()
        {
            if (!isAccessibleFromMainRoom)
            {
                isAccessibleFromMainRoom = true;
                foreach (Room room in connectedRooms)
                {
                    room.SetAccessibleFromMainRoom();
                }
            }
        }
        public bool IsConnected(Room otherRoom)
        {
            return connectedRooms.Contains(otherRoom);
        }
        public static void ConnectRooms(Room roomA, Room roomB)
        {
            if (roomA.isAccessibleFromMainRoom)
            {
                roomB.SetAccessibleFromMainRoom();
            }
            else if (roomB.isAccessibleFromMainRoom)
            {
                roomA.SetAccessibleFromMainRoom();
            }
            roomA.connectedRooms.Add(roomB);
            roomB.connectedRooms.Add(roomA);
        }
    }

    /*void CreateShape()
    {
        int CUBE_VERTS = 24;
        int i = 0;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < height; y++)
            {
                for (int z = 0; z < size; z++)
                {

                    bool addCube = false;
                    if ((x < 3 && z < 3)||( x > size - 4 && z > size -4))
                    {
                        addCube = false;
                    }
                    else if (y == 0 || y == height - 1 || x == 0 || x == size - 1 || z == 0 || z == size - 1)
                    {
                        addCube = true;
                    }
                    else if (Mathf.PerlinNoise((randomX + x) * noiseScale, (randomZ + z) * noiseScale) >= perlin)
                    {
                        addCube = true;
                    }

                    if(addCube)
                    {

                        listVertices.Add(new Vector3(x + 0, y + 0, z + 0)); //0
                        listVertices.Add(new Vector3(x + 1, y + 0, z + 0)); //1
                        listVertices.Add(new Vector3(x + 1, y + 1, z + 0)); //2
                        listVertices.Add(new Vector3(x + 0, y + 1, z + 0)); //3

                        listVertices.Add(new Vector3(x + 1, y + 0, z + 0)); //4
                        listVertices.Add(new Vector3(x + 1, y + 0, z + 1)); //5
                        listVertices.Add(new Vector3(x + 1, y + 1, z + 1)); //6
                        listVertices.Add(new Vector3(x + 1, y + 1, z + 0)); //7

                        listVertices.Add(new Vector3(x + 1, y + 0, z + 1)); //8
                        listVertices.Add(new Vector3(x + 0, y + 0, z + 1)); //9
                        listVertices.Add(new Vector3(x + 0, y + 1, z + 1)); //10
                        listVertices.Add(new Vector3(x + 1, y + 1, z + 1)); //11

                        listVertices.Add(new Vector3(x + 0, y + 0, z + 1)); //12
                        listVertices.Add(new Vector3(x + 0, y + 0, z + 0)); //13
                        listVertices.Add(new Vector3(x + 0, y + 1, z + 0)); //14
                        listVertices.Add(new Vector3(x + 0, y + 1, z + 1)); //15

                        listVertices.Add(new Vector3(x + 0, y + 1, z + 0)); //16
                        listVertices.Add(new Vector3(x + 1, y + 1, z + 0)); //17
                        listVertices.Add(new Vector3(x + 1, y + 1, z + 1)); //18
                        listVertices.Add(new Vector3(x + 0, y + 1, z + 1)); //19

                        listVertices.Add(new Vector3(x + 0, y + 0, z + 0)); //20
                        listVertices.Add(new Vector3(x + 1, y + 0, z + 0)); //21
                        listVertices.Add(new Vector3(x + 1, y + 0, z + 1)); //22
                        listVertices.Add(new Vector3(x + 0, y + 0, z + 1)); //23


                        listTriangles.AddRange(new List<int> { 0 + CUBE_VERTS * i, 2 + CUBE_VERTS * i, 1 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 0 + CUBE_VERTS * i, 3 + CUBE_VERTS * i, 2 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 17 + CUBE_VERTS * i, 16 + CUBE_VERTS * i, 19 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 17 + CUBE_VERTS * i, 19 + CUBE_VERTS * i, 18 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 4 + CUBE_VERTS * i, 7 + CUBE_VERTS * i, 6 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 4 + CUBE_VERTS * i, 6 + CUBE_VERTS * i, 5 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 13 + CUBE_VERTS * i, 12 + CUBE_VERTS * i, 15 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 13 + CUBE_VERTS * i, 15 + CUBE_VERTS * i, 14 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 11 + CUBE_VERTS * i, 10 + CUBE_VERTS * i, 9 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 11 + CUBE_VERTS * i, 9 + CUBE_VERTS * i, 8 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 20 + CUBE_VERTS * i, 22 + CUBE_VERTS * i, 23 + CUBE_VERTS * i });
                        listTriangles.AddRange(new List<int> { 20 + CUBE_VERTS * i, 21 + CUBE_VERTS * i, 22 + CUBE_VERTS * i });


                        listUVs.Add(new Vector2(0, 0));
                        listUVs.Add(new Vector2(1, 0));
                        listUVs.Add(new Vector2(1, 1));
                        listUVs.Add(new Vector2(0, 1));
                        listUVs.Add(new Vector2(0, 0));
                        listUVs.Add(new Vector2(1, 0));
                        listUVs.Add(new Vector2(1, 1));
                        listUVs.Add(new Vector2(0, 1));
                        listUVs.Add(new Vector2(0, 0));
                        listUVs.Add(new Vector2(1, 0));
                        listUVs.Add(new Vector2(1, 1));
                        listUVs.Add(new Vector2(0, 1));
                        listUVs.Add(new Vector2(0, 0));
                        listUVs.Add(new Vector2(1, 0));
                        listUVs.Add(new Vector2(1, 1));
                        listUVs.Add(new Vector2(0, 1));
                        listUVs.Add(new Vector2(0, 0));
                        listUVs.Add(new Vector2(1, 0));
                        listUVs.Add(new Vector2(1, 1));
                        listUVs.Add(new Vector2(0, 1));
                        listUVs.Add(new Vector2(0, 0));
                        listUVs.Add(new Vector2(1, 0));
                        listUVs.Add(new Vector2(1, 1));
                        listUVs.Add(new Vector2(0, 1));
                        i++;
                        if(y == 1)
                        {
                            mapMinimap[x, z] = 1;
                        }
                    }
                    else
                    {
                        mapMinimap[x, z] = 0;
                    }
                }
            }
        }

        for(int m = 0; m < i; m++)
        {

        }

    }

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = listVertices.ToArray();
        mesh.triangles = listTriangles.ToArray();
        mesh.uv = listUVs.ToArray();

        mesh.RecalculateNormals();
        mesh.Optimize();
        mesh.RecalculateBounds();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }*/

    public float Perlin3D(float x, float y, float z)
    {
        float ab = Mathf.PerlinNoise(x, y);
        float bc = Mathf.PerlinNoise(y, z);
        float ac = Mathf.PerlinNoise(x, z);

        float ba = Mathf.PerlinNoise(y, x);
        float cb = Mathf.PerlinNoise(z, y);
        float ca = Mathf.PerlinNoise(z, x);

        float abc = ab + bc + ac + ba + cb + ca;
        return abc / 6f;


    }
    private void SmoothCornersMesh()
    {
        int i = 0;
        int cornerVerts = 4;
        for (int x = 1; x < size - 1; x++)
        {
            for (int z = 1; z < size - 1; z++)
            {
                for (int y = 1; y < height; y++)
                {
                    if (mapMinimap[x, z] == 0 && mapMinimap[x, z + 1] == 1 && mapMinimap[x - 1, z] == 1)
                    {
                        listVerticesCorners.Add(new Vector3(x, y, z + (1 - cornersSize)));
                        listVerticesCorners.Add(new Vector3(x + cornersSize, y, z + 1));
                        listVerticesCorners.Add(new Vector3(x + cornersSize, y + 1, z + 1));
                        listVerticesCorners.Add(new Vector3(x, y + 1, z + (1 - cornersSize)));
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 2 + cornerVerts * i, 1 + cornerVerts * i });
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 3 + cornerVerts * i, 2 + cornerVerts * i });
                        listUVsCorners.Add(new Vector2(0, 0));
                        listUVsCorners.Add(new Vector2(1, 0));
                        listUVsCorners.Add(new Vector2(1, 1));
                        listUVsCorners.Add(new Vector2(0, 1));
                        i++;
                    }
                    if (mapMinimap[x, z] == 0 && mapMinimap[x, z - 1] == 1 && mapMinimap[x - 1, z] == 1)
                    {
                        listVerticesCorners.Add(new Vector3(x + cornersSize, y, z + 0));
                        listVerticesCorners.Add(new Vector3(x, y, z + cornersSize));
                        listVerticesCorners.Add(new Vector3(x, y + 1, z + cornersSize));
                        listVerticesCorners.Add(new Vector3(x + cornersSize, y + 1, z + 0));
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 2 + cornerVerts * i, 1 + cornerVerts * i });
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 3 + cornerVerts * i, 2 + cornerVerts * i });
                        listUVsCorners.Add(new Vector2(0, 0));
                        listUVsCorners.Add(new Vector2(1, 0));
                        listUVsCorners.Add(new Vector2(1, 1));
                        listUVsCorners.Add(new Vector2(0, 1));
                        i++;
                    }
                    if (mapMinimap[x, z] == 0 && mapMinimap[x, z + 1] == 1 && mapMinimap[x + 1, z] == 1)
                    {
                        listVerticesCorners.Add(new Vector3(x + (1 - cornersSize), y, z + 1));
                        listVerticesCorners.Add(new Vector3(x + 1, y, z + (1 - cornersSize)));
                        listVerticesCorners.Add(new Vector3(x + 1, y + 1, z + (1 - cornersSize)));
                        listVerticesCorners.Add(new Vector3(x + (1 - cornersSize), y + 1, z + 1));
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 2 + cornerVerts * i, 1 + cornerVerts * i });
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 3 + cornerVerts * i, 2 + cornerVerts * i });
                        listUVsCorners.Add(new Vector2(0, 0));
                        listUVsCorners.Add(new Vector2(1, 0));
                        listUVsCorners.Add(new Vector2(1, 1));
                        listUVsCorners.Add(new Vector2(0, 1));
                        i++;
                    }
                    if (mapMinimap[x, z] == 0 && mapMinimap[x, z - 1] == 1 && mapMinimap[x + 1, z] == 1)
                    {
                        listVerticesCorners.Add(new Vector3(x + 1, y, z + cornersSize));
                        listVerticesCorners.Add(new Vector3(x + (1 - cornersSize), y, z));
                        listVerticesCorners.Add(new Vector3(x + (1 - cornersSize), y + 1, z));
                        listVerticesCorners.Add(new Vector3(x + 1, y + 1, z + cornersSize));
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 2 + cornerVerts * i, 1 + cornerVerts * i });
                        listTrianglesCorners.AddRange(new List<int> { cornerVerts * i, 3 + cornerVerts * i, 2 + cornerVerts * i });
                        listUVsCorners.Add(new Vector2(0, 0));
                        listUVsCorners.Add(new Vector2(1, 0));
                        listUVsCorners.Add(new Vector2(1, 1));
                        listUVsCorners.Add(new Vector2(0, 1));
                        i++;
                    }
                }
            }
        }
    }
    private void doLeftSide(int x, int z, int y)
    {
        // Side walls are just flat
        if (x == 0)
        {
            listVerticesWalls.Add(new Vector3(x, y, z + 1));
            listVerticesWalls.Add(new Vector3(x, y + 1, z + 1));
            listVerticesWalls.Add(new Vector3(x, y, z));
            listVerticesWalls.Add(new Vector3(x, y + 1, z));
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 1 + vertices, 2 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 2 + vertices, 1 + vertices, 3 + vertices });
            vertices += 4;
        }
        else
        {
            if(z == size - 1)
            {
                listVerticesWalls.Add(new Vector3(x, y, z + 1));
                listVerticesWalls.Add(new Vector3(x, y + 1, z + 1));

            }
            // Checking for the corner, if corner -> both faces of the corner must have the same coords
            else if (mapMinimap[x, z + 1] == 0 && mapMinimap[x - 1, z + 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y + 1, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));

                // Checking for inner conners -> both face of the inner conner must have the same coords => Not inner conner
            }
            else if (mapMinimap[x - 1, z + 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x, y, z + 1));
                listVerticesWalls.Add(new Vector3(x, y + 1, z + 1));
            }
            // No corner
            else
            {
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, y * noiseWallsScale, (z + 1) * noiseWallsScale)), y, z + 1));
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, (y + 1) * noiseWallsScale, (z + 1) * noiseWallsScale)), y + 1, z + 1));

            }

            if(z == 0)
            {
                listVerticesWalls.Add(new Vector3(x, y, z));
                listVerticesWalls.Add(new Vector3(x, y + 1, z));
            }
            //Other side of face
            else if (mapMinimap[x, z - 1] == 0 && mapMinimap[x - 1, z - 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y, z - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y + 1, z - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x - 1, z - 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x, y, z));
                listVerticesWalls.Add(new Vector3(x, y + 1, z));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, y * noiseWallsScale, z * noiseWallsScale)), y, z));
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, (y + 1) * noiseWallsScale, z * noiseWallsScale)), y + 1, z));
            }

            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 1 + vertices, 2 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 2 + vertices, 1 + vertices, 3 + vertices });
            vertices += 4;
        }
    }
    private void doRightSide(int x, int z, int y)
    {
        if (x == size - 1)
        {
            listVerticesWalls.Add(new Vector3(x + 1, y, z + 1));
            listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + 1));
            listVerticesWalls.Add(new Vector3(x + 1, y, z));
            listVerticesWalls.Add(new Vector3(x + 1, y + 1, z));
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 2 + vertices, 1 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 2 + vertices, 3 + vertices, 1 + vertices });
            vertices += 4;
        }
        else
        {
            if(z == size - 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z + 1));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + 1));
            }
            else if (mapMinimap[x, z + 1] == 0 && mapMinimap[x + 1, z + 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y + 1, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x + 1, z + 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z + 1));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + 1));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x + (deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, y * noiseWallsScale, (z + 1) * noiseWallsScale)), y + 0, z + 1));
                listVerticesWalls.Add(new Vector3(x + (deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, (y + 1) * noiseWallsScale, (z + 1) * noiseWallsScale)), y + 1, z + 1));

            }
            if(z == 0)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z));
            }
            else if (mapMinimap[x, z - 1] == 0 && mapMinimap[x + 1, z - 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y, z - (-deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y + 1, z - (-deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x + 1, z - 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x + (deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, y * noiseWallsScale, z * noiseWallsScale)), y + 0, z + 0));
                listVerticesWalls.Add(new Vector3(x + (deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, (y + 1) * noiseWallsScale, z * noiseWallsScale)), y + 1, z + 0));
            }
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 2 + vertices, 1 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 2 + vertices, 3 + vertices, 1 + vertices });
            vertices += 4;
        }
    }
    private void doFrontSide(int x, int z, int y)
    {
        if (z == 0)
        {
            listVerticesWalls.Add(new Vector3(x + 1, y, z));
            listVerticesWalls.Add(new Vector3(x + 1, y + 1, z));
            listVerticesWalls.Add(new Vector3(x, y, z));
            listVerticesWalls.Add(new Vector3(x, y + 1, z));
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 2 + vertices, 1 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 2 + vertices, 3 + vertices, 1 + vertices });
            vertices += 4;
        }
        else
        {
            if (x == size - 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z));
            }
            else if (mapMinimap[x + 1, z] == 0 && mapMinimap[x + 1, z - 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y, z - (-deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y + 1, z - (-deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x + 1, z - 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x + 1, y + 0, z - (-deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, y * noiseWallsScale, z * noiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z - (-deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, (y + 1) * noiseWallsScale, z * noiseWallsScale))));

            }


            if (x == 0)
            {
                listVerticesWalls.Add(new Vector3(x, y, z));
                listVerticesWalls.Add(new Vector3(x, y + 1, z));
            }
            else if (mapMinimap[x - 1, z] == 0 && mapMinimap[x - 1, z - 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y, z - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale)), y + 1, z - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, z * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x - 1, z - 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x, y, z));
                listVerticesWalls.Add(new Vector3(x, y + 1, z));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x + 0, y + 0, z - (-deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, y * noiseWallsScale, z * noiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 0, y + 1, z - (-deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, (y + 1) * noiseWallsScale, z * noiseWallsScale))));
            }
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 2 + vertices, 1 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 2 + vertices, 3 + vertices, 1 + vertices });
            vertices += 4;
        }
    }
    private void doBackSide(int x, int z, int y)
    {
        if (z == size - 1)
        {
            listVerticesWalls.Add(new Vector3(x + 1, y, z + 1));
            listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + 1));
            listVerticesWalls.Add(new Vector3(x, y, z + 1));
            listVerticesWalls.Add(new Vector3(x, y + 1, z + 1));
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 1 + vertices, 2 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 1 + vertices, 3 + vertices, 2 + vertices });
            vertices += 4;
        }
        else
        {
            if(x == size - 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z + 1));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + 1));
            }
            else if (mapMinimap[x + 1, z] == 0 && mapMinimap[x + 1, z + 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y + 1, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D((x + 1) * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x + 1, z + 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x + 1, y, z + 1));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + 1));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x + 1, y + 0, z + (deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, y * noiseWallsScale, (z + 1) * noiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 1, y + 1, z + (deficit + perlinMultiplier * Perlin3D((x + 1) * noiseWallsScale, (y + 1) * noiseWallsScale, (z + 1) * noiseWallsScale))));

            }

            if(x == 0)
            {
                listVerticesWalls.Add(new Vector3(x, y, z + 1));
                listVerticesWalls.Add(new Vector3(x, y + 1, z + 1));
            }
            else if (mapMinimap[x - 1, z] == 0 && mapMinimap[x - 1, z + 1] == 0)
            {
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, y * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x - (-deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale)), y + 1, z + 1 + (deficit + perlinCornerMultiplier * Perlin3D(x * cornersNoiseWallsScale, (y + 1) * cornersNoiseWallsScale, (z + 1) * cornersNoiseWallsScale))));
            }
            else if (mapMinimap[x - 1, z + 1] == 1)
            {
                listVerticesWalls.Add(new Vector3(x, y, z + 1));
                listVerticesWalls.Add(new Vector3(x, y + 1, z + 1));
            }
            else
            {
                listVerticesWalls.Add(new Vector3(x + 0, y + 0, z + (deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, y * noiseWallsScale, (z + 1) * noiseWallsScale))));
                listVerticesWalls.Add(new Vector3(x + 0, y + 1, z + (deficit + perlinMultiplier * Perlin3D(x * noiseWallsScale, (y + 1) * noiseWallsScale, (z + 1) * noiseWallsScale))));
            }
            listTrianglesWalls.AddRange(new List<int> { 0 + vertices, 1 + vertices, 2 + vertices });
            listTrianglesWalls.AddRange(new List<int> { 1 + vertices, 3 + vertices, 2 + vertices });
            vertices += 4;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinding : MonoBehaviour
{
    public SpawnGround spawnGround;
    public int[,] mapMinimap;
    public Node[,] nodeMap;
    public List<Node> pathFinal;

    void Awake()
    {
        mapMinimap = spawnGround.mapMinimap;
        nodeMap = new Node[mapMinimap.GetLength(0), mapMinimap.GetLength(1)];
        CreateNodes();
        FindPath();

        //setting final path  in Minimap Object
        Minimap.Instance.FinalPath = pathFinal;
    }

    private void FindPath()
    {
        Node startNode = nodeMap[0, 0];
        Node targetNode = nodeMap[nodeMap.GetLength(0) - 1, nodeMap.GetLength(1) - 1];

        List<Node> openNodes = new List<Node>();
        HashSet<Node> closedNodes = new HashSet<Node>();
        openNodes.Add(startNode);

        while (openNodes.Count > 0)
        {
            Node currentNode = openNodes[0];
            for (int i = 1; i < openNodes.Count; i++)
            {
                if (openNodes[i].fCost <= currentNode.fCost && openNodes[i].hCost < currentNode.hCost)
                {
                    currentNode = openNodes[i];
                }
            }
            openNodes.Remove(currentNode);
            closedNodes.Add(currentNode);

            if (currentNode == targetNode)
            {
                RetracePath(startNode, targetNode);
                return;
            }

            foreach (Node neighbour in FindNeighbours(currentNode))
            {
                if (!neighbour.walkable || closedNodes.Contains(neighbour))
                {
                    continue;
                }

                int newMovementCostToNeighbour = currentNode.gCost + GetDistance(currentNode, neighbour);
                if (newMovementCostToNeighbour < neighbour.gCost || !openNodes.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if (!openNodes.Contains(neighbour))
                    {
                        openNodes.Add(neighbour);
                    }
                }
            }
        }
    }

    void RetracePath(Node startNode, Node endNode)
    {
        List<Node> path = new List<Node>();
        Node currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        pathFinal = path;   
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int dstY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (dstX > dstY)
        {
            return 14 * dstY + 10 * (dstX - dstY);
        }
        else
        {
            return 14 * dstX + 10 * (dstY - dstX);
        }

    }

    private void CreateNodes()
    {
        for (int i = 0; i < mapMinimap.GetLength(0); i++)
        {
            for (int j = 0; j < mapMinimap.GetLength(1); j++)
            {
                if (mapMinimap[i, j] == 0)
                {
                    nodeMap[i, j] = new Node(true, new Vector3(i, 0, j), i, j);
                }
                else
                {
                    nodeMap[i, j] = new Node(false, new Vector3(i, 0, j), i, j);
                }
            }
        }
    }

    private List<Node> FindNeighbours(Node node)
    {
        List<Node> neighbours = new List<Node>();

        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }

                if (node.gridX + i >= 0 && node.gridX + i < nodeMap.GetLength(0) && node.gridY + j >= 0 && node.gridY + j < nodeMap.GetLength(1))
                {
                    neighbours.Add(nodeMap[node.gridX + i, node.gridY + j]);
                }

            }
        }
        return neighbours;
    }
}

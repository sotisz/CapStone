using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : MonoBehaviour
{
    public List<WaypointNode> FindPath(WaypointNode startNode, WaypointNode targetNode)
    {
        List<WaypointNode> openList = new List<WaypointNode>();
        HashSet<WaypointNode> closedList = new HashSet<WaypointNode>();
        openList.Add(startNode);

        // A* 계산을 시작하기 전 모든 노드의 비용 초기화
        WaypointNode[] allNodes = FindObjectsOfType<WaypointNode>();
        foreach (WaypointNode node in allNodes)
        {
            node.gCost = float.MaxValue;
            node.parent = null;
        }
        startNode.gCost = 0;
        startNode.hCost = GetDistance(startNode, targetNode);


        while (openList.Count > 0)
        {
            WaypointNode currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost || (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                return RetracePath(startNode, targetNode);
            }

            foreach (var connection in currentNode.neighbors)
            {
                WaypointNode neighborNode = connection.neighbor;
                if (neighborNode == null || closedList.Contains(neighborNode))
                {
                    continue;
                }

                float newMovementCostToNeighbor = currentNode.gCost + GetDistance(currentNode, neighborNode, connection.connectionType);
                if (newMovementCostToNeighbor < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = newMovementCostToNeighbor;
                    neighborNode.hCost = GetDistance(neighborNode, targetNode);
                    neighborNode.parent = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }
        return null; // 경로 없음
    }

    private List<WaypointNode> RetracePath(WaypointNode startNode, WaypointNode endNode)
    {
        List<WaypointNode> path = new List<WaypointNode>();
        WaypointNode currentNode = endNode;
        while (currentNode != startNode)
        {
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Add(startNode);
        path.Reverse();
        return path;
    }

    private float GetDistance(WaypointNode nodeA, WaypointNode nodeB, WaypointNode.EConnectionType type = WaypointNode.EConnectionType.Walk)
    {
        float distance = Vector2.Distance(nodeA.transform.position, nodeB.transform.position);
        // (필요시 타입별 가중치 추가)
        return distance;
    }

    public WaypointNode FindClosestWaypoint(Vector3 position)
    {
        WaypointNode[] allWaypoints = FindObjectsOfType<WaypointNode>();
        WaypointNode closest = null;
        float minDistance = float.MaxValue;
        foreach (var waypoint in allWaypoints)
        {
            float distance = Vector3.Distance(position, waypoint.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = waypoint;
            }
        }
        return closest;
    }
}
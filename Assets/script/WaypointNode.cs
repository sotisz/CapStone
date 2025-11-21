using UnityEngine;
using System.Collections.Generic;

public class WaypointNode : MonoBehaviour
{
    [System.Serializable]
    public struct NeighborConnection
    {
        public WaypointNode neighbor;
        public EConnectionType connectionType;
    }

    public enum EConnectionType { Walk, Jump, Fall }
    public List<NeighborConnection> neighbors = new List<NeighborConnection>();

    [HideInInspector] public float gCost;
    [HideInInspector] public float hCost;
    public float fCost => gCost + hCost;
    [HideInInspector] public WaypointNode parent;

    [Header("In-Game Visuals")]
    [Tooltip("노드 '자신'에 표시할 이펙트 프리팹")]
    public GameObject nodeEffectPrefab;
    [Tooltip("이웃으로 가는 '경로'에 표시할 이펙트 프리팹")]
    public GameObject pathEffectPrefab;

    private GameObject currentNodeEffectInstance;
    private List<GameObject> currentPathEffectInstances = new List<GameObject>();


    public void ShowEffect(WaypointNode nextNodeInPath)
    {
        if (nodeEffectPrefab != null && currentNodeEffectInstance == null)
        {
            currentNodeEffectInstance = Instantiate(nodeEffectPrefab, transform.position, Quaternion.identity, transform);
        }

        if (pathEffectPrefab == null || nextNodeInPath == null) return;
        
        bool isNeighbor = false;
        foreach (var conn in neighbors) {
            if (conn.neighbor == nextNodeInPath) {
                isNeighbor = true;
                break;
            }
        }
        if (!isNeighbor) return;

        float distanceInterval = 1.0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = nextNodeInPath.transform.position;

        float totalDistance = Vector3.Distance(startPos, endPos);
        var direction = (endPos - startPos).normalized;
        var currentDistance = distanceInterval;

        while (currentDistance < totalDistance)
        {
            Vector3 point = startPos + direction * currentDistance;
            GameObject pathEffect = Instantiate(pathEffectPrefab, point, Quaternion.identity);
            currentPathEffectInstances.Add(pathEffect);
            currentDistance += distanceInterval;
        }
    }

    public void HideEffect()
    {
        if (currentNodeEffectInstance != null)
        {
            Destroy(currentNodeEffectInstance);
            currentNodeEffectInstance = null;
        }

        foreach (GameObject pathEffect in currentPathEffectInstances)
        {
            Destroy(pathEffect);
        }

        currentPathEffectInstances.Clear();
    }
    
    private float nodeRadius = 0.3f;
    private Color nodeColor = Color.cyan;

    private void OnDrawGizmos()
    {
        Gizmos.color = nodeColor;
        Gizmos.DrawSphere(transform.position, nodeRadius);

        foreach (var connection in neighbors)
        {
            if (connection.neighbor == null) continue;
            switch (connection.connectionType)
            {
                case EConnectionType.Walk: Gizmos.color = Color.green; break;
                case EConnectionType.Jump: Gizmos.color = Color.yellow; break;
                case EConnectionType.Fall: Gizmos.color = Color.blue; break;
            }
            Gizmos.DrawLine(transform.position, connection.neighbor.transform.position);
        }
    }
}
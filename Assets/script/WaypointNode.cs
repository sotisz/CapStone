using UnityEngine;
using System.Collections.Generic;

// (파일 상단의 기존 코드는 동일)
// public class WaypointNode : MonoBehaviour { ... }
// (NeighborConnection, EConnectionType, A* 변수 등은 동일)

public class WaypointNode : MonoBehaviour
{
    // --- 기존 Waypoint 설정 ---
    [System.Serializable]
    public struct NeighborConnection
    {
        public WaypointNode neighbor;
        public EConnectionType connectionType;
    }

    public enum EConnectionType { Walk, Jump, Fall }
    public List<NeighborConnection> neighbors = new List<NeighborConnection>();

    // A* 알고리즘 변수
    [HideInInspector] public float gCost;
    [HideInInspector] public float hCost;
    public float fCost => gCost + hCost;
    [HideInInspector] public WaypointNode parent;

    // --- 게임 내 시각화 설정 ---
    [Header("In-Game Visuals")]
    [Tooltip("노드 '자신'에 표시할 이펙트 프리팹")]
    public GameObject nodeEffectPrefab;
    [Tooltip("이웃으로 가는 '경로'에 표시할 이펙트 프리팹")]
    public GameObject pathEffectPrefab;

    private GameObject currentNodeEffectInstance;
    private List<GameObject> currentPathEffectInstances = new List<GameObject>();


    // --- [수정된] 이펙트를 켜는 함수 ---
    /// <summary>
    /// 이 노드와 '지정된 다음 노드'로 가는 경로에 이펙트를 활성화합니다.
    /// </summary>
    /// <param name="nextNodeInPath">A* 경로상의 다음 노드. null이면 노드 자체 이펙트만 켭니다.</param>
    public void ShowEffect(WaypointNode nextNodeInPath)
    {
        // 1. '노드' 자체에 이펙트 생성
        if (nodeEffectPrefab != null && currentNodeEffectInstance == null)
        {
            currentNodeEffectInstance = Instantiate(nodeEffectPrefab, transform.position, Quaternion.identity, transform);
        }

        // 2. '경로' 이펙트 생성 (지정된 다음 노드가 있을 때만)
        if (pathEffectPrefab == null || nextNodeInPath == null) return;
        
        // nextNodeInPath가 유효한 이웃인지 확인 (안전장치)
        bool isNeighbor = false;
        foreach (var conn in neighbors) {
            if (conn.neighbor == nextNodeInPath) {
                isNeighbor = true;
                break;
            }
        }
        if (!isNeighbor) return; // 이웃이 아니면 경로를 그리지 않음

        // [핵심] 지정된 'nextNodeInPath'로 가는 경로 이펙트만 생성합니다.
        float distanceInterval = 1.0f; // 이펙트 생성 간격
        Vector3 startPos = transform.position;
        Vector3 endPos = nextNodeInPath.transform.position;

        float totalDistance = Vector3.Distance(startPos, endPos);
        var direction = (endPos - startPos).normalized;
        var currentDistance = distanceInterval;

        while (currentDistance < totalDistance)
        {
            Vector3 point = startPos + direction * currentDistance;
            GameObject pathEffect = Instantiate(pathEffectPrefab, point, Quaternion.identity);
            currentPathEffectInstances.Add(pathEffect); // 나중에 지우기 위해 리스트에 추가
            currentDistance += distanceInterval;
        }
    }

    // --- [수정 안 됨] 이펙트를 끄는 함수 ---
    /// <summary>
    /// 이 노드와 경로의 모든 이펙트를 비활성화합니다.
    /// </summary>
    public void HideEffect()
    {
        // 1. '노드' 이펙트 파괴
        if (currentNodeEffectInstance != null)
        {
            Destroy(currentNodeEffectInstance);
            currentNodeEffectInstance = null;
        }

        // 2. '경로' 이펙트들을 모두 파괴
        foreach (GameObject pathEffect in currentPathEffectInstances)
        {
            Destroy(pathEffect);
        }

        // 3. 리스트를 비웁니다.
        currentPathEffectInstances.Clear();
    }

    // --- 기즈모 ---
    // (OnDrawGizmos 코드는 수정 없이 동일)
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
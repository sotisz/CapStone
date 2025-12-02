using System.Collections;
using UnityEngine;

public class RockFallZone : MonoBehaviour
{[Header("설정")]
    public GameObject debrisPrefab; // 떨어질 돌 프리팹
    public float spawnInterval = 0.5f; // 생성 간격

    private BoxCollider2D areaCollider;

    private void Start()
    {
        areaCollider = GetComponent<BoxCollider2D>();
        
        // 혹시 모르니 트리거로 설정 (물리 충돌 방지)
        if (areaCollider != null) 
            areaCollider.isTrigger = true;

        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            // 콜라이더가 없거나 꺼져있으면 생성 안 함
            if (areaCollider != null && areaCollider.enabled)
            {
                SpawnDebris();
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnDebris()
    {
        // 콜라이더의 영역(Bounds) 가져오기
        Bounds bounds = areaCollider.bounds;

        // 영역 안에서 랜덤한 X, Y 좌표 뽑기
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);

        // 해당 위치에 돌 생성
        Instantiate(debrisPrefab, new Vector2(randomX, randomY), Quaternion.identity);
    }
}
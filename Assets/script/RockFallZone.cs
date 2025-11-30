using System.Collections;
using UnityEngine;

public class RockFallZone : MonoBehaviour
{
    [Header("기본 설정")]
    public GameObject debrisPrefab;   // 떨어질 돌 프리팹
    public Transform bearTransform;   // 곰 (Player_Bear)
    public Transform tigerTransform;  // 호랑이 (Player_Tiger)

    [Header("생성 설정")]
    public float spawnInterval = 0.5f; // 돌 떨어지는 간격
    public float spawnHeight = 10f;    // 플레이어 머리 위 높이
    public float xRange = 5f;          // 랜덤 좌우 범위

    private BoxCollider2D zoneCollider; // 영역을 담당할 콜라이더
    private bool isSpawning = true;

    private void Start()
    {
        // 내 몸에 붙은 콜라이더 가져오기
        zoneCollider = GetComponent<BoxCollider2D>();

        if (zoneCollider == null)
        {
            Debug.LogError("DebrisZone: BoxCollider2D가 없습니다! 추가해주세요.");
            return;
        }

        // 영역은 물리 충돌을 일으키면 안 되므로 트리거로 설정
        zoneCollider.isTrigger = true;

        StartCoroutine(SpawnDebrisRoutine());
    }

    private IEnumerator SpawnDebrisRoutine()
    {
        while (isSpawning)
        {
            // 1. 현재 활성화된(눈에 보이는) 플레이어 찾기
            Transform activePlayer = GetActivePlayer();

            // 2. 플레이어가 있고 && 플레이어가 내 구역(Zone) 안에 있는지 확인
            if (activePlayer != null && IsPlayerInZone(activePlayer.position))
            {
                SpawnDebris(activePlayer);
            }
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 현재 곰과 호랑이 중 누가 켜져있는지 확인
    private Transform GetActivePlayer()
    {
        if (bearTransform != null && bearTransform.gameObject.activeInHierarchy) return bearTransform;
        if (tigerTransform != null && tigerTransform.gameObject.activeInHierarchy) return tigerTransform;
        return null;
    }

    // 플레이어가 이 구역(Collider) 안에 들어왔는지 확인하는 함수
    private bool IsPlayerInZone(Vector3 playerPos)
    {
        // OverlapPoint는 점이 콜라이더 안에 있는지 검사해줌 (아주 정확함!)
        return zoneCollider.OverlapPoint(playerPos);
    }

    private void SpawnDebris(Transform targetPlayer)
    {
        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(targetPlayer.position.x + randomX, targetPlayer.position.y + spawnHeight, 0);

        Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
    }
}
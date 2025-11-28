using System.Collections;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [Header("설정")]
    public GameObject debrisPrefab;   // 떨어질 돌 프리팹
    public Transform playerTransform; // 플레이어의 위치를 알기 위해 필요

    [Header("생성 범위 및 시간")]
    public float spawnInterval = 0.5f; // 돌이 떨어지는 간격 (초)
    public float spawnHeight = 10f;    // 플레이어 머리 위 얼마나 높은 곳에서 떨어질지
    public float xRange = 5f;          // 플레이어 기준 좌우 랜덤 범위

    private bool isSpawning = true;

    private void Start()
    {
        // 자동으로 코루틴 시작
        StartCoroutine(SpawnDebrisRoutine());
    }

    private IEnumerator SpawnDebrisRoutine()
    {
        while (isSpawning)
        {
            // 플레이어가 존재할 때만 생성
            if (playerTransform != null)
            {
                SpawnDebris();
            }

            // 지정된 시간만큼 대기
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnDebris()
    {
        // 1. 플레이어의 X 위치를 기준으로 랜덤한 X 위치 계산
        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(playerTransform.position.x + randomX, playerTransform.position.y + spawnHeight, 0);

        // 2. 돌 생성
        Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
    }

    // 게임 오버시나 컷신 진입 시 생성을 멈추기 위한 함수
    public void StopSpawning()
    {
        isSpawning = false;
    }
}
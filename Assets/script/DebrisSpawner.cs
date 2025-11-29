using System.Collections;
using UnityEngine;

public class DebrisSpawner : MonoBehaviour
{
    [Header("기본 설정")]
    public GameObject debrisPrefab;   // 떨어질 돌 프리팹

    [Header("플레이어 연결 (자식 오브젝트 연결)")]
    public Transform bearTransform;   // 'Player(Bear)'를 여기에 연결
    public Transform tigerTransform;  // 'Player(Tiger)'를 여기에 연결
    
    [Header("생성 범위 및 시간")]
    public float spawnInterval = 0.5f; 
    public float spawnHeight = 10f;    
    public float xRange = 5f;          

    [Header("낙석 중단 설정")]
    public bool useStopPosition = true; 
    public float stopXPosition = 0f;    

    private bool isSpawning = true; 

    private void Start()
    {
        // 안전 장치: 둘 중 하나라도 연결 안 되면 경고
        if (bearTransform == null || tigerTransform == null)
        {
            Debug.LogError("DebrisSpawner: 곰(Bear)과 호랑이(Tiger) Transform을 모두 연결해주세요!");
            return;
        }

        StartCoroutine(SpawnDebrisRoutine());
    }

    private IEnumerator SpawnDebrisRoutine()
    {
        while (isSpawning)
        {
            // 1. 현재 활성화된 플레이어 찾기 (태그 시스템 대응)
            Transform activePlayer = GetActivePlayer();

            // 활성화된 플레이어가 없으면 대기 (둘 다 꺼진 경우 등)
            if (activePlayer == null)
            {
                yield return null;
                continue;
            }

            // 2. 목표 지점 통과 확인 (현재 활성화된 캐릭터의 X 좌표 기준)
            if (useStopPosition && activePlayer.position.x >= stopXPosition)
            {
                Debug.Log($"플레이어({activePlayer.name})가 목표 지점을 통과하여 낙석을 중단합니다.");
                StopSpawning();
                yield break; 
            }

            // 3. 돌 생성 (활성화된 캐릭터 위치 기준)
            SpawnDebris(activePlayer);
            
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    // 현재 곰과 호랑이 중 누가 켜져있는지(Active) 확인해서 리턴하는 함수
    private Transform GetActivePlayer()
    {
        if (bearTransform.gameObject.activeInHierarchy) return bearTransform;
        if (tigerTransform.gameObject.activeInHierarchy) return tigerTransform;
        return null;
    }

    private void SpawnDebris(Transform targetPlayer)
    {
        if (debrisPrefab == null) return;

        // 활성화된 캐릭터의 위치를 기준으로 생성
        float randomX = Random.Range(-xRange, xRange);
        Vector3 spawnPos = new Vector3(targetPlayer.position.x + randomX, targetPlayer.position.y + spawnHeight, 0);

        Instantiate(debrisPrefab, spawnPos, Quaternion.identity);
    }
    
    public void StopSpawning()
    {
        isSpawning = false;
    }
}
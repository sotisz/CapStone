using System.Collections;
using UnityEngine;
using TMPro;

public class Level9Director : MonoBehaviour
{
    [Header("연기자 설정")]
    public Transform tiger;        // 움직일 호랑이 오브젝트
    public Transform meatPosition; // 고기가 있는 위치 (카메라가 비출 곳)
    
    [Header("카메라 설정")]
    public Camera mainCamera;      // 메인 카메라
    public float targetZoom = 3f;  // 목표 줌 크기 (작을수록 확대)
    public float zoomSpeed = 0.5f; // 연출 속도 (작을수록 느림)

    [Header("감시할 대사")]
    public string targetText = "그래도... 한 입만..."; // 이 대사가 나오면 연출 시작!
    
    private bool isCutscenePlaying = false;
    private float originalZoom;

    void Start()
    {
        if (mainCamera != null)
        {
            originalZoom = mainCamera.orthographicSize;
        }
    }

    void Update()
    {
        // 대화 매니저가 켜져 있고, 아직 연출 전인지 확인
        if (DialogManager.instance == null || DialogManager.instance.dialogText == null)
            return;

        // 특정 대사가 나오면 연출 시작
        if (!isCutscenePlaying && DialogManager.instance.dialogText.text == targetText)
        {
            StartCoroutine(StartTensionScene());
        }
    }

    IEnumerator StartTensionScene()
    {
        isCutscenePlaying = true;
        
        // 1. 슬로우 모션 발동
        Time.timeScale = 0.5f; 
        Debug.Log("연출 시작: 카메라가 고기를 향해 이동하며 줌인합니다.");

        float t = 0;
        
        // 시작 당시의 값들을 저장 (Lerp를 위해)
        Vector3 startTigerPos = tiger.position;
        Vector3 startCameraPos = mainCamera.transform.position; // 카메라 시작 위치
        float startCameraSize = mainCamera.orthographicSize;    // 카메라 시작 크기

        
        Vector3 targetCameraPos = new Vector3(meatPosition.position.x, meatPosition.position.y, startCameraPos.z);

        while (t < 1f)
        {
            // Time.unscaledDeltaTime을 써야 슬로우 모션 중에도 카메라는 부드럽게 움직임
            t += Time.unscaledDeltaTime * zoomSpeed; 

            // [핵심 1] 카메라 줌인 (크기 조절)
            mainCamera.orthographicSize = Mathf.Lerp(startCameraSize, targetZoom, t);

            // [핵심 2] 카메라 이동 (고기 쪽으로)
            mainCamera.transform.position = Vector3.Lerp(startCameraPos, targetCameraPos, t);

            yield return null;
        }
    }

    private void OnDestroy()
    {
        Time.timeScale = 1f; // 씬이 끝나면 시간 원상복구
    }
}
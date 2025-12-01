using System.Collections;
using UnityEngine;
using TMPro;

public class Level9Director : MonoBehaviour
{
    [Header("환경 설정")]
    public GameObject cutsceneBarriers; 
    public Transform meatPosition;      
    public Transform playerTransform;   

    [Header("카메라 설정")]
    public Camera mainCamera;      
    public float targetZoom = 3.5f;   
    public float zoomInSpeed = 1.0f; 
    public float zoomOutSpeed = 2.0f;

    [Header("카메라 흔들림 설정")]
    public float shakeAmount = 0.2f;    
    public float shakeEndX = 100f;      

    [Header("대사 감시 설정")]
    public string startText = "그래도... 한 입만..."; 
    public string endText = "이제 그들은 살아남기 위해 달려야 했다!)"; 
    
    // [추가] 카메라 추적 스크립트를 제어하기 위한 변수
    private MoveCamera cameraScript;

    private bool isSequenceStarted = false;  
    private bool isSequenceFinished = false; 
    
    private Vector3 originalCameraPos;
    private float originalCameraSize;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
        
        // 시작할 때 메인 카메라에 붙어있는 MoveCamera 스크립트를 찾아서 기억해둠
        if (mainCamera != null)
        {
            cameraScript = mainCamera.GetComponent<MoveCamera>();
        }

        originalCameraPos = mainCamera.transform.position;
        originalCameraSize = mainCamera.orthographicSize;
    }

    void Update()
    {
        if (DialogManager.instance == null || DialogManager.instance.dialogText == null) return;
        
        string currentText = DialogManager.instance.dialogText.text;

        if (!isSequenceStarted && currentText == startText)
        {
            StartCoroutine(StartZoomIn());
        }

        if (isSequenceStarted && !isSequenceFinished && currentText == endText)
        {
            StartCoroutine(ZoomOutAndShakeRoutine());
        }
    }

    IEnumerator StartZoomIn()
    {
        isSequenceStarted = true;
        
        // [핵심] 연출 시작! 카메라가 플레이어 따라다니는 걸 멈춤 (스크립트 끄기)
        if (cameraScript != null) cameraScript.enabled = false;

        if (cutsceneBarriers != null) cutsceneBarriers.SetActive(true);

        float t = 0;
        Vector3 startCameraPos = mainCamera.transform.position;
        float startCameraSize = mainCamera.orthographicSize;
        Vector3 targetCameraPos = new Vector3(meatPosition.position.x, meatPosition.position.y + 1f, startCameraPos.z);

        while (t < 1f)
        {
            t += Time.deltaTime * zoomInSpeed;
            mainCamera.orthographicSize = Mathf.Lerp(startCameraSize, targetZoom, t);
            mainCamera.transform.position = Vector3.Lerp(startCameraPos, targetCameraPos, t);
            yield return null;
        }
    }

    IEnumerator ZoomOutAndShakeRoutine()
    {
        isSequenceFinished = true;
        if (cutsceneBarriers != null) cutsceneBarriers.SetActive(false); 

        float t = 0;
        Vector3 currentCameraPos = mainCamera.transform.position;
        float currentCameraSize = mainCamera.orthographicSize;

        while (t < 1f)
        {
            t += Time.deltaTime * zoomOutSpeed;
            
            Vector3 nextPos = Vector3.Lerp(currentCameraPos, originalCameraPos, t);
            nextPos.x += Random.Range(-shakeAmount, shakeAmount);
            nextPos.y += Random.Range(-shakeAmount, shakeAmount);

            mainCamera.orthographicSize = Mathf.Lerp(currentCameraSize, originalCameraSize, t);
            mainCamera.transform.position = nextPos;
            
            yield return null;
        }
        
        // [중요] 연출이 끝나고 줌도 원래대로 돌아왔으니
        // 다시 플레이어를 따라다니도록 스크립트를 켬!
        mainCamera.orthographicSize = originalCameraSize; // 크기 확실하게 원복
        mainCamera.transform.position = originalCameraPos; // 위치도 원복 (이건 어차피 MoveCamera가 덮어씌우겠지만)
        
        if (cameraScript != null) cameraScript.enabled = true;

        // 플레이 중 흔들림 (옵션)
        while (playerTransform != null && playerTransform.position.x < shakeEndX)
        {
            float shakeZ = Random.Range(-1f, 1f) * (shakeAmount * 5f); 
            mainCamera.transform.rotation = Quaternion.Euler(0, 0, shakeZ);
            yield return null;
        }

        mainCamera.transform.rotation = Quaternion.identity; 
        this.enabled = false; 
    }
}
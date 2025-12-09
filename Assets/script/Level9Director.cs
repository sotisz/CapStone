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
    // public string endText = "이제 그들은..."; // [삭제] 더 이상 텍스트로 흔들지 않음

    private MoveCamera cameraScript;
    private bool isSequenceStarted = false;
    private bool isSequenceFinished = false;

    private Vector3 originalCameraPos;
    private float originalCameraSize;

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

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



        if (!isSequenceFinished && playerTransform != null && meatPosition != null)

        {

            // 플레이어의 X좌표가 고기보다 2만큼 더 오른쪽으로 갔다면 (이미 지나침)

            if (playerTransform.position.x > meatPosition.position.x + 2.0f)

            {

                // 아직 지진이 안 났다면 강제로 실행

                StartCoroutine(ZoomOutAndShakeRoutine());

            }

        }

    }

    // [추가됨] 플레이어가 이 오브젝트(콜라이더)에 닿으면 지진 시작!
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 연출이 끝났으면 무시
        if (isSequenceFinished) return;

        // 플레이어가 닿았을 때
        if (collision.CompareTag("Player"))
        {
            Debug.Log("지진 트리거 작동!");
            StartCoroutine(ZoomOutAndShakeRoutine());
        }
    }

    IEnumerator StartZoomIn()
    {
        isSequenceStarted = true;
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

        mainCamera.orthographicSize = originalCameraSize;
        mainCamera.transform.position = originalCameraPos;

        if (cameraScript != null) cameraScript.enabled = true;

        // 플레이 중 흔들림 유지
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
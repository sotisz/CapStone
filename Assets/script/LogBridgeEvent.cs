using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogBridgeEvent : MonoBehaviour
{
    [Header("통나무 조각들")]
    public Rigidbody2D logTopRb;    // 위쪽 통나무 리지드바디
    public Rigidbody2D logBottomRb; // 아래쪽 통나무 리지드바디

    [Header("내려오기 설정")]
    public float targetY = 0f;      
    public float moveSpeed = 5f;    
    
    private bool isDescending = false; 

    [Header("씬 전환 설정")]
    public string nextSceneName = "Stage10_Cutscene"; 
    public float sceneDelay = 2.0f; 

    void Update()
    {
        if (isDescending)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            if (Mathf.Abs(transform.position.y - targetY) < 0.01f)
            {
                isDescending = false;
            }
        }
    }

    public void StartDescent()
    {
        isDescending = true;
    }

    // [핵심] 통나무 파괴 & 물리 충돌 끄기
    public void BreakAndFinish()
    {
        Debug.Log("통나무 파괴! 바닥을 뚫고 추락합니다.");

        // 1. 위쪽 통나무 처리
        if (logTopRb != null)
        {
            logTopRb.bodyType = RigidbodyType2D.Dynamic; // 중력 켜기
            logTopRb.angularVelocity = 15f; // 회전
            logTopRb.AddForce(Vector2.up * 2f + Vector2.right * 2f, ForceMode2D.Impulse); // 튕김

            // [추가] 콜라이더를 꺼서 유령으로 만듦 (모든 벽 통과)
            Collider2D col = logTopRb.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        // 2. 아래쪽 통나무 처리
        if (logBottomRb != null)
        {
            logBottomRb.bodyType = RigidbodyType2D.Dynamic; // 중력 켜기
            logBottomRb.angularVelocity = -10f;
            logBottomRb.AddForce(Vector2.down * 2f, ForceMode2D.Impulse);

            // [추가] 콜라이더를 꺼서 바닥을 뚫고 지나가게 함
            Collider2D col = logBottomRb.GetComponent<Collider2D>();
            if (col != null) col.enabled = false;
        }

        StartCoroutine(LoadNextSceneRoutine());
    }

    IEnumerator LoadNextSceneRoutine()
    {
        yield return new WaitForSeconds(sceneDelay);
        
        if (GameManager.Instance != null)
        {
             GameManager.Instance.LoadNextScene(); 
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
using UnityEngine;

public class LogBreakTrigger : MonoBehaviour
{
    public LogBridgeEvent logEvent; // 아까 만든 통나무 스크립트 연결

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 통나무 부수는 함수 호출!
            logEvent.BreakAndFinish();
            
            // 트리거 삭제 (중복 발동 방지)
            Destroy(gameObject);
        }
    }
}
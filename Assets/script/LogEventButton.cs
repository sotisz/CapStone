using UnityEngine;

public class LogEventButton : MonoBehaviour
{
    [Header("연결할 대상")]
    public LogBridgeEvent giantLogScript; // 통나무(GiantLog_Parent)에 붙은 스크립트

    [Header("버튼 설정")]
    public Sprite pressedSprite; // 눌렸을 때 바뀔 이미지 (선택 사항)
    private bool isPressed = false; // 이미 눌렸는지 체크
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 이미 눌렸으면 무시 (한 번만 작동)
        if (isPressed) return;

        // 플레이어나 돌 등이 밟았을 때
        if (collision.CompareTag("Player") || collision.CompareTag("BoxObject") || collision.CompareTag("Breakable"))
        {
            PressButton();
        }
    }

    private void PressButton()
    {
        isPressed = true;
        Debug.Log("통나무 버튼 눌림!");

        // 1. 버튼 이미지 변경 (눌린 모습으로)
        if (sr != null && pressedSprite != null)
        {
            sr.sprite = pressedSprite;
        }

        // 2. 통나무 내려오기 시작!
        if (giantLogScript != null)
        {
            giantLogScript.StartDescent();
        }
        else
        {
            Debug.LogError("LogEventButton: GiantLogScript가 연결되지 않았습니다!");
        }
    }
}
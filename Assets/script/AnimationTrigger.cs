using System.Collections;
using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
    private PBtn PbtnSc;
    private bool wasOpen = false;  // 이전 프레임 문 상태 저장
    public GameObject splictScreen;
    private bool isRunning = false; // 코루틴 중복 방지
    public float activeTime;

    private void Start()
    {
        PbtnSc = GetComponent<PBtn>();
    }

    private void Update()
    {
        // 문이 닫혀 있다가 → 열린 순간만 실행
        if (!wasOpen && PbtnSc.IsOpen)
        {
            if (!isRunning)
                StartCoroutine(ActiveBoard());
        }

        wasOpen = PbtnSc.IsOpen; // 상태 업데이트
    }

    IEnumerator ActiveBoard()
    {
        isRunning = true;

        splictScreen.SetActive(true);
        yield return new WaitForSeconds(activeTime);
        splictScreen.SetActive(false);

        isRunning = false;
    }
}

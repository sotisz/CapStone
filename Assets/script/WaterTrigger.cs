using System.Collections;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public Transform waterTransform;
    public float targetHeight = 28.3f;    // 물이 올라갈 목표 높이
    public float originalHeight = 0.0f;   // 물이 내려갈 초기 높이
    public float fillSpeed = 2.5f;        // 물이 차오르는(이동하는) 속도

    private Coroutine movingRoutine;

    void Start()
    {
        if (waterTransform != null)
            originalHeight = waterTransform.position.y;
    }

    public void RaiseWater()
    {
        // 이미 움직이고 있다면 멈추고 새로 시작
        if (movingRoutine != null)
            StopCoroutine(movingRoutine);
        movingRoutine = StartCoroutine(MoveWaterToHeight(targetHeight));
    }

    public void LowerWater()
    {
        if (movingRoutine != null)
            StopCoroutine(movingRoutine);
        movingRoutine = StartCoroutine(MoveWaterToHeight(originalHeight));
    }

    private IEnumerator MoveWaterToHeight(float targetY)
    {
        float currentY = waterTransform.position.y;
        Debug.Log($"현재 물 높이: {currentY}, 목표 높이: {targetY}");

        // 목표 높이와 현재 높이의 차이가 0.01보다 클 때까지 반복 (거의 도착할 때까지)
        while (Mathf.Abs(currentY - targetY) > 0.01f)
        {
            currentY = Mathf.MoveTowards(currentY, targetY, fillSpeed * Time.deltaTime);
            Vector3 pos = waterTransform.position;
            pos.y = currentY;
            waterTransform.position = pos;
            yield return null;
        }
    }
}
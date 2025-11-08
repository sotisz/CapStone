using System.Collections;
using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    public Transform waterTransform;
    public float targetHeight = 28.3f;     // 올라갈 목표 높이
    public float originalHeight = 0.0f;   // 내려갈 원위치 높이
    public float fillSpeed = 2.5f;        // 초당 높이 변화량

    private Coroutine movingRoutine;

    void Start()
    {
        if (waterTransform != null)
            originalHeight = waterTransform.position.y;
    }

    public void RaiseWater()
    {
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

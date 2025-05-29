using Bundos.WaterSystem;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform player; // 플레이어 참조
    public float interactionDistance = 2f; // 상호작용 거리
    public KeyCode interactionKey = KeyCode.E; // 상호작용 키
    public Water water; // Water 컴포넌트 참조
    public float waterRiseAmount = 1f; // 높아질 수위

    private bool isActivated = false;

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= interactionDistance && Input.GetKeyDown(interactionKey) && !isActivated)
        {
            ActivateLever();
        }
    }

    void ActivateLever()
    {
        isActivated = true;

        if (water != null)
        {
            Vector3 waterPos = water.transform.position;
            water.transform.position = new Vector3(waterPos.x, waterPos.y + waterRiseAmount, waterPos.z);
        }

        // 애니메이션이나 사운드도 여기에 추가 가능
        Debug.Log("레버 작동: 물 높이 상승");
    }
}

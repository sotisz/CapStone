using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterButton : MonoBehaviour
{
    public WaterTrigger waterTrigger; // 제어할 물 오브젝트
    public bool isRaiseButton = true; // true면 물 올림, false면 물 내림
    private GameObject player;

    private bool isPlayerInside = false; // 플레이어가 범위 안으로 들어왔다면 true

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider) // 버튼 상호작용 활성화
    {
        if (!collider.CompareTag("Player")) // 플레이어가 아니라면 return
            return;

        isPlayerInside = true;
        Debug.Log("플레이어가 버튼 범위안에 들어옴");
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (!collider.CompareTag("Player")) // 플레이어가 아니라면 return
            return;

        isPlayerInside = false;
        Debug.Log("벗어남");
    }

    void Update()
    {
        if (!isPlayerInside || waterTrigger == null)
            return;

        // 플레이어가 범위 안에서 E키 누를 때
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isRaiseButton)
            {
                Debug.Log("물 올리기 버튼 작동");
                waterTrigger.RaiseWater();
            }
            else
            {
                Debug.Log("물 내리기 버튼 작동");
                waterTrigger.LowerWater();
            }
        }
    }
}

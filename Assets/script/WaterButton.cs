using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterButton : MonoBehaviour
{
    public WaterTrigger waterTrigger; // 물을 제어할 스크립트 연결
    public bool isRaiseButton = true; // true면 물 올리기, false면 물 내리기 버튼
    private GameObject player;

    private bool isPlayerInside = false; // 플레이어가 버튼 범위 안에 있으면 true

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collider) // 버튼 범위 진입 감지
    {
        if (!collider.CompareTag("Player")) // 플레이어가 아니라면 리턴
            return;

        isPlayerInside = true;
        Debug.Log("플레이어가 버튼 범위 안에 진입함");
    }

    private void OnTriggerExit2D(Collider2D collider) // 버튼 범위 탈출 감지
    {
        if (!collider.CompareTag("Player")) // 플레이어가 아니라면 리턴
            return;

        isPlayerInside = false;
        Debug.Log("플레이어가 버튼 범위를 벗어남");
    }

    void Update()
    {
        // 플레이어가 없거나 트리거가 연결 안 됐으면 리턴
        if (!isPlayerInside || waterTrigger == null)
            return;

        // 플레이어가 범위 안에서 E키를 눌렀을 때
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isRaiseButton)
            {
                Debug.Log("물 올리기 버튼 작동!");
                waterTrigger.RaiseWater();
            }
            else
            {
                Debug.Log("물 내리기 버튼 작동!");
                waterTrigger.LowerWater();
            }
        }
    }
}
using UnityEngine;
using System.Collections;
public class wraptrap : MonoBehaviour
{
    public Transform endPoint;

    // 플레이어가 트리거에 닿을 때마다 즉시 이동
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.position = endPoint.position;
        }
    }

}

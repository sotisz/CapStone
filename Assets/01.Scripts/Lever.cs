using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform player; // 플레이어 참조
    public float interactionDistance = 2f; // 상호작용 거리
    public KeyCode interactionKey = KeyCode.E; // 상호작용 키
    public Transform door;
    public Vector3 doorPos = new Vector3(0, 0, 0); // 문이 이동할 오프셋
    public float moveSpeed = 2f;

    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private bool isActivated = false;

    private Quaternion leverDefaultRot;      // 초기 회전값
    private Quaternion leverActivatedRot;    // -30도 회전값

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + doorPos;

        leverDefaultRot = transform.rotation;
        leverActivatedRot = Quaternion.Euler(0, 0, -30); 
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= interactionDistance && Input.GetKeyDown(interactionKey))
        {
            isActivated = !isActivated;
        }

        
        if (isActivated)
        {
            door.position = Vector3.MoveTowards(door.position, doorOpenPos, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, leverActivatedRot, 10f * Time.deltaTime); 
        }
        else
        {
            door.position = Vector3.MoveTowards(door.position, doorClosedPos, moveSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, leverDefaultRot, 10f * Time.deltaTime); 
        }
    }
}
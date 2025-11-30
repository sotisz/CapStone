using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public float interactionDistance = 2f; // 상호작용 거리
    public KeyCode interactionKey = KeyCode.E; // 상호작용 키

    public Transform door;
    public Vector3 doorPos = new Vector3(0, 0, 0); // 문이 이동할 오프셋
    public float moveSpeed = 2f;

    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;

    public LeverManager lever_manager;

    private Quaternion leverDefaultRot; // 초기 회전값
    private Quaternion leverActivatedRot; // -30도 회전값

    private BearController bear;

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + doorPos;

        leverDefaultRot = transform.rotation;
        leverActivatedRot = Quaternion.Euler(0, 0, -30);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!bear)
            bear = other.gameObject.GetComponent<BearController>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (bear != null && bear.gameObject == other.gameObject)
            bear = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(interactionKey) && bear)
        {
            Debug.Log("가져옴");
            lever_manager.isActivated = !lever_manager.isActivated;
        }
        if (lever_manager.isActivated)
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
using UnityEngine;

public class PBtn : MonoBehaviour
{
    public Transform Object; //버튼과 상호작용할 오브젝트
    public Transform door;
    public Vector3 doorPos = new Vector3(0,0,0); // 가로도 쓰고 세로도 쓸거라서 따로 지정은 안함
    public float moveSpeed = 2f;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private bool isOpen = false;
    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + doorPos;
    }

    // Update is called once per frame
    void Update()
    {
        if (isOpen)
        {
            door.position = Vector3.MoveTowards(door.position, doorOpenPos, moveSpeed * Time.deltaTime);
        }
        else
        {
            door.position = Vector3.MoveTowards(door.position, doorClosedPos, moveSpeed * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("BoxObject") || Object)
        {
            isOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("BoxObject") || Object)
        {
            isOpen = false;
        }
    }
}

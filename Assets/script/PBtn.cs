using UnityEngine;

public class PBtn : MonoBehaviour
{
    public enum ButtonType
    {
        Hold, // 지속,RedBtn
        OneTime // 한번(닫힘 불가능), BlueBtn
    }
    public ButtonType buttonType = ButtonType.Hold;

    public Transform Object; //버튼과 상호작용할 오브젝트
    public Transform door;
    public Vector3 doorPos = new Vector3(0,0,0); // 가로도 쓰고 세로도 쓸거라서 따로 지정은 안함
    public float moveSpeed = 2f;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private bool isOpen = false;
    public bool IsOpen => isOpen;
    

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
        if (other.CompareTag("BoxObject") || other.CompareTag("Player") || Object)
        {
            if (buttonType == ButtonType.Hold)
            {
                isOpen = true;
            }
            else if (buttonType == ButtonType.OneTime)
            {
                isOpen = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (buttonType == ButtonType.Hold)
        {
            if (other.CompareTag("BoxObject") || other.CompareTag("Player") || Object)
            {
                isOpen = false; 
            } 
        } // OneTime 타입은 Exit하는 동작 없음 -> 계속 열려 있음(닫을 수 없음)
    }
}

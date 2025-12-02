using UnityEngine;

public class PBtn : MonoBehaviour
{
    public enum ButtonType
    {
        Hold, 
        OneTime
    }
    public ButtonType buttonType = ButtonType.Hold;

    public Transform Object; 
    public Transform door;
    public Vector3 doorPos = new Vector3(0,0,0);
    public float moveSpeed = 2f;
    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;
    private bool isOpen = false;
    public bool IsOpen => isOpen;

    // 🔊 사운드 추가
    public AudioClip buttonPressSound;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public float soundVolume = 1f;

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + doorPos;
    }

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
        if (other.CompareTag("Breakable") || other.CompareTag("BoxObject") || other.CompareTag("Player"))
        {
            if (!isOpen)   // 이미 열려있을 때 소리 중복 방지
            {
                isOpen = true;
                
                if (buttonPressSound != null)
                    AudioSource.PlayClipAtPoint(buttonPressSound, transform.position, soundVolume);
                
                if (doorOpenSound != null)
                    AudioSource.PlayClipAtPoint(doorOpenSound, door.position, soundVolume);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (buttonType == ButtonType.Hold)
        {
            if (other.CompareTag("Breakable") || other.CompareTag("BoxObject") || other.CompareTag("Player"))
            {
                if (isOpen) // 닫힐 때 딱 한 번만 소리
                {
                    isOpen = false;
                    
                    if (doorCloseSound != null)
                        AudioSource.PlayClipAtPoint(doorCloseSound, door.position, soundVolume);
                }
            }
        }
    }
}

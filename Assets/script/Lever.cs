using System;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public float interactionDistance = 2f;
    public KeyCode interactionKey = KeyCode.E;

    public Transform door;
    public Vector3 doorPos = new Vector3(0, 0, 0);
    public float moveSpeed = 2f;

    private Vector3 doorClosedPos;
    private Vector3 doorOpenPos;

    public LeverManager lever_manager;

    private Quaternion leverDefaultRot;
    private Quaternion leverActivatedRot;

    private BearController bear;

    // 사운드
    public AudioClip leverSound;
    public AudioClip doorOpenSound;
    public AudioClip doorCloseSound;
    public float soundVolume = 1f;

    private bool prevActivateState = false;

    void Start()
    {
        doorClosedPos = door.position;
        doorOpenPos = door.position + doorPos;

        leverDefaultRot = transform.rotation;
        leverActivatedRot = Quaternion.Euler(0, 0, -30);

        prevActivateState = lever_manager.isActivated;
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
        // 레버를 당길 때
        if (Input.GetKeyDown(interactionKey) && bear)
        {
            lever_manager.isActivated = !lever_manager.isActivated;

            // 레버 당기는 소리
            if (leverSound != null)
                SoundManager.Instance.PlaySFX(leverSound);
        }

        // 상태가 변화했을 때 문 소리를 재생
        if (lever_manager.isActivated != prevActivateState)
        {
            if (lever_manager.isActivated)
            {
                // 문 열림 소리
                if (doorOpenSound != null)
                    SoundManager.Instance.PlaySFX(doorOpenSound);
            }
            else
            {
                // 문 닫힘 소리
                if (doorCloseSound != null)
                    SoundManager.Instance.PlaySFX(doorCloseSound);
            }

            prevActivateState = lever_manager.isActivated;
        }

        // 문 및 레버 애니메이션
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

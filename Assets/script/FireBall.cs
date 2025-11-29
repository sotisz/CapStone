using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float riseHeight = 3f;
    public float moveDuration = 1f;
    public float waitAtBottom = 1.5f;   // 바닥에서 기다리는 시간

    private Vector3 startPos;
    private Vector3 targetPos;

    private float timer = 0f;
    private bool rising = true;
    private bool waiting = false;
    private float waitTimer = 0f;

    void Start()
    {
        startPos = transform.position;
        targetPos = startPos + new Vector3(0, riseHeight, 0);
    }

    void Update()
    {
        //내려와서 대기 중이면 여기서 머무름
        if (waiting)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitAtBottom)
            {
                waiting = false;
                StartRising();    // 다시 상승 시작
            }
            return;
        }

        timer += Time.deltaTime;
        float t = Mathf.Clamp01(timer / moveDuration);

        if (rising)
        {
            // 올라가기
            transform.position = Vector3.Lerp(startPos, targetPos, t);

            if (t >= 1f)
            {
                StartFalling();     // 즉시 하강
            }
        }
        else
        {
            //내려가기
            transform.position = Vector3.Lerp(targetPos, startPos, t);

            // 내려갈 때 자연스럽게 회전
            transform.rotation = Quaternion.Lerp(
                Quaternion.Euler(0, 0, 0),
                Quaternion.Euler(0, 0, 180),
                t
            );

            if (t >= 1f)
            {
                // 대기 시작
                BeginWait();
            }
        }
    }

    void BeginWait()
    {
        waiting = true;
        waitTimer = 0f;

        // 내려온 뒤 회전 원복
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    void StartRising()
    {
        rising = true;
        timer = 0f;
    }

    void StartFalling()
    {
        rising = false;
        timer = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IKillable player = collision.gameObject.GetComponent<IKillable>();
            if(player != null)
            {
                player.Dead();
            }
        }
    }
}

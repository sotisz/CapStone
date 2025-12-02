using UnityEngine;

public class LavaRockTrigger : MonoBehaviour
{
    [Header("������ ������Ʈ")]
    public GameObject breakableFloor;
    public Transform lava;

    [Header("������")]
    public float rockGravity = 3.0f;
    public float lavaRiseSpeed = 2.0f;
    public float lavaStopY = -2.0f;

    private Rigidbody2D rb;
    private bool hasFallen = false;
    private bool isLavaRising = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void Update()
    {
        if (isLavaRising && lava != null)
        {
            if (lava.position.y < lavaStopY)
            {
                lava.Translate(Vector3.up * lavaRiseSpeed * Time.deltaTime);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasFallen && (collision.CompareTag("Player") || collision.gameObject.name.Contains("Player")))
        {
            Debug.Log("�÷��̾� ����! ���� ����.");
            hasFallen = true;

            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = rockGravity;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == breakableFloor)
        {
            Debug.Log("��! �ٴ� �ı� & ��� ����.");

            // 1. �ٴ� ���ֱ�
            breakableFloor.SetActive(false);

            // 2. [�ٽ�] ���� �� �ڸ��� ����! (������Ű��)
            // �� �̻� �߷��� ������ �ްų�, �ٸ� ��ü(���)�� �и��� �ʰ� ��
            rb.linearVelocity = Vector2.zero;       // ������ ����
            rb.angularVelocity = 0f;          // ȸ�� ����
            rb.bodyType = RigidbodyType2D.Kinematic; // ���� ���� ���� (����)

            // 3. ��� �ø��� ����ġ ON
            if (lava != null)
            {
                lava.gameObject.SetActive(true);
                isLavaRising = true;
            }
        }
    }
}
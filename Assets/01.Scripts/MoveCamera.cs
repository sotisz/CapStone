using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform Bear;
    public Transform Tiger;
    public float speed;

    public Vector2 center;
    public Vector2 size;
    float height;
    float width;
    

    void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    void LateUpdate()
    {
        Transform target = null;

        if (Bear != null && Bear.gameObject.activeInHierarchy)
        {
            target = Bear;
        }else if (Tiger != null && Tiger.gameObject.activeInHierarchy)
        {
            target = Tiger;
        }
        if (target != null)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);

            float lx = size.x * 0.5f - width;
            float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, center.x + lx);

            float ly = size.y * 0.5f - height;
            float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, center.y + ly);

            transform.position = new Vector3(clampX, clampY, -10f);
        }
    }
}
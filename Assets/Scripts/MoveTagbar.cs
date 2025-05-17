using UnityEngine;

public class MoveTagbar : MonoBehaviour
{
    public Transform target;
    public Vector3 Offset = new Vector3(0, 1.2f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + Offset;
        }
    }
}

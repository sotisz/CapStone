using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveCamera : MonoBehaviour
{
    public GameObject Bear;
    public GameObject Tiger;
    public Transform target;
    public Transform target2;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void LateUpdate()
    {
        if(Bear != null && Bear.activeInHierarchy)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
            transform.position = new Vector3(target.position.x, transform.position.y, -10f);
        }
        else if(Tiger != null && Tiger.activeInHierarchy)
        {
            transform.position = Vector3.Lerp(transform.position, target2.position, Time.deltaTime * speed);
            transform.position = new Vector3(target2.position.x, transform.position.y, -10f);
        }



    }
}

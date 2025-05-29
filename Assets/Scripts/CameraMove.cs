using System;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject target1, target2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate()
    {
        if(target1.active)
            transform.position = new Vector3(target1.transform.position.x, target1.transform.position.y, transform.position.z);
        else if(target2.active)
            transform.position = new Vector3(target2.transform.position.x, target2.transform.position.y, transform.position.z);
    }
}

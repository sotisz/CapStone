using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class MoveCamera : MonoBehaviour
{
    public GameObject Bear;
    public GameObject Tiger;
    public float speed = 3;
    
    // Update is called once per frame
    void LateUpdate()
    {
        if(Bear && Bear.activeInHierarchy)
        {
            Transform Bt = Bear.transform;
            transform.position = Vector3.Lerp(transform.position, Bt.position, Time.deltaTime * speed);
            transform.position = new Vector3(Bt.position.x, transform.position.y, -10f);
        }
        else if(Tiger && Tiger.activeInHierarchy)
        {
            Transform Tt = Tiger.transform;
            transform.position = Vector3.Lerp(transform.position, Tt.position, Time.deltaTime * speed);
            transform.position = new Vector3(Tt.position.x, transform.position.y, -10f);
        }



    }
}

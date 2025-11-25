using UnityEngine;

public class PlayerSync : MonoBehaviour
{
    public GameObject bear;
    public GameObject tiger;

    public Transform playerT;

    private void Start()
    {
        bear = playerT.GetChild(1).gameObject;
        tiger = playerT.GetChild(2).gameObject;
    }
    void LateUpdate()
    {
        if (bear.activeInHierarchy)
        {
            tiger.transform.position = bear.transform.position;
        }
        else if (tiger.activeInHierarchy)
        { 
            bear.transform.position = tiger.transform.position;
        }
    }
}

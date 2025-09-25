using UnityEngine;

public class BreakSystem : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Break()
    {
        Animator anim;
        anim = gameObject.GetComponent<Animator>();
        if (anim)
            anim.SetTrigger("Break");
        gameObject.SetActive(false);
    }
}

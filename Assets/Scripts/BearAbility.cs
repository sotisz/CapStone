using UnityEngine;

public class BearAbility : MonoBehaviour
{
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Punch()
    {
        anim.SetBool("Punch", true);
    }

    public void Smell()
    {
        
    }
}

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
        Debug.Log("펀치 애니메이션");
        anim.SetBool("Punch", true);
    }

    public void Smell()
    {
        
    }

    public void Push()
    {
        
    }
}

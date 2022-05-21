using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DatingController : MonoBehaviour
{
    public Animator anim;

    private int animID_attempt = Animator.StringToHash("Attempt");
    private int animID_success = Animator.StringToHash("Success");
    private int animID_failure = Animator.StringToHash("Failure");

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Success()
    {
        anim.SetTrigger(animID_success);
    }

    public void Attempt()
    {
        anim.SetTrigger(animID_attempt);
    }

    public void Failure()
    {
        anim.SetTrigger(animID_failure);
    }
}

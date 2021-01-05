using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSystem : MonoBehaviour, IAnimationSystem
{
    private Animator animator;

    [SerializeField] private string crouch = "isCrouching";

    void Awake()
    {
        animator = this.GetComponent<Animator>();
    }

    public void SetCrouching(bool value)
    {
        bool currentValue = animator.GetBool(crouch);

        if (currentValue != value)
        {
            animator.SetBool(crouch, value);
        }
    }
}

/*
    =================MirLaboratories=================
    Created by: Klein Holland
    Email: kpeholland@gmail.com
    =================================================
*/

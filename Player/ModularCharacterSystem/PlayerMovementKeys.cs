using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementKeys : MonoBehaviour, IPlayerInput
{
    IAnimationSystem animationSystem;

    private bool keysSet = false;

    private KeyCode forward = KeyCode.W;
    private KeyCode backward = KeyCode.S;
    private KeyCode left = KeyCode.A;
    private KeyCode right = KeyCode.D;
    private KeyCode jump = KeyCode.Space;
    private KeyCode crouch = KeyCode.LeftShift;
    private bool isCrouching = false;

    void Awake()
    {
        animationSystem = GetComponent<IAnimationSystem>();
    }

    private void Update()
    {
        if(keysSet == false)
        {
            if(KeyBindingsManager.i != null)
            {
                KeyBindingsManager bindings = KeyBindingsManager.i;

                forward = bindings.Forward;
                backward = bindings.Backward;
                left = bindings.Left;
                right = bindings.Right;
                jump = bindings.Jump;
                crouch = bindings.Crouch;

                keysSet = true;
            }
        }

        float moveX = 0f;
        float moveY = 0f;
        float moveZ = 0f;

        if (Input.GetKey(forward))
        {
            moveZ = +1f;
        }
        if (Input.GetKey(backward))
        {
            moveZ = -1f;
        }
        if (Input.GetKey(left))
        {
            moveX = -1f;
        }
        if (Input.GetKey(right))
        {
            moveX = +1f;
        }
        if (Input.GetKey(jump))
        {
            moveY = +1f;
        }
        if (Input.GetKeyDown(crouch))
        {
            isCrouching = true;
            animationSystem.SetCrouching(isCrouching);
        }
        else if (Input.GetKeyUp(crouch))
        {
            isCrouching = false;
            animationSystem.SetCrouching(isCrouching);
        }

        Vector3 moveDirection = new Vector3(moveX, moveY, moveZ).normalized;
        GetComponent<IMoveVelocity>().SetVelocity(moveDirection);
    }

    public bool IsCrouching()
    {
        return isCrouching;
    }
}

/*
    =================MirLaboratories=================
    Created by: Klein Holland
    Email: kpeholland@gmail.com
    =================================================
*/
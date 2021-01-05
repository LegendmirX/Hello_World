using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocityCharacterController : MonoBehaviour, IMoveVelocity
{
    private IPlayerInput playerInput;

    [SerializeField] private LayerMask groundLayer;
    private float moveSpeed;
    [SerializeField] private float jumpForce;
    private float fallSpeed;
    [SerializeField] private float gravity = -9.81f;

    private Vector3 velocityDirection;
    private CharacterController characterController;
    private AnimationSystem animator;

    public void Awake()
    {
        var m = GetComponent<ICharacterSheet>().GetStat(StatType.MoveSpeed);
        this.moveSpeed = (m == null ? 0f : m.Value);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<AnimationSystem>();
        playerInput = GetComponent<IPlayerInput>();
    }
    public void SetVelocity(Vector3 veclocityVector)
    {
        velocityDirection = veclocityVector;
    }

    private void FixedUpdate()
    {
        float fixedDeltaTime = Time.fixedDeltaTime;
        bool isGrounded = IsGrounded();
        bool isCrouching = playerInput.IsCrouching();

        if (isGrounded == true)
        {
            fallSpeed = 0;
        }
        else
        {
            fallSpeed += gravity * fixedDeltaTime / 2;
        }

        if (isGrounded == true && isCrouching == false)
        {
            fallSpeed += velocityDirection.y * fixedDeltaTime * jumpForce;
        }

        float MoveX = (velocityDirection.x * fixedDeltaTime) * moveSpeed;
        float MoveY = fallSpeed;
        float MoveZ = (velocityDirection.z * fixedDeltaTime) * moveSpeed;

        Vector3 movementThisFrame = transform.right * MoveX + transform.up * MoveY + transform.forward * MoveZ;

        characterController.Move(movementThisFrame);
    }

    private bool IsGrounded()
    {
        //Vector3 rayStart = this.transform.TransformPoint(characterController.center);
        //float rayLength = characterController.height/2 + 0.01f;
        //bool hasHit = Physics.SphereCast(rayStart, characterController.radius * 0.9f, Vector3.down, out RaycastHit hitInfo, rayLength, groundLayer);

        return characterController.isGrounded;
    }
}

/*
    =================MirLaboratories=================
    Created by: Klein Holland
    Email: kpeholland@gmail.com
    =================================================
*/
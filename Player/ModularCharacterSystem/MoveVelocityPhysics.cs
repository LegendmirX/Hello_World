using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocityPhysics : MonoBehaviour, IMoveVelocity
{
    private float moveSpeed;

    private Vector3 velocityDirection;
    private Rigidbody2D rb2D;
    private AnimationSystem animationSystem;

    public void Awake()
    {
        var m = GetComponent<ICharacterSheet>().GetStat(StatType.MoveSpeed);
        this.moveSpeed = (m == null ? 0f : m.Value);
        rb2D = GetComponent<Rigidbody2D>();
        animationSystem = GetComponent<AnimationSystem>();
    }

    public void SetVelocity(Vector3 velcocityDirection)
    {
        this.velocityDirection = velcocityDirection;
    }

    private void FixedUpdate()
    {
        rb2D.velocity = velocityDirection * moveSpeed;
    }
}

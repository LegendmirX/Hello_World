using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocityTransform : MonoBehaviour, IMoveVelocity 
{
    [SerializeField] private float moveSpeed;

    [SerializeField] private Vector3 velocityDirection;
    private AnimationSystem animationSystem;

    public void Awake()
    {
        var m = GetComponent<ICharacterSheet>().GetStat(StatType.MoveSpeed);
        this.moveSpeed = (m == null ? 0f : m.Value);
        animationSystem = GetComponent<AnimationSystem>();
    }

    public void SetVelocity(Vector3 velcocityDirection)
    {
        if(velcocityDirection != Vector3.zero)
        {
            SoundManager.PlaySound(SoundManager.Sound.PlayerWalk);
        }
        else
        {
            SoundManager.StopSound(SoundManager.Sound.PlayerWalk);
        }
        this.velocityDirection = velcocityDirection;
    }

    private void Update()
    {
        transform.position += velocityDirection * moveSpeed * Time.deltaTime;
    }
}

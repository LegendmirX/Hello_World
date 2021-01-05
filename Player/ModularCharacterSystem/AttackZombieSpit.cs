using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackZombieSpit : MonoBehaviour, IAttack
{
    GameObject projectile;
    public float AttackRange;
    public float directionMultiplier = 0.8f;

    [SerializeField]
    private float attackCoolDownTime = 0.5f;
    private float attackCoolDownTimer = 0f;
    private bool canAttack = true;

    private void Start()
    {
        //projectile = GameAssets.i.ZombieSpit;
    }

    private void Update()
    {
        if (canAttack == true)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        UpdateAttackTimer(deltaTime);
    }

    private void UpdateAttackTimer(float deltaTime)
    {
        attackCoolDownTimer += deltaTime;

        if (attackCoolDownTimer >= attackCoolDownTime)
        {
            canAttack = true;
            attackCoolDownTimer = 0f;
        }
    }

    public void AltAttack(Vector3 attackDirection)
    {
        throw new System.NotImplementedException();
    }

    public void Attack(Vector3 attackDirection)
    {
        if (canAttack == false)
        {
            return;
        }

        GameObject go = Instantiate(projectile, new Vector3(Position().x + (attackDirection.x * directionMultiplier), Position().y + (attackDirection.y * directionMultiplier), Position().z), Quaternion.identity);
        go.transform.LookAt(new Vector3(go.transform.position.x + attackDirection.x, go.transform.position.y + attackDirection.y, go.transform.position.z + attackDirection.z), new Vector3(1,0));
        canAttack = false;
    }

    public float Range()
    {
        return this.AttackRange;
    }


    private Vector3 Position()
    {
        return this.transform.position;
    }
}

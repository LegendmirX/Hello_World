using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAttack : MonoBehaviour, IAttack 
{
    GameObject projectile;
    float directionMultiplier = 1f;

    [SerializeField]
    private float attackCoolDownTime = 0.5f;
    private float attackCoolDownTimer = 0f;
    private bool canAttack = true;

    private void Start()
    {
        //projectile = GameAssets.i.Projectile;
    }

    private void Update()
    {
        if(canAttack == true)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        UpdateAttackTimer(deltaTime);
    }

    private void UpdateAttackTimer(float deltaTime)
    {
        attackCoolDownTimer += deltaTime;

        if(attackCoolDownTimer >= attackCoolDownTime)
        {
            canAttack = true;
            attackCoolDownTimer = 0f;
        }
    }

    public void Attack(Vector3 attackDirection)
    {
        if (canAttack == false)
        {
            return;
        }

        GameObject obj = Instantiate(projectile, new Vector3(Position().x + (attackDirection.x * directionMultiplier), Position().y + (attackDirection.y * directionMultiplier), Position().z), Quaternion.identity);
        obj.transform.LookAt(new Vector3(obj.transform.position.x + attackDirection.x, obj.transform.position.y + attackDirection.y, obj.transform.position.z + attackDirection.z));
        //obj.GetComponent<Projetile>().WhoFiredMe = this.transform;
        canAttack = false;
    }

    public void AltAttack(Vector3 attackDirection)
    {
        if (canAttack == false)
        {
            return;
        }

        //TODO: Impliment an Alt Attack
        canAttack = false;
    }

    private Vector3 Position()
    {
        return this.transform.position;
    }

    public float Range()
    {
        //TODO fix this
        return 10f;
    }
}

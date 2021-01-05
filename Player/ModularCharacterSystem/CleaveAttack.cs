using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaveAttack : MonoBehaviour, IAttack
{
    public float Range = 0.7f;
    public float Radius = 0.4f;
    public float Damage = 10f;

    private GameObject cleave;

    private List<GameObject> toDestroy;
    #region Timer
    private float destroyTimer = 0f;
    private float destroyTime = 0.1f;
    #endregion

    [SerializeField] private float attackCoolDownTime = 0.5f;
    private float attackCoolDownTimer = 0f;
    private bool canAttack = true;

    private void Start()
    {
        //cleave = GameAssets.i.Cleave;
        toDestroy = new List<GameObject>();
    }

    private void Update()
    {
        if (toDestroy.Count > 0)
        {
            destroyTimer += Time.deltaTime;

            if (destroyTimer >= destroyTime)
            {
                destroyTimer = 0f;
                GameObject go = toDestroy[0];
                toDestroy.Remove(go);
                Destroy(go);
            }
        }

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

    public void Attack(Vector3 attackDirection)
    {
        //TODO: Play attack action

        Collider2D[] hits = Physics2D.OverlapCircleAll(Position() + (attackDirection * Range), Radius);
        GameObject go = Instantiate(cleave, Position() + (attackDirection * Range), Quaternion.identity);
        toDestroy.Add(go);
        go.transform.localScale = new Vector3((Radius * 2), (Radius * 2));

        foreach(Collider2D other in hits)
        {
            if(other.transform == this.transform)
            {
                continue;
            }

            IDamageable idam = other.GetComponent<IDamageable>();

            if(idam != null)
            {
                idam.OnHit(Damage);
            }
        }
        //GameObject obj = Instantiate(cleave, new Vector3(Position().x + (attackDirection.x * Range), Position().y + (attackDirection.y * Range), Position().z + heightFromFloor), Quaternion.identity);
        //obj.transform.LookAt(new Vector3(obj.transform.position.x + attackDirection.x, obj.transform.position.y + attackDirection.y, obj.transform.position.z + attackDirection.z));
    }

    public void AltAttack(Vector3 attackDirection)
    {
        throw new System.NotImplementedException();
    }

    float IAttack.Range()
    {
        return Range + Radius;
    }

    private Vector3 Position()
    {
        return this.transform.position;
    }


}

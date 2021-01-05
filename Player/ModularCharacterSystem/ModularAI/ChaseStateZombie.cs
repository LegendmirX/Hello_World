using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
using Unity.Mathematics;


public class ChaseStateZombie : MonoBehaviour, IChaseState
{
    [SerializeField]

    int2? destination;

    [SerializeField]
    bool isSetMovePositionCalled = false;
    public float range;

    public void Execute(GameObject target)
    {
        if (target != null && isSetMovePositionCalled == false)
        {
            destination = new int2(WorldController.current.RoundPositionToInt(target.transform.position));
            GetComponent<IMovePosition>().SetMovePosition(new Vector3(destination.Value.x, destination.Value.y), OnFailedPathCallback);
            isSetMovePositionCalled = true;
        }

        if (destination != null)
        {
            if (Vector3.Distance(Position(), new Vector3(destination.Value.x, destination.Value.y)) <= range)
            {
                destination = null;
                isSetMovePositionCalled = false;
            }
        }
    }

    public bool CanSeeTarget(GameObject target)
    {
        Vector3 dirToTarget = (target.transform.position - Position()).normalized;
        Vector2 rayStart = new Vector2(Position().x + dirToTarget.x, Position().y + dirToTarget.y);
        float disToTarget = Vector3.Distance(Position(), target.transform.position);

        RaycastHit2D hitInfo = Physics2D.Raycast(rayStart, dirToTarget, disToTarget);

        if (hitInfo == true)
        {
            Debug.DrawLine(transform.position, hitInfo.point, Color.red, 0.1f);

            if(hitInfo.transform.gameObject == target)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsInRange(GameObject target)
    {
        if(Vector3.Distance(Position(), target.transform.position) <= range)
        {
            return true;
        }

        return false;
    }

    public void SetRange(float range)
    {
        this.range = range;
    }

    public void ResetState()
    {
        destination = null;
        isSetMovePositionCalled = false;
    }

    private void OnFailedPathCallback()
    {
        destination = null;
        isSetMovePositionCalled = false;
    }

    private Vector3 Position()
    {
        return this.transform.position;
    }
}

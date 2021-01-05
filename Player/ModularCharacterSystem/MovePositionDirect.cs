using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovePositionDirect : MonoBehaviour, IMovePosition
{
    private Vector3 moveDestination;

    public void SetMovePosition(Vector3 moveDestination, Action onFailedPath = null)
    {
        this.moveDestination = moveDestination;
    }

    private void Update()
    {
        Vector3 moveDirection = (moveDestination - transform.position).normalized;
        GetComponent<IMoveVelocity>().SetVelocity(moveDirection);
    }
}

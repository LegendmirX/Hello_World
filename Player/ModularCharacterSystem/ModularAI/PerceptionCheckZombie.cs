using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
using Unity.Mathematics;


public class PerceptionCheckZombie : MonoBehaviour, IPerceptionCheck
{

    private int2 previousPos;


    #region Timer
    [SerializeField]
    float checkTime = 2;
    float checkTimer;
    bool canCheck = false;
    #endregion

    private void Update()
    {
        if (canCheck == true)
        {
            return;
        }

        float deltaTime = Time.deltaTime;
        UpdateCheckTimer(deltaTime);
    }

    public GameObject Check()
    {
        int2 currentPos = WorldController.current.RoundPositionToInt(Position());

        if(currentPos.x == previousPos.x && currentPos.y == previousPos.y)
        {
            if(canCheck == true)
            {
                canCheck = false;
                return CheckArea(currentPos);
            }
            return null;
        }

        previousPos = currentPos;

        return CheckArea(currentPos);
    }

    private void UpdateCheckTimer(float deltaTime)
    {
        checkTimer += deltaTime;

        if(checkTimer >= checkTime)
        {
            checkTimer = 0;
            canCheck = true;
        }
    }


    private GameObject CheckArea(int2 currentPos)

    {
        Transform target;
        List<Transform> visableTargets = GetComponent<FieldOfView>().FindVisableTargets();

        if(visableTargets.Count <= 0)
        {
            return null;
        }
        else if (visableTargets.Count > 1)
        {
            Transform closestTarget = null;
            float dist = Mathf.Infinity;

            for (int i = 0; i < visableTargets.Count; i++)
            {
                Transform targetCheck = visableTargets[i];
                float distCheck = Vector3.Distance(this.transform.position, targetCheck.position);

                if(distCheck < dist)
                {
                    closestTarget = targetCheck;
                    dist = distCheck;
                }
            }

            target = closestTarget;
        }
        else
        {
            target = visableTargets[0];
        }

        return target.gameObject;
    }

    private Vector3 Position()
    {
        return this.transform.position;
    }
}


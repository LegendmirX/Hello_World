using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
using Unity.Mathematics;


public class WanderStateZombie : MonoBehaviour, IWanderState
{
    int2 areaCentre = int2.zero;
    int wanderRadius = 1;
    bool restrictToArea = false;

    [SerializeField] private float wanderDistance = 5f;
    [SerializeField] private int2? destination;
    [SerializeField] private bool isSetMovePositionCalled = false;

    [SerializeField] private float minWanderDelay = 0f;
    [SerializeField] private float maxWanderDelay = 5f;
    private float wanderDelayTimer = 0f;
    private float wanderDelay;
    

    public void Execute()
    {
        if(destination == null)
        {
            if(wanderDelay == 0)
            {
                wanderDelay = UnityEngine.Random.Range(minWanderDelay, maxWanderDelay);
            }

            wanderDelayTimer += Time.deltaTime;
            if(wanderDelayTimer >= wanderDelay)
            {
                wanderDelay = 0;
                wanderDelayTimer = 0;
                destination = LookForDestination();
                return;
            }
        }
        
        if(destination != null && isSetMovePositionCalled == false)
        {
            GetComponent<IMovePosition>().SetMovePosition(new Vector3(destination.Value.x, destination.Value.y), OnFailedPathCallback);
            isSetMovePositionCalled = true;
        }

        if(destination != null)
        {
            if (Vector3.Distance(Position(), new Vector3(destination.Value.x, destination.Value.y)) <= 0.1)
            {
                destination = null;
                isSetMovePositionCalled = false;
            }
        }
    }

    public void SetDestination(int2 destination)
    {
        this.destination = destination;
        isSetMovePositionCalled = false;
    }

    private void OnFailedPathCallback()
    {
        destination = null;
        isSetMovePositionCalled = false;
    }

    private int2? LookForDestination()
    {
        Vector2 candidatePoint;

        if (restrictToArea == true)
        {
            int2 areaStart = new int2(areaCentre.x - wanderRadius, areaCentre.y - wanderRadius);
            int2 areaEnd = new int2(areaCentre.x + wanderRadius, areaCentre.y + wanderRadius);

            candidatePoint = new Vector2(UnityEngine.Random.Range(areaStart.x, areaEnd.x), UnityEngine.Random.Range(areaStart.y, areaEnd.y));
        }
        else
        {
            float angle = UnityEngine.Random.value * Mathf.PI * 2;
            Vector2 dir = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)); //these 2 lines pick a direction
            candidatePoint = new Vector2(Position().x, Position().y) + dir * UnityEngine.Random.Range(wanderDistance, 2 * wanderDistance);//this line picks a point along that direction to place our point on.
        }

        int2 pos = WorldController.current.RoundPositionToInt(candidatePoint);

        TileGridObj obj = WorldController.current.mainGrid.GetGridObject(pos.x, pos.y);

        if(obj != null && obj.IsWalkable() == true)
        {
            return pos;
        }

        return null;
    }

    private Vector3 Position()
    {
        return this.transform.position;
    }

    public void SetWanderArea(bool restrictToArea, Vector3 areaCentre, int wanderRadius)
    {
        SetWanderArea(restrictToArea, WorldController.current.RoundPositionToInt(areaCentre), wanderRadius);
    }
    public void SetWanderArea(bool restrictToArea, int2 areaCentre, int wanderRadius)
    {
        this.restrictToArea = restrictToArea;
        this.areaCentre = areaCentre;
        this.wanderRadius = wanderRadius;
    }
}

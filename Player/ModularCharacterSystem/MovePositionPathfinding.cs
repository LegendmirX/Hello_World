using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;


public class MovePositionPathfinding : MonoBehaviour, IMovePosition 
{
    private PathData pathData;
    [SerializeField] private Vector3 currentNode;
    private Action onFailedPathCallBack;

    private bool findPathCalled = false;

    #region Timer
    private float stuckTimer = 0f;
    private float stuckTime = 5f;

    private float minStuckTime = 4;
    private float maxStuckTime = 8;

    private float pathReturnTimer = 0f;
    private float pathReturnTime = 3f;
    #endregion

    private void Awake()
    {
        currentNode = Position();
        stuckTime = UnityEngine.Random.Range(minStuckTime, maxStuckTime);
    }

    public void SetMovePosition(Vector3 moveDestination, Action onFailedPath = null)
    {
        WorldController.current.findPathJobsList.Add(SetPathJob(moveDestination));
        findPathCalled = true;
        if(onFailedPath != null)
        {
            onFailedPathCallBack += onFailedPath;
        }
    }

    public void Update()
    {
        if(findPathCalled == true && pathData.Path == null)
        {
            pathReturnTimer += Time.deltaTime;

            if(pathReturnTimer >= pathReturnTime)
            {
                pathReturnTimer = 0f;
                findPathCalled = false;
                onFailedPathCallBack?.Invoke();
                onFailedPathCallBack = null;
            }
        }

        if(pathData.Path != null)
        {
            if(findPathCalled == true)
            {
                pathReturnTimer = 0;
                findPathCalled = false;
            }

            Vector3 moveDirection = (currentNode - Position()).normalized;
            GetComponent<IMoveVelocity>().SetVelocity(moveDirection);

            if (Vector3.Distance(Position(), currentNode) <= 0.1)
            {
                int2 p = RoundPositionToInt(currentNode);

                pathData.Path.Remove(p);

                if(pathData.Path.Count <= 0)
                {
                    pathData.Path = null;
                    GetComponent<IMoveVelocity>().SetVelocity(Vector3.zero);
                }
                else
                {
                    int2 pos = pathData.Path[pathData.Path.Count - 1];
                    currentNode = new Vector3(pos.x, pos.y);
                }
            }
            else
            {
                stuckTimer += Time.deltaTime;
                if(stuckTimer >= stuckTime)
                {
                    stuckTimer = 0;
                    onFailedPathCallBack?.Invoke();
                    onFailedPathCallBack = null;
                    return;
                }
            }
        }
        else if(pathData.Path == null)
        {
            onFailedPathCallBack?.Invoke();
            onFailedPathCallBack = null;
        }
    }

    public void OnPathReceived(object path)
    {
        PathData data = (PathData)path;
        this.pathData = data;

        findPathCalled = false;
        pathReturnTimer = 0f;

        if (this.pathData.Path == null)
        {
            Debug.Log("PathError");
            onFailedPathCallBack?.Invoke();
            onFailedPathCallBack = null;
            return;
        }

        if (this.pathData.Path.Count > 1)
        {
            this.pathData.Path.Remove(this.pathData.Path[this.pathData.Path.Count - 1]);
        }

        int2 pos = this.pathData.Path[this.pathData.Path.Count - 1];
        currentNode = new Vector3(pos.x, pos.y);
    }

    public PathJob SetPathJob(Vector3 destination)
    {
        int2 endPos = new int2(Mathf.RoundToInt(destination.x), Mathf.RoundToInt(destination.y));

        return SetPathJob(endPos);
    }
    public PathJob SetPathJob(int2 destination)
    {
        int2 startPos = new int2(Mathf.RoundToInt(Position().x), Mathf.RoundToInt(Position().y));
        
        return new PathJob(OnPathReceived, startPos, destination);
    }

    public Vector3 Position()
    {
        return this.transform.position;
    }

    public int2 RoundPositionToInt(Vector3 position)
    {
        return new int2(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));
    }
}

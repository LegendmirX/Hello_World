using System.Collections;
using System.Collections.Generic;
using UnityEngine;
  
using Unity.Mathematics;


public class ModularAI : MonoBehaviour 
{    
    [Space]
    [Header("List Modular parts needed")]
    [SerializeField]
    private AIState state;
    private GameObject target;
    private Vector3 targetLastKnownPosition;

    private void Start()
    {
        GetComponent<IChaseState>().SetRange(GetComponent<IAttack>().Range());
    }

    private void Update() 
    {
        switch (state)
        {
            case AIState.Wander:
                GameObject t = GetComponent<IPerceptionCheck>().Check();

                if(t != null)
                {
                    target = t;
                    state = AIState.Chase;
                }
                else
                {
                    GetComponent<IWanderState>().Execute();
                }
                break;
            case AIState.Chase:
                IChaseState chaseState = GetComponent<IChaseState>();

                if(chaseState.IsInRange(target) == true && chaseState.CanSeeTarget(target) == true)
                {
                    GetComponent<IMovePosition>().SetMovePosition(Position());
                    state = AIState.Attack;
                    chaseState.ResetState();
                    return;
                }

                chaseState.Execute(target);

                if(chaseState.CanSeeTarget(target) == false)
                {
                    Debug.Log("CantSeeTarget");
                    GetComponent<IWanderState>().SetDestination(WorldController.current.RoundPositionToInt(targetLastKnownPosition));
                    state = AIState.Wander;
                    chaseState.ResetState();
                    return;
                }
                else
                {
                    targetLastKnownPosition = target.transform.position;
                }
                break;
            case AIState.Attack:
                GetComponent<IAttack>().Attack((target.transform.position - this.transform.position).normalized);
                state = AIState.Chase;
                break;
        }
    }

    Vector3 Position()
    {
        return transform.position;
    }

    public enum AIState
    {
        Wander,
        Chase,
        Attack
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    [SerializeField]
    public float viewRadius;
    [SerializeField]
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obsticalMask;

    public Collider2D[] targetsInViewRadius;
    public List<Transform> visableTargets = new List<Transform>();


    //private void Start()
    //{
    //    StartCoroutine("FindTargetsWithDelay", 0.1f);
    //}

    //IEnumerator FindTargetsWithDelay(float delay)
    //{
    //    while (true)
    //    {
    //        yield return new WaitForSeconds(delay);
    //        FindVisableTargets();
    //    }
    //}

    public List<Transform> FindVisableTargets()
    {
        visableTargets.Clear();
        targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            if(target == this.transform)
            {
                continue;
            }

            Vector3 dirToTarget = (target.position - transform.position).normalized;

            if(Vector3.Angle(transform.forward, dirToTarget) < viewAngle/2)
            {
                float disToTarget = Vector3.Distance(transform.position, target.position);
                Vector2 rayStart = new Vector2(transform.position.x + dirToTarget.x, transform.position.y + dirToTarget.y);
                RaycastHit2D hitInfo = Physics2D.Raycast(rayStart, dirToTarget, disToTarget);

                if(hitInfo == true)
                {
                    Debug.DrawLine(transform.position, hitInfo.point, Color.red, 0.1f);
                    ICharacterSheet characterSheet = hitInfo.transform.GetComponent<ICharacterSheet>();

                    if(characterSheet != null && characterSheet.GetTeam() != this.GetComponent<ICharacterSheet>().GetTeam())
                    {
                        visableTargets.Add(target);
                    }
                }
            }
        }

        return visableTargets;
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if(angleIsGlobal == false)
        {
            angleInDegrees -= transform.eulerAngles.z;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }
}

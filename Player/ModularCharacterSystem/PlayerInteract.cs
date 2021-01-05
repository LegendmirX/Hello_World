using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactDistance = 1.5f;

    private void Update()
    {
        if(WorldController.IsPointerOverUIObject() == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pointerPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 direction = (new Vector3(pointerPos.x, pointerPos.y, Position().z) - Position()).normalized;

            RaycastHit2D[] raycast = Physics2D.RaycastAll(Position(), direction, interactDistance * 1.1f);

            if(raycast.Length > 0)
            {
                foreach(RaycastHit2D hit in raycast)
                {
                    if(hit.transform == this.transform)
                    {
                        continue;
                    }                   

                    if(Vector3.Distance(Position(), hit.point) <= interactDistance)
                    {
                        IInteractable interactTarget = hit.transform.GetComponent<IInteractable>();

                        if(interactTarget != null)
                        {
                            interactTarget.Interact(this.transform);
                        }
                        break;
                    }
                }


            }
        }
    }


    public Vector3 Position()
    {
        return this.transform.position;
    }
}

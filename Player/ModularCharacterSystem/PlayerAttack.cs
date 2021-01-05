using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private void Update()
    {
        if(WorldController.IsPointerOverUIObject() == true)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
            Vector3 attackDirection = (mousePosition - this.transform.position).normalized;
            GetComponent<IAttack>().Attack(attackDirection);
        }
    }
}

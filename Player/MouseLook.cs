using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float mouseSensativity = 100f;
    [SerializeField] private Transform playerBody;

    private float xRotation = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        if(playerBody == null)
        {
            playerBody = this.transform;
        }

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaTime = Time.deltaTime;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensativity * deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensativity * deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up, mouseX);
    }
}

/*
    =================MirLaboratories=================
    Created by: Klein Holland
    Email: kpeholland@gmail.com
    =================================================
*/

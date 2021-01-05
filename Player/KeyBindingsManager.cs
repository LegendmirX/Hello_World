using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBindingsManager : MonoBehaviour
{
    public static KeyBindingsManager i;

    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode Left;
    public KeyCode Right;
    public KeyCode Jump;
    public KeyCode Crouch;
    public KeyCode OpenInventory;

    public void SetUp()
    {
        i = this;
    }
}

/*
    =================MirLaboratories=================
    Created by: Klein Holland
    Email: kpeholland@gmail.com
    =================================================
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldBootUp : MonoBehaviour 
{
    public enum BootMode
    {
        NewGame,
        LoadGame
    }
    public static BootMode bootMode = BootMode.NewGame;

    List<Action> bootUpList;

    private void Awake()
    {
        bootUpList = new List<Action>();

        switch (bootMode)
        {
            case BootMode.NewGame:
                NewGame();
                break;
            case BootMode.LoadGame:
                NewGame(); //TODO: Impliment Load Game
                break;
        }


        for (int i = 0; i < bootUpList.Count; i++)
        {
            bootUpList[i].Invoke();
        }
        
    }

    void NewGame()
    {
        //PrototypeManager protoManager = FindObjectOfType<PrototypeManager>();
        bootUpList.Add(FindObjectOfType<Clock>().SetUp);
        //bootUpList.Add(FindObjectOfType<ModelManager>().SetUp);
        bootUpList.Add(FindObjectOfType<ThreadedDataRequester>().SetUp);
        bootUpList.Add(FindObjectOfType<UIReferences>().SetUp);
        bootUpList.Add(FindObjectOfType<GameAssets>().SetUp);
        bootUpList.Add(FindObjectOfType<KeyBindingsManager>().SetUp);
        bootUpList.Add(SoundManager.SetUp);
        //List<Action> actions = FindObjectOfType<SpriteManager>().SetUp();
        //foreach (Action action in actions)
        //{
        //    bootUpList.Add(action);
        //}
        bootUpList.Add(FindObjectOfType<WorldController>().SetUp);
    }
}

/*
    =================MirLaboratories=================
    Created by: Klein Holland
    Email: kpeholland@gmail.com
    =================================================
*/
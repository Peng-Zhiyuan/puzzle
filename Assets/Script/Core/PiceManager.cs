using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PiceManager
{
    public static Pice prefab_pice;
    public static List<Pice> list = new List<Pice>();
    public static void Init()
    {
        prefab_pice = Resources.Load<Pice>("Core/Pice");
    }
    public static Pice Create()
    {
        var instance = GameObject.Instantiate<Pice>(prefab_pice);
        list.Add(instance);
        return instance;
    }

    public static void Clean()
    {
        foreach(var pice in list)
        {
            GameObject.Destroy(pice.gameObject);
        }
        list.Clear();
    }
}
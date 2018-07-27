using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class PiceManager
{
    public static Pice prefab_pice;
    public static List<Pice> list = new List<Pice>();
    public static Dictionary<int, Pice> indexToPice = new Dictionary<int, Pice>();
    public static void Init()
    {
        prefab_pice = Resources.Load<Pice>("Core/Pice");
    }
    public static Pice Create(Map map, int index)
    {
        var instance = GameObject.Instantiate<Pice>(prefab_pice);
        instance.Init(map, index);
        list.Add(instance);
        indexToPice[index] = instance;
        return instance;
    }

    public static void Clean()
    {
        foreach(var pice in list)
        {
            GameObject.Destroy(pice.gameObject);
        }
        list.Clear();
        indexToPice.Clear();
    }

    public static Pice GetByIndex(int index)
    {
        return indexToPice[index];
    }
}
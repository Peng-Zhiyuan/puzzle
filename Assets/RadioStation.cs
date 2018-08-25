using System.Collections.Generic;
using System.Collections;
using System;

public class RadioStation
{
    static Dictionary<string, List<Action>> dic = new Dictionary<string, List<Action>>();
    public static void Brodcast(string msg)
    {
        List<Action> list;
        dic.TryGetValue(msg, out list);
        if(list != null)
        {
            foreach(var act in list)
            {
                act.Invoke();
            }
            list.Clear();
        }
    }

    public static void RegisterOnce(string msg, Action act)
    {
        List<Action> list;
        dic.TryGetValue(msg, out list);
        if(list == null)
        {
            list = new List<Action>();
            dic[msg] = list;
        }
        list.Add(act);
    } 

    public static IEnumerator WaiteMsgAsync(string msg)
    {
        var got = false;
        RegisterOnce(msg, ()=>{
            got = true;
        });
        while(!got)
        {
            yield return null;
        }
    }

}
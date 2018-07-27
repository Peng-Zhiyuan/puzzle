using System.Collections.Generic;

public static class DicPool
{
    static List<Dictionary<Pice, bool>> list = new List<Dictionary<Pice, bool>>();

    public static Dictionary<Pice, bool> Take()
    {
        if(list.Count > 0)
        {
            var top = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return top;
        }
        return new Dictionary<Pice, bool>(); 
    }

    public static void Put(Dictionary<Pice, bool> dic)
    {
        dic.Clear();
        list.Add(dic);
    }
}
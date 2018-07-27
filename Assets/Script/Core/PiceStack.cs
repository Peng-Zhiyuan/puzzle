using System.Collections.Generic;

public class PiceStack
{
    public List<Pice> list = new List<Pice>();

    public void Remove(Pice pice)
    {
        list.Remove(pice);
    }

    public Pice Peek()
    {
        return list[list.Count - 1];
    }

    public void Push(Pice pice)
    {
        list.Add(pice);
    }

    public bool Contains(Pice pice)
    {
        return list.Contains(pice);
    }

    public int Count
    {
        get
        {
            return list.Count;
        } 
    }

}
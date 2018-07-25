public static class LayerOrderDispatcher
{
    static int next = 1;

    public static void Init()
    {
        next = 1;
    }

    public static int Next
    {
        get
        {
            return next++;
        }
    }
}
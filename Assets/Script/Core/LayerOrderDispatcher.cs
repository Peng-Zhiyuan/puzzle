public static class LayerOrderDispatcher
{
    public static int next = 10;

    public static void Clean()
    {
        next = 10;
    }

    public static int Next
    {
        get
        {
            return next++;
        }
    }
}
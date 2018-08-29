public static class GameInfo
{
    public static bool? _forceDeveloper;
    public static bool ForceDeveloper
    {
        get
        {
            if(_forceDeveloper == null)
            {
                _forceDeveloper = bool.Parse(GameManifestFinal.Get("force-developer", "false"));
            }
            return _forceDeveloper.Value;
        }
    }


    public static bool ForceRemoveAd
    {
        get
        {
            var b = GameManifestFinal.Get("force-remove-ad", "false");
            if(b == "true")
            {
                return true;
            }
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CustomLitJson;

public static class Log
{
    public static void Scrren(string msg, bool show = false)
    {
        if(GameInfo.ForceDeveloper)
		{
			if(CommandLineFloating.instance != null)
			{
				CommandLineFloating.instance.Log(msg);
				if(show)
				{
					CommandLineFloating.instance.Show();
				}
			}
		}
    }
}
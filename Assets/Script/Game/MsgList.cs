using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MsgList 
{
	public static string Get(string key)
	{
		var language = StaticDataLite.GetCell<string>("msglist", key, "cn");
		if(language == null)
		{
			return key;
		}
		return language;
	}
}

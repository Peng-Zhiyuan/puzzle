using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PicLibrary  
{
	public static Texture2D Load(string file)
	{
		var texture = Resources.Load<Texture2D>("pic-library/" + file);
		return texture;
	}
	
	public static string FindFirstFileNameOfType(string findType)
	{
		var sheet = StaticDataLite.GetSheet("pic");
		foreach(string id in sheet.Keys)
		{
			var row = sheet[id];
			var type = row.Get<string>("type");
			if(findType == type)
			{
				return row.Get<string>("file");
			}
		}
		return "";
	}
}

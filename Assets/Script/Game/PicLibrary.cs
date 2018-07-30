using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomLitJson;

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

	public static JsonData LoadDataRow(int id)
	{
		return StaticDataLite.GetRow("pic", id.ToString());
	}

	public static Texture2D LoadPicById(int id)
	{
		var row = LoadDataRow(id);
		var fileName = row.Get<string>("file");
		var pic = PicLibrary.Load(fileName);
		return pic;
	}
	
}

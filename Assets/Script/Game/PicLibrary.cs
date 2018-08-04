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


	//static Dictionary<string, Sprite> spriteCache = new Dictionary<string, Sprite>();
	public static Sprite LoadContentSprite(string file)
	{
		// Sprite s;
		// spriteCache.TryGetValue(file, out s);
		// if(s != null)
		// {
		// 	return s;
		// }
		var texture = Load(file);
		var content = Sprite.Create(texture, new Rect(0, 106, 512, 300), new Vector2(0.5f, 0.5f), texture.width/1000f);
		// spriteCache[file] = content;
		return content;
	}
	
	public static JsonData FindFirstRowOfType(string findType)
	{
		var sheet = StaticDataLite.GetSheet("pic");
		foreach(string id in sheet.Keys)
		{
			var row = sheet[id];
			var type = row.Get<string>("type");
			if(findType == type)
			{
				return row;
			}
		}
		return null;
	}

	public static JsonData LoadDataRow(int id)
	{
		return StaticDataLite.GetRow("pic", id.ToString());
	}

	public static Sprite LoadContentSpriteById(int id)
	{
		var row = LoadDataRow(id);
		var fileName = row.Get<string>("file");
		var sprite = PicLibrary.LoadContentSprite(fileName);
		return sprite;
	}
	
}

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
	

}

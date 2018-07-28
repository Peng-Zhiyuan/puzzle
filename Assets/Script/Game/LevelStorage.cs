using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStorage 
{
	public static bool IsPictureUnlocked(string id)
	{
		var defaultUnlocked = StaticDataLite.GetCell<bool>("pic", id, "unlock");
		var unlock = PlayerPrefs.GetInt("levelstorage.unlock." + id, 0);
		return defaultUnlocked || unlock == 1;
	}

	public static void SetPictureUnlocked(string id)
	{
		PlayerPrefs.SetInt("levelstorage.unlock." + id, 1);
	}
	
}

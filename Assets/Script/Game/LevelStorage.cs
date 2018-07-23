using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LevelStorage 
{
	public static bool IsPictureUnlocked(string id)
	{
		var defaultUnlocked = StaticDataLite.GetCell<bool>("pic", id, "unlocked");
		var unlock = PlayerPrefs.GetInt("levelstorage.unlock." + id, 0);
		return defaultUnlocked || unlock == 1;
	}

	public static void SetPictureUnlocked(string id)
	{
		PlayerPrefs.SetInt("levelstorage.unlock." + id, 1);
	}

		public static bool IsPictureComplete(string id)
	{
		var defaultUnlocked = StaticDataLite.GetCell<bool>("pic", id, "unlocked");
		var unlock = PlayerPrefs.GetInt("levelstorage.complete." + id, 0);
		return defaultUnlocked || unlock == 1;
	}

	public static void SetPictureComplete(string id)
	{
		PlayerPrefs.SetInt("levelstorage.complete." + id, 1);
	}
}

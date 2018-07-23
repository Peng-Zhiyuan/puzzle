using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStorage 
{
	public static int Gold
	{
		get
		{
			int gold = PlayerPrefs.GetInt("gamestorage.gold", 0);
			return gold;
		}
		set
		{
			PlayerPrefs.SetInt("gamestorage.gold", value);
		}
	}

	public static int star
	{
		get
		{
			int star = PlayerPrefs.GetInt("gamestorage.star", 0);
			return star;
		}
		set
		{
			PlayerPrefs.SetInt("gamestorage.star", value);
		}
	}
}

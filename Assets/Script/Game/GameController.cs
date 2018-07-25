using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameController  
{
	static int lastPicId;

	public static void EnterCore(int picId)
	{
		// save status
		lastPicId = picId;

		// hide bg and show core page
		UIEngine.HideFlaoting<BackgroundFloating>();
		UIEngine.Forward<CorePage>();

		// load picture which player select
		var picFile = StaticDataLite.GetCell<string>("pic", picId.ToString(), "file");
		var picTexture = PicLibrary.Load(picFile);

		// start core game
		Puzzle.Instance.Init();
		Puzzle.Instance.StartPuzzle(picTexture, 200);

		// when compelte
		Puzzle.Instance.Complete += OnCoreGameCompelte;
	}

	private static void OnCoreGameCompelte()
	{
		UIEngine.Forward<LevelCompletePage>();
	}

}

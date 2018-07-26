using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameController  
{
	static int lastPicId;

	public static void EnterCore(int picId, int sliceId)
	{
		// save status
		lastPicId = picId;

		// hide bg and show core page
		UIEngine.HideFlaoting<BackgroundFloating>();
		UIEngine.Forward<CorePage>();

		// load picture which player select
		var picFile = StaticDataLite.GetCell<string>("pic", picId.ToString(), "file");
		var picTexture = PicLibrary.Load(picFile);

		// load slice info
		var piceSize = StaticDataLite.GetCell<int>("pice_slice", sliceId.ToString(), "cell_size");

		// start core game
		Puzzle.Instance.StartPuzzle(picTexture, piceSize);

		// when compelte
		Puzzle.Instance.Complete += OnCoreGameCompelte;
	}


	private static void OnCoreGameCompelte()
	{
		PlayerStatus.exp += 10;
		PlayerStatus.gold += 10;
		PlayerStatus.Save();

		var admin = new Admission_FadeInNewPage();
		UIEngine.Forward<LevelCompletePage>(null, admin);
	}

}

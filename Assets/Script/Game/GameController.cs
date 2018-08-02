using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class GameController  
{
	public static int lastPicId;
	public static int lastSliceId;

	public static void EnterCore(int picId, int sliceId)
	{
		// save status
		lastPicId = picId;
		lastSliceId = sliceId;
  
		// hide bg and show core page
		UIEngine.HideFlaoting<BackgroundFloating>();
		UIEngine.CleanAdmission();
		UIEngine.Forward<CorePage>();

		// load picture which player select
		var picFile = StaticDataLite.GetCell<string>("pic", picId.ToString(), "file");
		var contentSprite = PicLibrary.LoadContentSprite(picFile);

		// load slice info
		var piceSize = StaticDataLite.GetCell<int>("pice_slice", sliceId.ToString(), "cell_size");

		// test code
		// piceSize = 400;

		// start core game
		Puzzle.Instance.StartPuzzle(contentSprite, piceSize);

		// when compelte
		Puzzle.Instance.Complete += OnCoreGameCompelte;

		//LocalNotification.SendNotification(1, 5000, "Title", "Long message text", new Color32(0xff, 0x44, 0x44, 255));
	}

	public static void EnterWithInfo(CoreInfo info)
	{
		var picId = info.picId;
		var sliceId = info.sliceId;
		EnterCore(picId, sliceId);
		Puzzle.instance.LoadInfo(info.puzzleInfo);
	}


	private static void OnCoreGameCompelte()
	{
		var sliceRow = StaticDataLite.GetRow("pice_slice", lastSliceId.ToString());
		var gold = sliceRow.Get<int>("gold");
		var exp = sliceRow.Get<int>("exp");

		HeadBarFloating.instance.AutoRefresh = false;
		PlayerStatus.exp += 10;
		PlayerStatus.gold += 10;
		PlayerStatus.completeCount ++;
		// 更新记录
		var record = PlayerStatus.GetCompleteInfoOfPicId(lastPicId);
		if(record == null || record.sliceId < lastSliceId)
		{
			var info = new CompleteInfo();
			info.pid = lastPicId;
			info.sliceId = lastSliceId;
			PlayerStatus.completeDic[lastPicId.ToString()] = info;
		}
		// 如果这张图有中途存档，则删除存档
		PlayerStatus.RemoveUncompleteInfoOfPicId(lastPicId);
	
		PlayerStatus.Save();

		LevelCompletePage.goldParam = gold;
		LevelCompletePage.expParam = exp;
	
		var admin = new Admission_FadeInNewPage();
		UIEngine.Forward<LevelCompletePage>(null, admin);

	}

	private static CoreInfo CreateInfo()
	{
		var info = new CoreInfo();
		info.picId = lastPicId;
		info.sliceId = lastSliceId;
		var puzzleInfo = Puzzle.Instance.CreateInfo();
		info.puzzleInfo = puzzleInfo;
		return info;
	}

	public static void SaveUncompletePuzzle()
	{
		var info = CreateInfo();
		PlayerStatus.uncompletePuzzle[lastPicId.ToString()] = info;
		PlayerStatus.Save();
	}

}

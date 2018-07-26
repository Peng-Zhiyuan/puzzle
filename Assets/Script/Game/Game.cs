using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour 
{
	void Start()
	{
		StaticDataLite.Init();
		UIEngine.Init();
		PlayerStatus.Read();
		UIEngine.Forward("MainPage");
		//UIEngine.Forward("LevelSettingsPage");
		var floating = UIEngine.ShowFloating<BackgroundFloating>(null, -10);
		floating.transform.SetAsFirstSibling();
		UIEngine.ShowFloating<HeadBarFloating>();
	}


	public static void TestCore () 
	{
		UnityEngine.Random.InitState(DateTime.UtcNow.Second);
		var texture = Resources.Load<Texture2D>("mapbg");
		Puzzle.Instance.StartPuzzle(texture, 200);
	}

	void Update()
	{
		UpdateManager.Update();
	}

}

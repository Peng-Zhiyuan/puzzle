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
		UIEngine.Forward("MainPage");
		UIEngine.ShowFloating<HeadBarFloating>();
	}


	void TestCore () 
	{
		UnityEngine.Random.InitState(DateTime.UtcNow.Second);
		Puzzle.Instance.Init();
		var texture = Resources.Load<Texture2D>("mapbg");
		Puzzle.Instance.StartPuzzle(texture, 200);
	}

	void Update()
	{
		UpdateManager.Update();
	}

}

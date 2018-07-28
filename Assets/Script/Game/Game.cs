using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour 
{
	public static float heightScale;
	void Start()
	{
		StaticDataLite.Init();
		UIEngine.Init();
		PlayerStatus.Read();
		UIEngine.Forward("MainPage");
		//UIEngine.Forward<LevelCompletePage>();
		var floating = UIEngine.ShowFloating<BackgroundFloating>(null, -10);
		floating.transform.SetAsFirstSibling();
		UIEngine.ShowFloating<HeadBarFloating>();

		// 摄像机渲染区域默认根据高去调整宽
		// 但是这里需要根据宽调整高
		// 因此需要手动设置摄像机的渲染高度，达到宽度固定 1080 效果
		Debug.Log(Screen.width + ", " + Screen.height);
		
		// 
		heightScale = CameraUtil.SetCameraSizeByDecisionRevelutionAndFixAtWidth(1080, 1920);
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

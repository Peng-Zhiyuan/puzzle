using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Game : MonoBehaviour 
{
	public static float heightScale;
	void Start()
	{
		Application.logMessageReceived += OnUnityLog;
		UnityEngine.Random.InitState(DateTime.UtcNow.Second);
		StaticDataLite.Init();
		UIEngine.Init();
		PlayerStatus.Read();
		UIEngine.Forward("LoadingPage", null, new Admission_None());
		//UIEngine.Forward<LevelCompletePage>();
		if(GameInfo.ForceDeveloper)
		{
			var commandline = UIEngine.ShowFloating<CommandLineFloating>(null, UIDepth.Top);
		}
		PushManager.ResetNotification();

		// 摄像机渲染区域默认根据高去调整宽
		// 但是这里需要根据宽调整高
		// 因此需要手动设置摄像机的渲染高度，达到宽度固定 1080 效果

		heightScale = CameraUtil.SetCameraSizeByDecisionRevelutionAndFixAtWidth(1080, 1920);
	}

	void OnUnityLog(string condition, string stackTrace, LogType type)
	{
		if(GameInfo.ForceDeveloper)
		{
			if(type == LogType.Exception)
			{
				if(CommandLineFloating.instance != null)
				{
					CommandLineFloating.instance.Log(condition + "\n" + stackTrace);
					CommandLineFloating.instance.Show();
				}

			}
		}

	}

	void Update()
	{
		UpdateManager.Update();

		if(Input.GetKeyDown(KeyCode.Escape))
		{
			var page = UIEngine.Top;
			if(page is MainPage)
			{
				SDKManager.Exit();
			}
			else
			{
				HeadBarFloating.instance.OnBackButton();
			}
		}
	}

}

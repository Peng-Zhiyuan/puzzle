using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorePage : Page 
{
	public Button button_eye;

	bool isEyeShowding = false;

	public override void OnPush()
	{
		isEyeShowding = false;
		SDKManager.OnEnterCore();
	}

	public override void OnPop()
	{
		SDKManager.OnExitCore();
	}

	public void OnEyeButton()
	{
		isEyeShowding = !isEyeShowding;
		Puzzle.instance.ShowEye(isEyeShowding);
		if(isEyeShowding)
		{
			var image = button_eye.GetComponent<Image>();
			var c = image.color;
			c.a = 0.5f;
			image.color = c;
		}
		else
		{
			var image = button_eye.GetComponent<Image>();
			var c = image.color;
			c.a = 1f;
			image.color = c;
		}
	}
	
}

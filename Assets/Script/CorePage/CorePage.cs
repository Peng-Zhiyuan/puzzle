using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorePage : Page 
{
	Button button_eye;

	bool isEyeShowding = false;

	public override void OnPush()
	{
		isEyeShowding = false;
	}

	public void OnEyeButton()
	{
		isEyeShowding = !isEyeShowding;
		Puzzle.instance.ShowEye(isEyeShowding);
	}
	
}

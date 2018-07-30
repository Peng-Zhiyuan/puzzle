using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdPage : Page
{
	public Action Compelte;

	public void OnGetButton()
	{
		Helper.AddGold(40);
		PlayerPrefs.Save();
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(null, admission);
		AudioManager.PlaySe("button");
		Compelte?.Invoke();
		Compelte = null;
	}

	public void OnCloseButton()
	{
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(null, admission);
		AudioManager.PlaySe("button");
		Compelte?.Invoke();
		Compelte = null;
	}
}

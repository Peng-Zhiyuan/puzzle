using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdPage : Page
{

	public void OnGetButton()
	{
		PlayerStatus.gold += 40;
		PlayerPrefs.Save();
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(null, admission);
	}

	public void OnCloseButton()
	{
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(null, admission);
	}
}

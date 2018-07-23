using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogPage : Page
{
	public Text des;

	public override void OnParamChanged()
	{
		this.des.text = this.param;
	}

	public void OnConfirmButton()
	{
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(DialogResult.Conform, admission);
	}

	public void OnCloseButton()
	{
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(DialogResult.Cancel, admission);
	}
}

public enum DialogResult
{
	Conform,
	Cancel,
}
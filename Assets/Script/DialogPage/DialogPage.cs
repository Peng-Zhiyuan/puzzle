using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogPage : Page
{
	public Text des;
	public Text buttonLabel;

	public Action<DialogResult> Complete;

	public override void OnParamChanged()
	{
		//this.des.text = this.param as string;
		var p = this.param as DialogParam;
		this.des.text = p.des;
		this.buttonLabel.text = p.button;
	}

	public void OnConfirmButton()
	{
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(DialogResult.Conform, admission);
		Complete?.Invoke(DialogResult.Conform);
		Complete = null;
	}

	public void OnCloseButton()
	{
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(DialogResult.Cancel, admission);
		Complete?.Invoke(DialogResult.Cancel);
		Complete = null;
	}
}

public enum DialogResult
{
	Conform,
	Cancel,
}

public class DialogParam
{
	public string button;
	public string des;
}
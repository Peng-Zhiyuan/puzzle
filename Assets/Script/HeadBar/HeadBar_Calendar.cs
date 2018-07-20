using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadBar_Calendar : MonoBehaviour 
{
	public Text text;

	public string Value
	{
		get
		{
			return text.text;
		}
		set
		{
			this.text.text = value;
		}
	}
}

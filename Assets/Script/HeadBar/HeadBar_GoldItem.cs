using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadBar_GoldItem : MonoBehaviour 
{
	public Text text;

	public string Value
	{
		set
		{
			this.text.text = value;
		}
		get
		{
			return this.text.text;
		}
	}
}

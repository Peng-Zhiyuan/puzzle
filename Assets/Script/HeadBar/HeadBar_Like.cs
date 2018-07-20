using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadBar_Like : MonoBehaviour 
{
	public Text text;

	public string Value
	{
		get
		{
			return this.text.text;
		}
		set
		{
			this.text.text = value;
		}
	}

}

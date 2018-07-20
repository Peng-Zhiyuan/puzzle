using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadBar_Star : MonoBehaviour 
{
	public Text text;

	public string value
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

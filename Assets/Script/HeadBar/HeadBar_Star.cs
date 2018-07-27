using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeadBar_Star : MonoBehaviour 
{
	public Text text;

	public RectTransform mask;

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

	public Rect WorldRect()
	{
		return RectTransformUtil.GetWorldRect(this.GetComponent<RectTransform>());
	}

	public float Process
	{
		set
		{
			mask.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 57 * value);
		}
	}
}

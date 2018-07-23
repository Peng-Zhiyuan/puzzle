using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PictruePage_Item : MonoBehaviour 
{
	public PictruePage_ItemData data;
	public Image picture;
	public Image puzzleMask;
	public Transform unlockLayer;
	public Text label;

	public Sprite Picture
	{
		set
		{
			this.picture.sprite = value;
		}
	}

	public bool IsShowPuzzleMask
	{
		set
		{
			this.puzzleMask.gameObject.SetActive(value);
		}
	}

	public bool IsShowUnlockLayer
	{
		set
		{
			this.unlockLayer.gameObject.SetActive(value);
		}
	}

	public string LabelText
	{
		set
		{
			this.label.text = value;
		}
	}

	public void OnUnlockButton()
	{
		SendMessageUpwards("OnItemUnlockButton", this);
	}

	public void OnClicked()
	{
		SendMessageUpwards("OnItemClicked", this);
	}
}

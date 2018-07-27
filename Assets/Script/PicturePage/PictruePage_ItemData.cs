using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomLitJson;

public class PictruePage_ItemData 
{
	public JsonData picRow;
	public PicturePage_ItemStatus status;

}

public enum PicturePage_ItemStatus
{
	Locked,
	Unlocked,
	Complete,
}

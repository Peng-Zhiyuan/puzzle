using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBar_Mail : MonoBehaviour
{
	public GameObject notification;

	public bool IsShowNotification
	{
		get
		{
			return notification.activeSelf;
		}
		set
		{
			this.notification.SetActive(value);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AdPage : Page
{
	public Action Compelte;

	public static AdPageOpenSources sources;

	private async void _async()
	{
		await UIUtil.DialogAsync("40 Coins", "receive");
		Helper.AddGold(40);
		if(sources == AdPageOpenSources.Shop)
		{
			PlayerStatus.lastUseAtGiftTime = TimestampUtil.Now;
		}
		PlayerStatus.Save();	

		//Log.Scrren("AdPage: clicked: " + clicked);
		var admission = new Admission_PopdownOldPage();
		UIEngine.Back(null, admission);
		Compelte?.Invoke();
		Compelte = null;
		
	}

	public void OnGetButton()
	{
		if(SDKManager.IsAdLoaded)
		{
			SDKManager.ShowInterAd(clicked =>{
				if(clicked)
				{
					_async();
					
				}
				else
				{
					var param = new DialogParam();
					param.des = "The video has not finished playing and cannot be rewarded";
					param.button = "confirm";
					var popup = new Admission_PopupNewPage();
					var dialog = UIEngine.Forward<DialogPage>(param, popup);
					dialog.Complete = result =>{
						Log.Scrren("AdPage: clicked: " + clicked);
						UIEngineHelper.WateAdmissionComplete(()=>{
							var admission = new Admission_PopdownOldPage();
							UIEngine.Back(null, admission);
							Compelte?.Invoke();
							Compelte = null;
						});

					};

				}

			});
		}
		else
		{
			var admission = new Admission_PopdownOldPage();
			UIEngine.Back(null, admission);
			Compelte?.Invoke();
			Compelte = null;
		}

		AudioManager.PlaySe("button");

	}

	public void OnCloseButton()
	{
		if(sources == AdPageOpenSources.LevelComplete)
		{
			OnGetButton();
		}
		else
		{
			var admission = new Admission_PopdownOldPage();
			UIEngine.Back(null, admission);
			AudioManager.PlaySe("button");
			Compelte?.Invoke();
			Compelte = null;
		}
	}
}

public enum AdPageOpenSources
{
	Shop,
	LevelComplete,
}
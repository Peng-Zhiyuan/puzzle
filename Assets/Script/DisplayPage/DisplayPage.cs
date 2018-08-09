using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DisplayPage : Page 
{
	public Image image_pic;
	public RectTransform button_layout;

	public int PicId
	{
		get
		{
			return (int)this.param;
		}
	}

	public override void OnPush()
	{
		image_pic.gameObject.SetActive(false);
		button_layout.gameObject.SetActive(false);
		var sprite = PicLibrary.LoadContentSpriteById(PicId);
		image_pic.sprite = sprite;
		CoroutineManager.Create(TimeTaks());
	}

	public IEnumerator TimeTaks()
	{
		{
			image_pic.gameObject.SetActive(true);
			var color = image_pic.color;
			color.a = 0;
			image_pic.color = color;
			image_pic.DOFade(1, 0.5f);
			yield return new WaitForSeconds(0.5f);
		}

		{
			button_layout.gameObject.SetActive(true);
			var cg = GameObjectUtil.TryGetComponent<CanvasGroup>(button_layout.gameObject);
			cg.alpha = 0;
			cg.DOFade(1, 0.5f);
			yield return new WaitForSeconds(0.5f);			
		}
	}

	public void OnBackButton()
	{
		var completeCount = PlayerStatus.completeCount;
		if(completeCount == 2 || completeCount == 3)
		{
			var text = MsgList.Get("comment");
			var admin = new Admission_PopupNewPage();
			var dialog = UIEngine.Forward<DialogPage>(text, admin);
			dialog.Complete = result => {
				if(result == DialogResult.Conform)
				{
					SDKManager.Comment();
				}
				DoBack();
			};
		}
		else if(completeCount >= 4)
		{
			if(SDKManager.IsAdLoaded)
			{
				var admin = new Admission_PopupNewPage();
				var adPage = UIEngine.Forward<AdPage>(null, admin);
				adPage.Compelte = () => {
					DoBack();
				};
			}
			else
			{
				DoBack();
			}
		}
		else
		{
			DoBack();
		}
	}

	private void DoBack()
	{
		Puzzle.Instance.Clean();
		UIEngineHelper.WateAdmissionComplete(()=>{
			UIEngine.BackTo<MainPage>();
			UIEngine.DestroyFromPool("LevelCompletePage");
		});
		
	}

	public void OnShareButton()
	{

	}

	public void OnGiftButton()
	{
        AudioManager.PlaySe("sign-and-shop");
        var addmision = new Admission_PopupNewPage();
        UIEngine.Forward<AdPage>(null, addmision);
	}
}

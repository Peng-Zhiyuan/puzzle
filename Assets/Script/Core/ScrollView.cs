using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
	public Transform content;
	public Transform view;
	public Transform dot;

	void Start()
	{
		ContentBoxCliider.enabled = false;
		ViewBoxCliider.enabled = false;
	}

	#region Interface Implementations
 
  	public GameObject DraggedInstance;
  	Vector3 _startPosition;
	Vector3 _offsetToMouse;
	float _zDistanceToCamera;
	public bool draging;
	float speed;
	float FACTORY = 3f;

	public void OnTouch()
	{
		speed = 0;
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		draging = true;
		speed = 0;
		DraggedInstance = content.gameObject;
		_startPosition = content.transform.position;
		_zDistanceToCamera = Mathf.Abs (_startPosition.z - Camera.main.transform.position.z);

		_offsetToMouse = _startPosition - Camera.main.ScreenToWorldPoint (
			new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
		);
	}

	public void OnDrag (PointerEventData eventData)
	{
		if(Input.touchCount > 1)
			return;

		var tempPosition = Camera.main.ScreenToWorldPoint (
			new Vector3 (Input.mousePosition.x, Input.mousePosition.y, _zDistanceToCamera)
			) + _offsetToMouse;
		content.transform.position = new Vector2(tempPosition.x, content.transform.position.y);
		// check out view
		var viewRect = ViewRect;
		var contentRect = ContentRect;
		if(contentRect.xMin > viewRect.xMin)
		{
			var delta = contentRect.xMin - viewRect.xMin;
			content.transform.position = new Vector2(content.transform.position.x - delta, content.transform.position.y);
		}
		else if(contentRect.xMax < viewRect.xMax)
		{
			var delta = viewRect.xMax - contentRect.xMax;
			content.transform.position = new Vector2(content.transform.position.x + delta, content.transform.position.y);
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{
		draging = false;
		DraggedInstance = null;
		_offsetToMouse = Vector3.zero;
	}

	float lastX;
	void Update()
	{
		if(draging)
		{
			var nowX = content.transform.position.x;
			this.speed = nowX - lastX;
			lastX = nowX;
			return;
		}
		if(speed != 0)
		{
			var p = content.position;
			p.x += speed;
			content.position = p;
			if(speed < 0)
			{
				speed *= (1 - FACTORY * Time.deltaTime);
				if(speed > -0.1f)
				{
					speed = 0;
				}
			}
			else if(speed > 0)
			{
				speed *= (1 - FACTORY * Time.deltaTime);
				if(speed < 0.1f)
				{
					speed = 0;
				}
			}

			if(speed < 0)
			{
				if(ContentRect.xMax < ViewRect.xMax)
				{
					var delta = ViewRect.xMax - ContentRect.xMax;
					content.transform.position = new Vector2(content.transform.position.x + delta, content.transform.position.y);
					speed = 0;
					return;
				}
			}
			else if(speed > 0)
			{
				if(ContentRect.xMin > ViewRect.xMin)
				{
					var delta = ContentRect.xMin - ViewRect.xMin;
					content.transform.position = new Vector2(content.transform.position.x - delta, content.transform.position.y);
					speed = 0;
					return;
				}
			}

			
		}
	}


	public void AnimateFixContentPosition()
	{
		var viewRect = ViewRect;
		var contentRect = ContentRect;
		if(contentRect.xMin > viewRect.xMin && contentRect.xMax > viewRect.xMax)
		{
			var delta = contentRect.xMin - viewRect.xMin;
			var targetPosition = new Vector2(content.transform.position.x - delta, content.transform.position.y);
			//iTween.MoveTo(content.gameObject, targetPosition, 0.2f);
			GameObject.Destroy(content.GetComponent<iTween>());
			iTween.MoveTo(content.gameObject, iTween.Hash("x", targetPosition.x, "y", targetPosition.y, "easeType", iTween.EaseType.easeOutCirc, "time", 0.2f));
		}
		else if(contentRect.xMax < viewRect.xMax && contentRect.xMin < viewRect.xMin)
		{
			GameObject.Destroy(content.GetComponent<iTween>());
			var delta = viewRect.xMax - contentRect.xMax;
			var targetPosition = new Vector2(content.transform.position.x + delta, content.transform.position.y);
			//iTween.MoveTo(content.gameObject, targetPosition, 0.2f);
			iTween.MoveTo(content.gameObject, iTween.Hash("x", targetPosition.x, "y", targetPosition.y, "easeType", iTween.EaseType.easeOutCirc, "time", 0.2f));
		}
		else if(contentRect.xMin >= viewRect.xMin && contentRect.xMax <= viewRect.xMax)
		{
			GameObject.Destroy(content.GetComponent<iTween>());
			var delta = viewRect.xMax - contentRect.xMax;
			var targetPosition = new Vector2(content.transform.position.x + delta, content.transform.position.y);
			//iTween.MoveTo(content.gameObject, targetPosition, 0.2f);
			iTween.MoveTo(content.gameObject, iTween.Hash("x", targetPosition.x, "y", targetPosition.y, "easeType", iTween.EaseType.easeOutCirc, "time", 0.2f));
		}
	}


	#endregion

	BoxCollider2D contentBoxCollider = null;

	public BoxCollider2D ContentBoxCliider
	{
		get
		{
			if(contentBoxCollider == null)
			{
				contentBoxCollider = content.GetComponent<BoxCollider2D>();
			}
			return contentBoxCollider;
		}
	}

	BoxCollider2D viewBoxCollider = null;

	public BoxCollider2D ViewBoxCliider
	{
		get
		{
			if(viewBoxCollider == null)
			{
				viewBoxCollider = view.GetComponent<BoxCollider2D>();
			}
			return viewBoxCollider;
		}
	}

	public void SetContentLength(int length)
	{
		var c = ContentBoxCliider;
		c.size = new Vector2(length, c.size.y);
		c.offset = new Vector2(length/2, 0);
		this.dot.localPosition = new Vector2(length, 0);
	}

	public Rect ContentRect
	{
		get
		{
			var left = ContentBoxCliider.offset.x - ContentBoxCliider.size.x/2;
			var width = ContentBoxCliider.size.x;
			var bottom = ContentBoxCliider.offset.y - ContentBoxCliider.size.y/2;
			var height =  ContentBoxCliider.size.y;
			left += this.content.transform.position.x;
			bottom += this.content.transform.position.y;
			return new Rect(left, bottom, width, height);
		}
	}

	public Rect ViewRect
	{
		get
		{
			var left = ViewBoxCliider.offset.x - ViewBoxCliider.size.x/2;
			var width = ViewBoxCliider.size.x;
			var bottom = ViewBoxCliider.offset.y - ViewBoxCliider.size.y/2;
			var height =  ViewBoxCliider.size.y;
			left += this.view.transform.position.x;
			bottom += this.view.transform.position.y;
			return new Rect(left, bottom, width, height);
		}
	}
}

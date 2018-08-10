using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class VirtualScrollView : MonoBehaviour
{
	public float cellHeight = 100;
	public float cellWidth = 100;

	public int depth;
	public string elementName;
	public Action<RectTransform, object> onSetControl;

	public RectTransform content;
	public RectTransform sample;

	public RectTransform view;

	public float offsetY;
	public float extraY;

	private Queue<RectTransform> queue = new Queue<RectTransform>();
	private RectTransform head;
	private RectTransform tail;
	public List<RectTransform> activeElementList = new List<RectTransform>();
	private List<object> dataList = new List<object>();


	private float ViewTop
	{
		get
		{
			var viewY = - this.content.localPosition.y;
			var top = viewY + (1 - this.view.pivot.y) * this.view.rect.height;
			return top;
		}
	}

	private float ContentTop
	{
		get
		{
			return (1 - this.content.pivot.y) * this.content.rect.height;
		}
	}

	private float ContentLeft
	{
		get
		{
			return - (this.content.pivot.x * this.content.rect.width);
		}
	}

	private float ContentRight
	{
		get
		{
			return (1 - this.content.pivot.x) * this.content.rect.width;
		}
	}

	private float ContentBotom
	{
		get
		{
			return -(this.content.pivot.y * this.content.rect.height);
		}
	}

	private float ViewBottom
	{
		get
		{
			var viewY = - this.content.localPosition.y;
			var bottom = viewY - this.view.pivot.y * this.view.rect.height;
			return bottom;
		}
	}

	private float ContentCenterX
	{
		get
		{
			return (0.5f - this.content.pivot.x) * this.content.rect.width;
		}
	}
    private int rowCount
	{
		get
		{
			return (int)(this.content.rect.width / this.cellWidth);
		}
    }

    private int getIndexX(int index)
	{
        return index % this.rowCount;
    }

	private int getIndexY(int index)
	{
        return index / this.rowCount;
    }

	private RectTransform CreateControl()
	{
		if(this.queue.Count == 0)
		{
			var item = GameObject.Instantiate(this.sample);
			item.parent = this.content;
			item.gameObject.SetActive(true);
			return item;
		}
		else
		{
			var item = this.queue.Dequeue();
			item.gameObject.SetActive(true);
			return item;
		}
	}

	Dictionary<RectTransform, ItemTag> tagDic = new Dictionary<RectTransform, ItemTag>();
	private ItemTag GetTag(RectTransform item)
	{
		ItemTag tag;
		tagDic.TryGetValue(item, out tag);
		if(tag != null)
		{
			return tag;
		}
		tag = new ItemTag();
		tagDic[item] = tag;
		return tag;
	}


	private RectTransform CreateControlAt(int index)
	{
		var item = this.CreateControl();
		item.localPosition = this.getControlPosition(index);
		GetTag(item).listIndex = index;
		var data = this.dataList[index];
		this.onSetControl?.Invoke(item, data);
		return item;
	}

	private Vector2 getControlPosition(int index)
	{
		var indexX = this.getIndexX(index);
		var indexY = this.getIndexY(index);
		var y = this.head.localPosition.y - indexY * this.cellHeight;
		var x = this.head.localPosition.x + indexX * this.cellWidth;
		return new Vector2(x, y);
	}

	public float Movement
	{
		get
		{
			return this.head.localPosition.y - (this.ViewTop - this.cellHeight / 2) + this.offsetY;
		}
	}

	private void DestroyAllActiveControl()
	{
		for(var i = this.activeElementList.Count - 1; i >= 0; i--)
		{
			var item = this.activeElementList[i];
			this.activeElementList.RemoveAt(i);
			this.DestroyControl(item);
		}
	}

	private void DestroyControl(RectTransform control)
	{
        control.gameObject.SetActive(false);
        this.queue.Enqueue(control);
    }

	private bool IsInited
	{
		get
		{
			return this.head != null;
		}
	}

	public RectTransform FindControl(object data)
	{
		for(var i = 0; i < this.activeElementList.Count; i++)
		{
			var element = this.activeElementList[i];
			var nowIndex = GetTag(element).listIndex;
			var nowData = this.dataList[nowIndex];
			if(nowData == data)
			{
				return element;
			}
		}
		return null;
	}

	private void CreateHeadTail() 
	{
        var headY = this.ContentTop - this.cellHeight / 2 - this.offsetY;
        var headX = this.ContentLeft + this.cellWidth / 2;
        {
            var head = new RectTransform();
            head.parent = this.content;
			head.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.cellWidth);
			head.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.cellHeight);
            head.localPosition = new Vector2(headX, headY);
            head.name = "head";
            this.head = head;
        }
        {
            var tail = new RectTransform();
            tail.parent = this.content;
			tail.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this.cellWidth);
			tail.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this.cellHeight);
            tail.localPosition = this.head.localPosition;
            tail.name = "tail";
            this.tail = tail;
        }
    }

	private void RepositionTail() 
	{
        this.tail.localPosition = this.head.localPosition;
        var indexX = (this.dataList.Count - 1) % this.rowCount;
        var indexY = (this.dataList.Count - 1) / this.rowCount;
        var shiftX = indexX * this.cellHeight;
        var shiftY = indexY * this.cellHeight;
        if (shiftX < 0) shiftX = 0;
        if (shiftY < 0) shiftY = 0;
        var position = this.tail.localPosition;
        this.tail.localPosition = new Vector2(position.x + shiftX, position.y - shiftY);
		var contentHeight = shiftY + this.cellHeight + this.offsetY + this.extraY;
		this.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, contentHeight);
    }

    private void MoveToTail(bool refresh = true) 
	{
        var delta = this.ViewBottom - this.ContentBotom;
        this.Move(-delta, refresh);
    }

    private void MoveToHead(bool refresh = true) {
        var delta = this.ContentTop - this.ViewTop;
        this.Move(delta, refresh);
    }

    private void Move(float deltaY, bool refresh = true) 
	{
        var p = this.content.localPosition;
        p.y = p.y - deltaY;
        this.content.localPosition = p;
		if(refresh)
		{
			this.RecalculateVisibleControl();
		}
    }

    private bool IsIndexValidate(int index) 
	{
        if (index < 0 || index >= this.dataList.Count) 
		{
            return false;
        }
        return true;
    }

    private bool IsControlVisible(int index) 
	{
        if (!this.IsIndexValidate(index)) {
            return false;
        }
        var indexY = this.getIndexY(index);
        var y = this.head.localPosition.y - indexY * this.cellHeight;
        var top = y + this.cellHeight / 2;
        var bottom = y - this.cellHeight / 2;
        if (bottom > this.ViewTop || top < this.ViewBottom) 
		{
            return false;
        }
        return true;
    }
	private void CalculateFirstActiveControlIndex(out int firstIndex, out float firstY)
	{
        var headTop = this.head.position.y + this.cellHeight / 2;
        var topDistance = headTop - this.ViewTop;
        var indexY = (int)(topDistance / this.cellHeight);
        var index = indexY * this.rowCount;
        var firstActiveElementShift = topDistance % this.cellHeight;
        var firstActiveElementY = this.ViewTop - this.cellHeight / 2 + firstActiveElementShift;
        firstIndex = index;
        firstY = firstActiveElementY;
    }

	private void PlaceControl() 
	{
        this.DestroyAllActiveControl();
        if (this.dataList.Count == 0) 
		{
            return;
        }
		int index;
		float y;
        this.CalculateFirstActiveControlIndex(out index, out y);
        if (index < 0) index = 0;
        if (!this.IsIndexValidate(index)) 
		{
            return;
        }
        var counter = 0;
        while (this.IsControlVisible(index)) 
		{
            var element = this.CreateControlAt(index);
            this.activeElementList.Add(element);
            index ++;
            if (!this.IsIndexValidate(index)) 
			{
                break;
            }
            counter ++;
            if (counter >= 100) 
			{
                Debug.LogWarning("counter more than 100, something is wrong!");
                break;
            }
        }
    }

	private void RecalculateVisibleControl() 
	{
        if (!this.IsInited) 
		{
            return;
        }
        // 去掉出界控件
        for (var i = this.activeElementList.Count - 1; i >= 0; i--) 
		{
            var control = this.activeElementList[i];
            var index = GetTag(control).listIndex;
            var visible = this.IsControlVisible(index);
            if (!visible) {
                this.activeElementList.RemoveAt(i);
                this.DestroyControl(control);
            }
        }
        if (this.activeElementList.Count > 0) {
            // 尝试从两端创建新控件
            var firstActiveControl = this.activeElementList[0];
            while (true) 
			{
                var preIndex = GetTag(firstActiveControl).listIndex - 1;
                if (this.IsControlVisible(preIndex)) 
				{
                    var newControl = this.CreateControlAt(preIndex);
                    this.activeElementList.Insert(0, newControl);
                    firstActiveControl = newControl;
                    continue;
                }
                break;
            }
            // tail
            var lastActiveControl = this.activeElementList[this.activeElementList.Count - 1];
            while (true) 
			{
                var nextIndex = GetTag(lastActiveControl).listIndex + 1;
                if (this.IsControlVisible(nextIndex)) {
                    var newControl = this.CreateControlAt(nextIndex);
                    this.activeElementList.Add(newControl);
                    lastActiveControl = newControl;
                    continue;
                }
                break;
            }
        }
        else 
		{
            this.PlaceControl();
        }
    }

	void Update()
	{
		this.RecalculateVisibleControl();
	}

	public void ChangeData(object[] dataList, VirtaulGridScrollViewOption option = VirtaulGridScrollViewOption.None)
	{
		if(!this.IsInited)
		{
			this.CreateHeadTail();
		}
		this.dataList.Clear();
		this.dataList.AddRange(dataList);
		this.RepositionTail();
		switch(option)
		{
			case VirtaulGridScrollViewOption.None:
				break;
			case VirtaulGridScrollViewOption.MoveToHead:
				this.MoveToHead(false);
				break;
			case VirtaulGridScrollViewOption.MoveToTail:
				this.MoveToTail(false);
				break;
		}
		this.PlaceControl();
	}

	class ItemTag
	{
		public int listIndex = -1;
	}
}

public enum VirtaulGridScrollViewOption {
    None,
    MoveToHead,
    MoveToTail,
}
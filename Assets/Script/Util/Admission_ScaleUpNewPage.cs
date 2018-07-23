﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Admission_ScaleUpNewPage : Admission
{
    Rect rect;
    float DURATION = 0.2f;

    public Admission_ScaleUpNewPage(Rect rect)
    {
        this.rect = rect;
    } 

    public override void Play(Page oldPage, Page newPage)
    {
        oldPage.Active = true;
        newPage.Active = true;
        var canvas = UIEngine.Canvas.GetComponent<RectTransform>();
        var canvasWidth = canvas.rect.width;
        var canvasHeight = canvas.rect.height;
        // newPage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, rect.xMin, rect.size.x);
        // newPage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, rect.yMin,rect.size.y);
        // var targetPosition = newPage.transform.position;
        var targetPosition = new Vector2(canvasWidth/2, canvasHeight/2);
        Debug.Log(targetPosition);
        var fromPosition = this.rect.center;
        newPage.transform.position = fromPosition;
        newPage.rectTransform.DOMove(targetPosition, DURATION);
        

        var fromScaleX = this.rect.width / canvasWidth;
        var fromScaleY = this.rect.height / canvasHeight;
        newPage.rectTransform.localScale = new Vector2(fromScaleX, fromScaleY);
        newPage.rectTransform.DOScale(Vector2.one, DURATION);
        var cg = TryGetComponent<CanvasGroup>(newPage.gameObject);
        cg.alpha = DURATION;
        cg.DOFade(1, DURATION).OnComplete(()=>{
            this.finished = true;
            oldPage.Active = false;
        });

    }

    static T TryGetComponent<T>(GameObject a) where T: Component
    {
        var c = a.GetComponent<T>();
        if(c != null)
        {
            return c;
        }
        return a.AddComponent<T>();
    }
}

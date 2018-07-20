using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Admission_ScaleUpNewPage : Admission
{
    Rect rect;
    float DURATION = 0.3f;

    public Admission_ScaleUpNewPage(Rect rect)
    {
        this.rect = rect;
    } 

    public override void Play(Page oldPage, Page newPage)
    {
        oldPage.Active = true;
        newPage.Active = true;
        // newPage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, rect.xMin, rect.size.x);
        // newPage.rectTransform.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, rect.yMin,rect.size.y);
        var targetPosition = newPage.transform.position;
        var fromPosition = this.rect.center;
        newPage.transform.position = this.rect.center;
        newPage.rectTransform.DOMove(targetPosition, DURATION);
        var canvas = UIEngine.Canvas.GetComponent<RectTransform>();
        var canvasWidth = canvas.rect.width;
        var canvasHeight = canvas.rect.height;
        var fromScaleX = this.rect.width / canvasWidth;
        var fromScaleY = this.rect.height / canvasHeight;
        newPage.rectTransform.localScale = new Vector2(fromScaleX, fromScaleY);
        newPage.rectTransform.DOScale(Vector2.one, DURATION);
        var cg = newPage.gameObject.AddComponent<CanvasGroup>();
        cg.alpha = DURATION;
        cg.DOFade(1, DURATION).OnComplete(()=>{
            this.finished = true;
        });
    }
}

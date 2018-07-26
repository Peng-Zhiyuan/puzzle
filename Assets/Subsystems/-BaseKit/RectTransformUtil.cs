using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RectTransformUtil
{
    static public Rect GetWorldRect (RectTransform rt) {
         // Convert the rectangle to world corners and grab the top left
         Vector3[] corners = new Vector3[4];
         rt.GetWorldCorners(corners);
         Vector3 topLeft = corners[0];
 
         // Rescale the size appropriately based on the current Canvas scale
         //Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);
         Vector2 scaledSize = new Vector2(rt.rect.size.x, rt.rect.size.y);

         return new Rect(topLeft, scaledSize);
    }
}
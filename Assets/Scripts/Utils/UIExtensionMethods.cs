using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class UIExtensionMethods
{
    public static Vector2 GetScreenPosition(this RectTransform rt)
    {
        if (rt == null || rt.parent == null) return Vector2.zero;

        RectTransform parentRt = rt.parent as RectTransform;

        Vector2 parentPos = parentRt.GetScreenPosition();
        Vector2 parentSize = parentRt.GetSize();

        return new Vector2(parentSize.x * rt.anchorMin.x + parentPos.x + (rt.offsetMin.x - rt.sizeDelta.x * (rt.localScale.x - 1) * rt.pivot.x) * rt.lossyScale.x / rt.localScale.x,
                           parentSize.y * rt.anchorMin.y + parentPos.y + (rt.offsetMin.y - rt.sizeDelta.y * (rt.localScale.y - 1) * rt.pivot.y) * rt.lossyScale.y / rt.localScale.y);
    }

    #region GetSize

    public static Vector2 GetSize(this RectTransform rt) { return rt.GetSize(rt.GetLocalSize()); }
    public static Vector2 GetSize(this RectTransform rt, Vector2 localSize)
    {
        return new Vector2(localSize.x * rt.localScale.x, localSize.y * rt.localScale.y);
    }

    public static Vector2 GetLocalSize(this RectTransform rt, Vector2 size)
    {
        return new Vector2(size.x / rt.localScale.x, size.y / rt.localScale.y);
    }

    public static Vector2 GetLocalSize(this RectTransform rt)
    {
        return rt.rect.size;
    }

    public static Vector2 GetSizeWithTextCorrection(this RectTransform rt)
    {
        Text text = rt.GetComponent<Text>();
        if (text == null) return rt.GetSize();
        return new Vector2(rt.GetSize().x, text.preferredHeight);
    }

    #endregion

    #region SetSize

    public static void SetLocalSize(this RectTransform rectTransform, Vector2 localSize) {
        rectTransform.SetLocalSize(localSize, true, true);
    }

    public static void SetLocalSize(this RectTransform rectTransform, Vector2 localSize, bool growRight, bool growTop) {
        rectTransform.SetSize(rectTransform.GetSize(localSize), growRight, growTop);
    }

    public static void SetSize(this RectTransform rectTransform, Vector2 size) {
        rectTransform.SetSize(size, true, true);
    }

    public static void SetSize(this RectTransform rectTransform, Vector2 size, bool growRight, bool growTop)
    {
        Vector2 currentLocalSize = rectTransform.GetLocalSize();
        Vector2 currentSize = rectTransform.GetSize(currentLocalSize);

        Vector2 sizeDelta = size - currentSize;

        Vector2 scaledOffsetMin = rectTransform.GetScaledOffsetMin(currentLocalSize);
        Vector2 scaledOffsetMax = rectTransform.GetScaledOffsetMax(currentLocalSize);

        Vector2 newScaledOffsetMin = new Vector2((growRight ? 0 : -sizeDelta.x) + scaledOffsetMin.x,
                                                 (growTop   ? 0 : -sizeDelta.y) + scaledOffsetMin.y);

        Vector2 newScaledOffsetMax = new Vector2((growRight ? sizeDelta.x : 0) + scaledOffsetMax.x,
                                                 (growTop   ? sizeDelta.y : 0) + scaledOffsetMax.y);

        rectTransform.SetScaledOffsets(newScaledOffsetMin, newScaledOffsetMax);
    }

    public static void SetGlobalSizeWithCurrentAnchors(this RectTransform rectTransform, Vector2 size)
    {
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x / rectTransform.localScale.x);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,   size.y / rectTransform.localScale.y);
    }

    #endregion

    #region GetPosition

    public static Vector2 GetPosition(this RectTransform rectTransform) {
        return rectTransform.GetPosition(true, true);
    }

    public static Vector2 GetPosition(this RectTransform rectTransform, bool fromLeft, bool fromBottom) {
        Vector2 parentLocalSize = (rectTransform.parent as RectTransform).GetLocalSize();
        Vector2 childLocalSize = rectTransform.GetLocalSize();

        return rectTransform.GetPosition(fromLeft, fromBottom, childLocalSize, parentLocalSize);
    }

    public static Vector2 GetPosition(this RectTransform rectTransform, bool fromLeft, bool fromBottom, Vector2 childLocalSize, Vector2 parentLocalSize)
    {
        Vector2 scaledOffsetMin = rectTransform.GetScaledOffsetMin(childLocalSize);
        Vector2 scaledOffsetMax = rectTransform.GetScaledOffsetMax(childLocalSize);

        Vector2 positionBottomLeft = new Vector2(parentLocalSize.x * rectTransform.anchorMin.x + scaledOffsetMin.x,
                                                 parentLocalSize.y * rectTransform.anchorMin.y + scaledOffsetMin.y);

        Vector2 positionTopRight = new Vector2(-(parentLocalSize.x * (rectTransform.anchorMax.x - 1) + scaledOffsetMax.x),
                                               -(parentLocalSize.y * (rectTransform.anchorMax.y - 1) + scaledOffsetMax.y));

        return new Vector2(fromLeft   ? positionBottomLeft.x : positionTopRight.x,
                           fromBottom ? positionBottomLeft.y : positionTopRight.y);
    }

    #endregion

    #region SetPosition

    public static void SetPosition(this RectTransform rectTransform, Vector2 position) {
        rectTransform.SetPosition(position, true, true);
    }

    public static void SetPosition(this RectTransform rectTransform, Vector2 position, Vector2 localSize) {
        rectTransform.SetPosition(position, true, true, localSize);
    }

    public static void SetPosition(this RectTransform rectTransform, Vector2 position, bool fromLeft, bool fromBottom) {
        rectTransform.SetPosition(position, fromLeft, fromBottom, rectTransform.GetLocalSize());
    }

    public static void SetPosition(this RectTransform rectTransform, Vector2 position, bool fromLeft, bool fromBottom, Vector2 localSize)
    {
        RectTransform parentRectTransform = rectTransform.parent as RectTransform;
        Vector2 parentLocalSize = parentRectTransform.GetLocalSize();

        Vector2 scaledOffsetsDelta = rectTransform.GetScaledOffsetMax(localSize) - rectTransform.GetScaledOffsetMin(localSize);

        Vector2 bottomLeftMin = new Vector2(-parentLocalSize.x * rectTransform.anchorMin.x + position.x, position.y - parentLocalSize.y * rectTransform.anchorMin.y);
        Vector2 bottomLeftMax = bottomLeftMin + scaledOffsetsDelta;

        Vector2 topRightMax = new Vector2(-position.x + parentLocalSize.x * (1 - rectTransform.anchorMax.x), -position.y + parentLocalSize.y * (1 - rectTransform.anchorMax.y));
        Vector2 topRightMin = topRightMax - scaledOffsetsDelta;

        rectTransform.SetScaledOffsets(new Vector2(fromLeft ? bottomLeftMin.x : topRightMin.x, fromBottom ? bottomLeftMin.y : topRightMin.y),
                                       new Vector2(fromLeft ? bottomLeftMax.x : topRightMax.x, fromBottom ? bottomLeftMax.y : topRightMax.y),
                                       parentLocalSize);
    }

    #endregion

    #region Offsets

    private static Vector2 OffsetDeltaMin(this RectTransform rectTransform, Vector2 localSize)
    {
        return new Vector2(localSize.x * rectTransform.pivot.x * (1 - rectTransform.localScale.x),
                           localSize.y * rectTransform.pivot.y * (1 - rectTransform.localScale.y));
    }

    private static Vector2 OffsetDeltaMax(this RectTransform rectTransform, Vector2 localSize)
    {
        return new Vector2(-(localSize.x * (1 - rectTransform.pivot.x) * (1 - rectTransform.localScale.x)),
                           -(localSize.y * (1 - rectTransform.pivot.y) * (1 - rectTransform.localScale.y)));
    }

    public static Vector2 GetScaledOffsetMin(this RectTransform rectTransform)
    {
        return rectTransform.GetScaledOffsetMin(rectTransform.GetLocalSize());
    }

    public static Vector2 GetScaledOffsetMin(this RectTransform rectTransform, Vector2 localSize)
    {
        Vector2 offsetDeltaMin = rectTransform.OffsetDeltaMin(localSize);

        return offsetDeltaMin + rectTransform.offsetMin;
    }

    public static Vector2 GetScaledOffsetMax(this RectTransform rectTransform)
    {
        return rectTransform.GetScaledOffsetMax(rectTransform.GetLocalSize());
    }

    public static Vector2 GetScaledOffsetMax(this RectTransform rectTransform, Vector2 localSize)
    {
        Vector2 offsetDeltaMax = rectTransform.OffsetDeltaMax(localSize);

        return offsetDeltaMax + rectTransform.offsetMax;
    }

    public static void SetScaledOffsetMin(this RectTransform rectTransform, Vector2 min)
    {
        rectTransform.SetScaledOffsets(min, rectTransform.GetScaledOffsetMax());
    }

    public static void SetScaledOffsetMax(this RectTransform rectTransform, Vector2 max)
    {
        rectTransform.SetScaledOffsets(rectTransform.GetScaledOffsetMin(), max);
    }

    public static void SetScaledOffsets(this RectTransform rectTransform, Vector2 min, Vector2 max)
    {
        rectTransform.SetScaledOffsets(min, max, (rectTransform.parent as RectTransform).GetLocalSize());
    }

    public static void SetScaledOffsets(this RectTransform rectTransform, Vector2 min, Vector2 max, Vector2 parentLocalSize)
    {
        Vector2 minCorrection = new Vector2((rectTransform.localScale.x - 1) * parentLocalSize.x * (rectTransform.anchorMax.x - rectTransform.anchorMin.x) * rectTransform.pivot.x,
                                            (rectTransform.localScale.y - 1) * parentLocalSize.y * (rectTransform.anchorMax.y - rectTransform.anchorMin.y) * rectTransform.pivot.y);

        Vector2 maxCorrection = new Vector2(-(rectTransform.localScale.x - 1) * parentLocalSize.x * (rectTransform.anchorMax.x - rectTransform.anchorMin.x) * (1 - rectTransform.pivot.x),
                                            -(rectTransform.localScale.y - 1) * parentLocalSize.y * (rectTransform.anchorMax.y - rectTransform.anchorMin.y) * (1 - rectTransform.pivot.y));

        min += minCorrection;
        max += maxCorrection;

        //Scale delta variables, left and right
        Vector2 sc1l = GetOffsetScale(rectTransform, 1, true);
        Vector2 sc0l = GetOffsetScale(rectTransform, 0, true);
        Vector2 sc1r = GetOffsetScale(rectTransform, 1, false);
        Vector2 sc0r = GetOffsetScale(rectTransform, 0, false);

        rectTransform.offsetMin = new Vector2(
            (sc1r.x * min.x + sc0l.x * max.x) / (sc1l.x * sc1r.x - sc0l.x * sc0r.x),
            (sc1r.y * min.y + sc0l.y * max.y) / (sc1l.y * sc1r.y - sc0l.y * sc0r.y)
            );

        rectTransform.offsetMax = new Vector2(
            (sc1l.x * max.x + sc0r.x * min.x) / (sc1l.x * sc1r.x - sc0l.x * sc0r.x),
            (sc1l.y * max.y + sc0r.y * min.y) / (sc1l.y * sc1r.y - sc0l.y * sc0r.y)
            );
    }

    private static Vector2 GetOffsetScale(RectTransform rectTransform, int one, bool left) {
        return new Vector2(one + (rectTransform.localScale.x - 1) * (left ? rectTransform.pivot.x : (1 - rectTransform.pivot.x)),
                           one + (rectTransform.localScale.y - 1) * (left ? rectTransform.pivot.y : (1 - rectTransform.pivot.y)));
    }


    #endregion
}

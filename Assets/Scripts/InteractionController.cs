using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    public Transform handleCenterTransform;
    public RotateKeyboardConstantSpeed rotationExecutor;
    private Vector2 prevMousePos;

    public void OnMouseDown(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        prevMousePos = pointerEventData.pressPosition;
        rotationExecutor.StartRotation();
    }

    public void OnMouseUp(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        var releasePos = pointerEventData.position;
        rotationExecutor.StopExternalRotation();
    }

    public void OnMouseDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        var curPointerPos = pointerEventData.position;
        var handleCenterScreen = (Vector2)Camera.main.WorldToScreenPoint(handleCenterTransform.position);
        var curVec = curPointerPos - handleCenterScreen;
        var prevVec = prevMousePos - handleCenterScreen;
        var relativeAngle = Vector2.SignedAngle(prevVec, curVec);

        rotationExecutor.RotateDeltaExternal(relativeAngle);
        prevMousePos = curPointerPos;
    }
}

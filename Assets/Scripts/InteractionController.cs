﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    public Transform handleCenterTransform;
    public RotateKeyboardConstantSpeed rotationExecutor;

    
    private Vector2 prevMousePos;

    public float PreviousAngle
    {
        get { return rotationExecutor.previousAngle; }
        set { rotationExecutor.previousAngle = value; }
    }

    public void OnMouseDown(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        prevMousePos = pointerEventData.pressPosition;
        PreviousAngle = RotationTarget.Angle;
    }

    public void OnMouseUp(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        var releasePos = pointerEventData.position;
        rotationExecutor.RotateToClosest();
    }

    public void OnMouseDrag(BaseEventData eventData)
    {
        rotationExecutor.StartRotation();

        var pointerEventData = eventData as PointerEventData;
        var curPointerPos = pointerEventData.position;

        var handleCenterScreen = (Vector2)Camera.main.WorldToScreenPoint(handleCenterTransform.position);
        var curVec = curPointerPos - handleCenterScreen;
        var prevVec = prevMousePos - handleCenterScreen;

        var relativeAngle = Vector2.SignedAngle(prevVec, curVec);

        var curAngle = PreviousAngle + relativeAngle;

        RotationTarget.Angle = curAngle;
        rotationExecutor.RotateDelta(curAngle - PreviousAngle);

        PreviousAngle = curAngle;
        prevMousePos = curPointerPos;
        //Debug.Log("mouse drag");
        //Debug.Log(string.Format("dragged pos {0}", pointerEventData.position));
    }
}

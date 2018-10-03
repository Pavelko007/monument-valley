﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    public Transform handleCenterTransform;
    public RotateKeyboardNoInertia rotationExecutor;

    private Vector2 startMousePos;
    private Vector2 curMousePos;

    private bool isDraging = false;
    private float startAngle;

    public void OnMouseDown(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        startMousePos = pointerEventData.pressPosition;
        startAngle = RotationTarget.transform.localRotation.eulerAngles.z;
    }

    public void OnMouseUp(BaseEventData eventData)
    {
        isDraging = false;
        var pointerEventData = eventData as PointerEventData;
        var releasePos = pointerEventData.position;
        rotationExecutor.RotateToClosest();
    }

    public void OnMouseDrag(BaseEventData eventData)
    {
        isDraging = true;
        var pointerEventData = eventData as PointerEventData;
        curMousePos = pointerEventData.position;

        var handleCenterScreen = (Vector2)Camera.main.WorldToScreenPoint(handleCenterTransform.position);
        var curVec = curMousePos - handleCenterScreen;
        var startVec = startMousePos - handleCenterScreen;
        var curAngle = Vector2.SignedAngle(startVec, curVec);

        RotationTarget.transform.localRotation = Quaternion.Euler(0,0, startAngle+curAngle);

        Debug.Log("mouse drag");
        Debug.Log(string.Format("dragged pos {0}", pointerEventData.position));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    public Transform handleCenterTransform;
    public RotateKeyboardConstantSpeed rotationExecutor;

    private Vector2 curMousePos;

    private bool isDraging = false;
    private float previousAngle;
    private float angularSpeed;
    private Vector2 prevMousePos;


    void Update()
    {
        if (isDraging)
        {
            Debug.Log($"angular speed : {angularSpeed}");
        }
    }

    public void OnMouseDown(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        prevMousePos = pointerEventData.pressPosition;
        previousAngle = RotationTarget.transform.localRotation.eulerAngles.z;
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
        var prevVec = prevMousePos - handleCenterScreen;

        var relativeAngle = Vector2.SignedAngle(prevVec, curVec);

        var curAngle = previousAngle + relativeAngle;

        RotationTarget.transform.localRotation = Quaternion.Euler(0,0, curAngle);
        float deltaAngle = curAngle - previousAngle;
        angularSpeed = deltaAngle / Time.deltaTime;

        previousAngle = curAngle;
        prevMousePos = curMousePos;
        //Debug.Log("mouse drag");
        //Debug.Log(string.Format("dragged pos {0}", pointerEventData.position));
    }
}

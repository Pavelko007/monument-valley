using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    public Transform handleCenterTransform;
    public RotateKeyboardConstantSpeed rotationExecutor;

    private bool isDraging = false;

    
    private Vector2 prevMousePos;

    public float PreviousAngle
    {
        get { return rotationExecutor.previousAngle; }
        set { rotationExecutor.previousAngle = value; }
    }

    public float AngularSpeed
    {
        get { return rotationExecutor.angularSpeed; }
        set { rotationExecutor.angularSpeed = value; }
    }


    void Update()
    {
        if (isDraging)
        {
            Debug.Log($"angular speed : {AngularSpeed}");
        }
    }

    public void OnMouseDown(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        prevMousePos = pointerEventData.pressPosition;
        PreviousAngle = RotationTarget.transform.localRotation.eulerAngles.z;
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
        var curPointerPos = pointerEventData.position;

        var handleCenterScreen = (Vector2)Camera.main.WorldToScreenPoint(handleCenterTransform.position);
        var curVec = curPointerPos - handleCenterScreen;
        var prevVec = prevMousePos - handleCenterScreen;

        var relativeAngle = Vector2.SignedAngle(prevVec, curVec);

        var curAngle = PreviousAngle + relativeAngle;

        RotationTarget.transform.localRotation = Quaternion.Euler(0,0, curAngle);
        float deltaAngle = curAngle - PreviousAngle;
        AngularSpeed = deltaAngle / Time.deltaTime;

        PreviousAngle = curAngle;
        prevMousePos = curPointerPos;
        //Debug.Log("mouse drag");
        //Debug.Log(string.Format("dragged pos {0}", pointerEventData.position));
    }
}

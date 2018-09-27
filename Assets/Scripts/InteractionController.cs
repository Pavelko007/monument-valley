using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    public Transform handleCenterTransform;
    private Vector3 startMousePos;
    
    public void OnMouseDown(BaseEventData eventData)
    {
        startMousePos = Input.mousePosition;
        RotationTarget.transform.Rotate(transform.up, 45, Space.Self);
    }

    public void OnMouseUp(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        var releasePos = pointerEventData.position;
        RotationTarget.transform.Rotate(transform.up, -45, Space.Self);
        //compute angle between initial and release handle vectors
    }

    public void OnMouseDrag(BaseEventData eventData)
    {
        var pointerEventData = eventData as PointerEventData;
        Debug.Log(string.Format("press pos {0}, dragged pos {1}",pointerEventData.pressPosition, pointerEventData.position));
        Debug.Log("mouse drag");
    }
}

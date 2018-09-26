using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionController : MonoBehaviour
{
    public RotationTarget RotationTarget;
    private Vector3 startMousePos;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void OnMouseDown(BaseEventData eventData)
    {
        startMousePos = Input.mousePosition;
        RotationTarget.transform.Rotate(transform.up, 45, Space.Self);
    }

    public void OnMouseUp(BaseEventData eventData)
    {
        var curMousePos = Input.mousePosition;
        RotationTarget.transform.Rotate(transform.up, -45, Space.Self);
        Debug.Log("on mouse up");
    }
}

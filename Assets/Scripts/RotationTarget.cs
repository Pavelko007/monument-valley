using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// the attached transform should have rotation set to identity
/// </summary>
public class RotationTarget : MonoBehaviour
{
    public float Angle
    {
        get { return transform.eulerAngles.z; }
        set { transform.eulerAngles = new Vector3(0, 0, value); }
    }
}

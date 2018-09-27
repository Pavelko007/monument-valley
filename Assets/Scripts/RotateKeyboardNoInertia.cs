using UnityEngine;

public class RotateKeyboardNoInertia : MonoBehaviour
{
    enum State
    {
        RotatingClockwise,
        RotatingCounterclockwise,
        FixedPosition
    }

    private State state = State.FixedPosition;

    private float rotVel = 0f;
    private float smoothTime = .3f;

    public float CurAngle
    {
        get { return transform.eulerAngles.z; }
        set { transform.eulerAngles = new Vector3(0, 0, value); }
    }

    private int targetAngle;
    private float angleEps = 0.5f;

    public int TargetAngle
    {
        get { return targetAngle; }
        set
        {
            //make sure that angle wraps to [0,360) range
            targetAngle = (value % 360 + 360)%360;
        }
    }

    void Update()
    {
        switch (state)
        {
            case State.FixedPosition:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    state = State.RotatingCounterclockwise;
                    TargetAngle = (int) (CurAngle + 90);
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    state = State.RotatingClockwise;
                    TargetAngle = (int) (CurAngle - 90);
                }
                break;
            case State.RotatingClockwise:
            case State.RotatingCounterclockwise:
                var newAngle = Mathf.SmoothDampAngle(CurAngle, TargetAngle, ref rotVel, smoothTime);

                var angleDiff = Mathf.Min(
                    Mathf.Abs(newAngle - TargetAngle), 
                    Mathf.Abs(newAngle - 360 - TargetAngle)
                    );
                
                if (angleDiff < angleEps )
                {
                    CurAngle = TargetAngle;
                    state = State.FixedPosition;
                    break;
                }

                CurAngle = newAngle;
                break;
        }
    }
}

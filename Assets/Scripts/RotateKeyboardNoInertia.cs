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
    private const int FullRotationAngle = 360;
    private const int RotationStepDegrees = 90;

    public int TargetAngle
    {
        get { return targetAngle; }
        set
        {
            targetAngle = WrapAngle(value);
        }
    }

    private int WrapAngle(int angle)
    {
        return (angle % FullRotationAngle + FullRotationAngle)%FullRotationAngle;
    }

    void Update()
    {
        switch (state)
        {
            case State.FixedPosition:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    RotateCounterclockwise();
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    RotateClockwise();
                }
                break;
            case State.RotatingClockwise:
            case State.RotatingCounterclockwise:
                var newAngle = Mathf.SmoothDampAngle(CurAngle, TargetAngle, ref rotVel, smoothTime);

                var angleDiff = Mathf.Min(
                    Mathf.Abs(newAngle - TargetAngle), 
                    Mathf.Abs(newAngle - FullRotationAngle - TargetAngle)
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

    private void RotateCounterclockwise()
    {
        state = State.RotatingCounterclockwise;
        int numQuaterRotations = (int) CurAngle/ RotationStepDegrees;
        TargetAngle = (numQuaterRotations-1)*RotationStepDegrees;
    }

    public void RotateClockwise()
    {
        state = State.RotatingClockwise;
        int numQuaterRotations = (int)CurAngle / RotationStepDegrees;
        TargetAngle = (numQuaterRotations+1)*RotationStepDegrees;
    }
}

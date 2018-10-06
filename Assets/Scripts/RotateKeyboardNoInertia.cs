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
                RotateOneFrame();

                break;
        }
    }

    private void RotateOneFrame()
    {
        var newAngle = Mathf.SmoothDampAngle(CurAngle, TargetAngle, ref rotVel, smoothTime);

        var angleDiff = AngleDiff(newAngle, TargetAngle);

        if (angleDiff < angleEps)
        {
            CurAngle = TargetAngle;
            state = State.FixedPosition;
            return;
        }

        CurAngle = newAngle;
    }

    private float AngleDiff(float angle1, int angle2)
    {
        return Mathf.Min(
            Mathf.Abs(angle1 - angle2),
            Mathf.Abs(angle1 - FullRotationAngle - angle2)
        );
    }

    private void RotateCounterclockwise()
    {
        state = State.RotatingCounterclockwise;
        TargetAngle = GetNextCounterclockwiseAngle();
    }

    public void RotateClockwise()
    {
        state = State.RotatingClockwise;
        TargetAngle = GetNextClockwiseAngle();
    }

    private int GetNextClockwiseAngle()
    {
        int numQuaterRotations = (int) CurAngle / RotationStepDegrees;
        return (numQuaterRotations + 1) * RotationStepDegrees;
    }

    private int GetNextCounterclockwiseAngle()
    {
        int numQuaterRotations = (int) CurAngle / RotationStepDegrees;
        return (numQuaterRotations) * RotationStepDegrees;
    }

    public void RotateToClosest()
    {
        var nextCounterclockwiseAngle = GetNextCounterclockwiseAngle();

        if ( (Mathf.Abs(CurAngle - nextCounterclockwiseAngle) < 45))
        {
            RotateCounterclockwise();
        }
        else
        {
            RotateClockwise();
        }
    }
}

using UnityEngine;

public class RotateKeyboardConstantSpeed : MonoBehaviour
{
    public RotationTarget RotationTarget;

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
        get { return RotationTarget.Angle; }
        set { RotationTarget.Angle =  value; }
    }

    private float angleEps = 5f;
    private const int FullRotationAngle = 360;
    private const int RotationStepDegrees = 90;

    private int targetAngle;

    public int TargetAngle
    {
        get { return targetAngle; }
        set
        {
            targetAngle = (int)WrapAngle(value);
        }
    }

    public float angularSpeed;
    public bool isRotating = false;

    private float WrapAngle(float angle)
    {
        return (angle %  FullRotationAngle + FullRotationAngle)% FullRotationAngle;
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
                var newAngularSpeed = angularSpeed;
                var nextClockwiseAngle = GetNextClockwiseAngle();
                var nextCounterclockwiseAngle = GetNextCounterclockwiseAngle();
                //find closest point
                var accMag = 30;
                if (AngleDiff(CurAngle, nextCounterclockwiseAngle) < 45)
                {
                    
                    newAngularSpeed += Time.deltaTime * (-1)*accMag;
                }
                else
                {
                    newAngularSpeed += Time.deltaTime * accMag;
                }
                ///determine acc sign
                //calculate acceleration toward point

                RotateOneFrame();

                break;
        }

        if (isRotating)
        {
            Debug.Log($"angular speed : {angularSpeed}");
        }
    }
    public void StartRotation()
    {
        isRotating = true;
    }

    private void RotateOneFrame()
    {
        var newAngle = WrapAngle( CurAngle + angularSpeed*Time.deltaTime);

        var angleDiff = AngleDiff(newAngle, TargetAngle);

        if (angleDiff < angleEps)
        {
            StopRotation();
            return;
        }

        CurAngle = newAngle;
    }

    private void StopRotation()
    {
        isRotating = false;
        CurAngle = TargetAngle;
        state = State.FixedPosition;
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

    public void StopExternalRotation()
    {
        if (angularSpeed > 0)
        {
            RotateClockwise();
        }
        else
        {
            RotateCounterclockwise();
        }
    }

    public void RotateDeltaExternal(float deltaAngle)
    {
        CurAngle += deltaAngle;
        angularSpeed = deltaAngle / Time.deltaTime;
    }
}
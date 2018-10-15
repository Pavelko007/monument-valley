using System;
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

    private float angleEps = 2f;
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
    public int accMagnitude = 45;
    private float prevAngle;

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
                var counterclockwiseDist = AngleDiff(CurAngle, nextCounterclockwiseAngle);
                var clockwiseDist = 90 - counterclockwiseDist;
                float acc;

                float accStrenght;

                if (counterclockwiseDist < 45)
                {
                    accStrenght = CalcAccStrenght(counterclockwiseDist);
                    acc = (-1) * accStrenght * accMagnitude;
                }
                else
                {
                    accStrenght = CalcAccStrenght(clockwiseDist);
                    acc = accStrenght * accMagnitude;
                }
                newAngularSpeed += Time.deltaTime * acc;
                ///determine acc sign
                //calculate acceleration toward point

                RotateOneFrame();

                break;
        }

        
    }

    private void FixedUpdate()
    {
        if (isRotating)
        {

            angularSpeed = (CurAngle-prevAngle) / Time.deltaTime;
            prevAngle = CurAngle;
            Debug.Log($"angular speed : {angularSpeed}");
        }
    }


    private static float CalcAccStrenght(float distToClosestTarget)
    {
        return (45-distToClosestTarget)/45;
    }

    public void StartRotation()
    {
        isRotating = true;
        prevAngle = CurAngle;
    }

    private void RotateOneFrame()
    {
        var newAngle = WrapAngle( CurAngle + angularSpeed*Time.deltaTime);

        var angleDiff = AngleDiff(newAngle, TargetAngle);

        if (angleDiff < angleEps && Math.Abs(rotVel) < 5)
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
    }
}
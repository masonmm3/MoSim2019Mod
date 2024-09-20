using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class CheesyPoofs : MonoBehaviour
{
    public ConfigurableJoint Turret;
    public ConfigurableJoint Carriage;
    public ConfigurableJoint Arm;
    public ConfigurableJoint Intake;
    public ConfigurableJoint Climber;
    public ConfigurableJoint ClimberAxis;
    public Rigidbody Suction;
    public GameObject ClimbFeet;
    public GameObject ClawL;
    public GameObject ClawR;

    private HatchHandler hatchHandler;

    private BallHandler ballHandler;

    private DriveController driveController;

    private bool low;

    private bool islow;
    private bool mid;
    private bool ismid;
    private bool high;
    private bool ishigh;
    private bool climb;
    private bool special;
    private bool debounce = false;
    private bool BallIntake;
    private bool isIntaking;

    private float CarriageHeight;
    private Vector2 TurretAngle;
    private float ArmAngle;
    private float IntakeAngle;
    private float climbStage;
    private float hatchAngle;
    private float ClawAngle;
    private float timer = 0;
    private bool isRedRobot;
    private bool isFieldRelative;
    // Start is called before the first frame update
    void Start()
    {
        hatchHandler = GetComponent<HatchHandler>();
        ballHandler = GetComponent<BallHandler>();
        driveController = GetComponent<DriveController>();

        climbStage = 0;

        isRedRobot = driveController.isRedRobot;
        isFieldRelative = driveController.isFieldCentric;
    }

    // Update is called once per frame
    void Update()
    {

        if (low && !debounce)
        {
            islow = !islow;
            ismid = false;
            ishigh = false;
        }

        if (mid && !debounce)
        {
            ismid = !ismid;
            islow = false;
            ishigh = false;
        }

        if (high && !debounce)
        {
            ishigh = !ishigh;
            islow = false;
            ismid = false;
        }

        if (climb && !debounce)
        {
            climbStage += 1;
        }

        if (special || low || mid || high || climb)
        {
            debounce = true;
        }
        else
        {
            debounce = false;
        }

        if (isIntaking && !hatchHandler.hasHatchInRobot)
        {
            CarriageHeight = 0.3f;
            ArmAngle = -40f;
            IntakeAngle = -30;
        }
        else if (ismid && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 2.0f;
            ArmAngle = 60f;
            IntakeAngle = 0;
        }
        else if (ishigh && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 4.5f;
            ArmAngle = 90f;
            IntakeAngle = 40;
        }
        else if (ismid)
        {
            CarriageHeight = 0.5f;
            ArmAngle = 60;
            IntakeAngle = 0;
        }
        else if (ishigh)
        {
            CarriageHeight = 4.5f;
            ArmAngle = 70f;
            IntakeAngle = 0;
        }
        else if (islow)
        {
            CarriageHeight = 4.5f;
            ArmAngle = -50f;
            IntakeAngle = 0;
        }
        else if (ballHandler.hasBallInRobot)
        {
            CarriageHeight = 3.5f;
            ArmAngle = -65f;
            IntakeAngle = 0;
        }
        else
        {
            CarriageHeight = 1.8f;
            ArmAngle = -60;
            IntakeAngle = 0;
        }

        if (ballHandler.hasBallInRobot)
        {
            timer = 0.5f;
        }

         if (timer >= 0) timer -= Time.deltaTime;

        if ((isIntaking && !hatchHandler.hasHatchInRobot) || ballHandler.hasBallInRobot || timer >= 0)
        {
            ClawAngle = 0;
        } else
        {
            ClawAngle = 30;
        }
        
        Vector3 Target =  new Vector3(TurretAngle.y, 0 ,TurretAngle.x);

        float angleOffset;

        if (!isFieldRelative)
        {
            angleOffset = 0;
        } else if (isRedRobot)
        {
            angleOffset = transform.localRotation.eulerAngles.y + 270;
        }
        else
        {
            angleOffset = transform.localRotation.eulerAngles.y + 90;
        }


        Vector3 FieldRelativeAngle = Quaternion.AngleAxis(angleOffset, Vector3.up) * Target;

        float TargetAngle = Vector2.SignedAngle(new Vector2(FieldRelativeAngle.z, FieldRelativeAngle.x), new Vector2(0, 1));

        if (climbStage == 1)
        {
            TargetAngle = 0;
            CarriageHeight = 4.5f;
            ArmAngle = 30;
            IntakeAngle = 0;
            ClimberAxis.targetPosition = new Vector3(0, 0, 0);
            Climber.targetRotation = Quaternion.Euler(new Vector3(-45, 0, 0));
            ClimbFeet.transform.localRotation = Quaternion.RotateTowards(ClimbFeet.transform.localRotation, Quaternion.Euler(-50, 0, 0), 200 * Time.deltaTime);
        }
        else if (climbStage >= 2) {
            TargetAngle = 0;
            CarriageHeight = 1.0f;
            ArmAngle = -30;
            IntakeAngle = 0;
            ClimberAxis.targetPosition = new Vector3(0, 3, 0);
            Climber.targetRotation = Quaternion.Euler(new Vector3(-45, 0, 0));
            Suction.mass = 2000;
            Suction.useGravity = true;
        }

        

        Turret.targetRotation = Quaternion.Euler( new Vector3(0, -TargetAngle, 0));
        Carriage.targetPosition = new Vector3(0, -CarriageHeight, 0);
        Arm.targetRotation = Quaternion.Euler(new Vector3(ArmAngle, 0, 0));
        Intake.targetRotation = Quaternion.Euler(new Vector3(Arm.transform.localRotation.eulerAngles.x+9+IntakeAngle,0,0));
        ClawL.transform.localRotation = Quaternion.RotateTowards(ClawL.transform.localRotation, Quaternion.Euler(0, ClawAngle, 0), 100 * Time.deltaTime);
        ClawR.transform.localRotation = Quaternion.RotateTowards(ClawR.transform.localRotation, Quaternion.Euler(0, -ClawAngle, 0), 100 * Time.deltaTime);
    }

    public void onBallIntake(InputAction.CallbackContext ctx)
    {
        BallIntake = ctx.action.triggered;
    }

    public void onLow(InputAction.CallbackContext ctx)
    {
        low = ctx.action.triggered;
    }

    public void OnMid(InputAction.CallbackContext ctx)
    {
        mid = ctx.action.triggered;
    }

    public void OnHigh(InputAction.CallbackContext ctx)
    {
        high = ctx.action.triggered;
    }

    public void OnSpecial(InputAction.CallbackContext ctx)
    {
        special = ctx.action.triggered;
    }

    public void OnClimb(InputAction.CallbackContext ctx)
    {
        climb = ctx.action.triggered;
    }

    public void OnIntake(InputAction.CallbackContext ctx)
    {
        isIntaking = ctx.action.triggered;
    }

    public void onTurret(InputAction.CallbackContext ctx)
    {
        TurretAngle = ctx.ReadValue<Vector2>();
    }
}

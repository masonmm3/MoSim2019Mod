using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoboNauts : MonoBehaviour
{
    public Rigidbody robot;

    public ConfigurableJoint Carriage;

    public ConfigurableJoint ExtendStage;

    public ConfigurableJoint Climber;

    public GameObject hatchIntake;

    private HatchHandler hatchHandler;

    private BallHandler ballHandler;

    private DriveController driveController;

    public GameObject DiskIntakeL;

    public GameObject DiskIntakeR;

    public GameObject RearRayCastL;
    public GameObject RearRayCastR;
    public GameObject RayCastC;

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
    private float ExtendHeight;
    private float HatchIntakeAngle;
    private float climbStage;
    private float hatchAngle;
    // Start is called before the first frame update
    void Start()
    {
        hatchHandler = GetComponent<HatchHandler>();
        ballHandler = GetComponent<BallHandler>();
        driveController = GetComponent<DriveController>();

        climbStage = 0;
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

        if (mid && !debounce) {
            ismid = !ismid;
            islow = false;
            ishigh = false;
        }

        if (high && !debounce) {
            ishigh = !ishigh;
            islow = false;
            ismid = false;
        }

 
        if (islow && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 4.5f;
            ExtendHeight = 0;
        }
        else if (ismid && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 4.5f;
            ExtendHeight = 2.5f;
        }
        else if (ishigh && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 4.5f;
            ExtendHeight = 6.3f;
        } else  if (ballHandler.hasBallInRobot)
        {
            CarriageHeight = 2.8f;
            ExtendHeight = 0.0f;
        }
        else if (ismid) 
        {
            CarriageHeight = 4.5f;
            ExtendHeight = 1.0f;
        } 
        else if (ishigh)
        {
            CarriageHeight = 4.5f;
            ExtendHeight = 5.5f;
        }
        else
        {
            CarriageHeight = 1.5f;
            ExtendHeight = 0;
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
            HatchIntakeAngle = 0;
            CarriageHeight = 0.1f;
            ExtendHeight = 0;
        }else if (ballHandler.hasBallInRobot)
        {
            HatchIntakeAngle = 0;
        }
        else
        {
            HatchIntakeAngle = 80;
        }

        if(climbStage == 1)
        {
            CarriageHeight = 4.4f;
            ExtendHeight = 0.0f;
            Climber.targetPosition = new Vector3(0, 0, 0);
        } else if (climbStage == 2)
        {
            CarriageHeight = 0f;
            ExtendHeight = 0f;
            Climber.targetPosition = new Vector3(0, 3.8f, 0);

            driveController.moveSpeed = 1000;

            RearRayCastL.transform.position = RayCastC.transform.position;
            RearRayCastR.transform.position = RayCastC.transform.position;
        } else if (climbStage >= 3)
        {
            CarriageHeight = 0f;
            ExtendHeight = 0f;
            Climber.targetPosition = new Vector3(0, 0.0f, 0);

            driveController.moveSpeed = 700;

            RearRayCastL.transform.position = RayCastC.transform.position;
            RearRayCastR.transform.position = RayCastC.transform.position;
        }

        if(hatchHandler.hasHatchInRobot)
        {
            hatchAngle = 0;
        }
        else
        {
            hatchAngle = 100;
        }

        DiskIntakeL.transform.localRotation = Quaternion.RotateTowards(DiskIntakeL.transform.localRotation, Quaternion.Euler(0, hatchAngle, 0), 600 * Time.deltaTime);
        DiskIntakeR.transform.localRotation = Quaternion.RotateTowards(DiskIntakeR.transform.localRotation, Quaternion.Euler(0, -hatchAngle, 0), 600 * Time.deltaTime);
        hatchIntake.transform.localRotation = Quaternion.RotateTowards(hatchIntake.transform.localRotation, Quaternion.Euler(-HatchIntakeAngle, 0, 0), 600 * Time.deltaTime);
        Carriage.targetPosition = new Vector3(0,-CarriageHeight,0);
        ExtendStage.targetPosition = new Vector3(0, -ExtendHeight, 0);
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
}

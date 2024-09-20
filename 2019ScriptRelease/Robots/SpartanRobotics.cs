using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpartanRobotics : MonoBehaviour
{
    public HingeJoint lowerIntakeBar;

    public ConfigurableJoint Stage1;

    public ConfigurableJoint Stage2;

    public ConfigurableJoint IntakePivot;

    public ConfigurableJoint Climber;

    public ConfigurableJoint Climber2;

    private JointSpring spring = new JointSpring();

    private HatchHandler hatchHandler;

    private BallHandler ballHandler;

    private DriveController driveController;

    public GameObject FrontRayCastL;
    public GameObject FrontRayCastR;
    public GameObject RayCastC;

    private Rigidbody rb;

    private bool low;
    private bool islow;
    private bool mid;
    private bool ismid;
    private bool high;
    private bool ishigh;
    private bool climb;
    private bool special;
    private bool isSpecial;
    private bool debounce = false;
    private bool secondDebounce = false;
    private bool BallIntake;
    private bool isIntaking;

    private float CarriageHeight;
    private float ExtendHeight;
    private float intakeAngle;
    private float HatchIntakeAngle;
    private float climbStage;
    private float hatchAngle;
    private float deployTimer;
    private bool isDeployed;
    private bool isFlipping;
    private float moveSpeed1;

    // Start is called before the first frame update
    void Start()
    {
        spring.spring = 7000;
        spring.damper = 1000;

        deployTimer = 0.2f;
        isDeployed = false;

        isSpecial = false;

        hatchHandler = GetComponent<HatchHandler>();
        ballHandler = GetComponent<BallHandler>();
        driveController = GetComponent<DriveController>();
        rb = GetComponent<Rigidbody>();

        moveSpeed1 = driveController.moveSpeed;
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

        if (special && !debounce)
        {
            isSpecial = !isSpecial;
            isFlipping = true;
        }

        if (islow && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 0.5f;
            ExtendHeight = 0;
            
        }
        else if (ismid && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 1.9f;
            ExtendHeight = 1.0f;
            
        }
        else if (ishigh && ballHandler.hasBallInRobot)
        {
            CarriageHeight = 1.9f;
            ExtendHeight = 5.4f;
            
        }
        else if (ballHandler.hasBallInRobot)
        {
            CarriageHeight = -1.0f;
            ExtendHeight = 0.0f;
            ;
        }
        else if (ismid)
        {
            CarriageHeight = 1.9f;
            ExtendHeight = 1.0f;
            
        }
        else if (ishigh)
        {
            CarriageHeight = 1.9f;
            ExtendHeight = 5.4f;
            
        }
        else
        {
            CarriageHeight = -1.1f;
            ExtendHeight = 0;
            
        } 

        if (climb && !debounce)
        {
            climbStage += 1;
        }

        if (isIntaking && !debounce)
        {
            deployTimer = 0.2f;
        }

        if (isDeployed && !isIntaking && !secondDebounce)
        {
            deployTimer = 0.2f;
        }

        if (special || low || mid || high || climb || isIntaking)
        {
            debounce = true;
        }
        else
        {
            debounce = false;
        }

        if ((isDeployed && !isIntaking))
        {
            secondDebounce = true;
        }
        else
        {
            secondDebounce = false;
        }

        if (isFlipping)
        {
            intakeAngle = 90;
            isFlipping = false;
        }
        else if (isSpecial && ballHandler.hasBallInRobot)
        {
            intakeAngle = 180+30;
        }
        else if (isSpecial && hatchHandler.hasHatchInRobot)
        {
            intakeAngle =  -160;
        } else if (isSpecial)
        {
            intakeAngle = -160;
        }
        else if(ballHandler.hasBallInRobot)
        {
            intakeAngle = 45;
        }
        else if(hatchHandler.hasHatchInRobot)
        {
            intakeAngle = 20;
        } else
        {
            intakeAngle = 20;
        }

        if (isIntaking && !hatchHandler.hasHatchInRobot)
        {
            if(!ballHandler.hasBallInRobot && deployTimer <= 0)
            {
                CarriageHeight = 0;
                ExtendHeight = -1;
                intakeAngle = 0;
            } else
            {
                CarriageHeight = 0;
                ExtendHeight = 1;
                intakeAngle = 10;
            }

            spring.targetPosition = 95;

            if (deployTimer > -0.1)
            {
                deployTimer -= Time.deltaTime;
            }
            isDeployed = true;

        }
        else
        {
            spring.targetPosition = 0;
        }

        if (isDeployed && !isIntaking)
        {
            CarriageHeight = 0;
            ExtendHeight = 1;
            intakeAngle = 10;

            if (deployTimer <= 0.0)
            {
                isDeployed = false;
            }

            if (deployTimer > -0.1)
            {
                deployTimer -= Time.deltaTime;
            }
        }

        if (climbStage == 0)
        {
            Climber.targetPosition = new Vector3(0, 0, 0);
            Climber2.targetRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        else if (climbStage == 1)
        {
            Climber.targetPosition = new Vector3(0, 4.5f, 0);
            Climber2.targetRotation = Quaternion.Euler(new Vector3(0, 0, 0));
            FrontRayCastL.transform.position = RayCastC.transform.position;
            FrontRayCastR.transform.position = RayCastC.transform.position;
            driveController.moveSpeed = 3000;
            CarriageHeight = 1;
            intakeAngle = 90;
        }
        else if (climbStage == 2)
        {
            Climber.targetPosition = new Vector3(0, 4.5f, 0);
            Climber2.targetRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            CarriageHeight = 1;
            intakeAngle = 90;
        } else if (climbStage >= 3)
        {
            driveController.moveSpeed = moveSpeed1;
            Climber.targetPosition = new Vector3(0, 0.0f, 0);
            Climber2.targetRotation = Quaternion.Euler(new Vector3(90, 0, 0));
            rb.mass = 500;
            rb.centerOfMass = new Vector3(0.0f, 0, 0.8f);
            CarriageHeight = 1;
            intakeAngle = 90;
        }

        lowerIntakeBar.spring = spring;

        Stage1.targetPosition = new Vector3(0, -ExtendHeight, 0);
        Stage2.targetPosition = new Vector3(0, -CarriageHeight, 0);
        IntakePivot.targetRotation = Quaternion.Euler(new Vector3(intakeAngle, 0, 0));
    }

    public void OnIntake(InputAction.CallbackContext ctx)
    {
        isIntaking = ctx.action.triggered;
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
}

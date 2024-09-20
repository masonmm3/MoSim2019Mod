using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class JackInTheBot : MonoBehaviour
{
    private Rigidbody rb;
    public ConfigurableJoint HatchIntake;
    public ConfigurableJoint Arm;
    public ConfigurableJoint Climber;
    public Transform DriveOnClimb;

    private DriveController driveController;

    private bool low;
    private bool islow;
    private bool climb;
    private float climbStage;
    private bool isIntaking;
    private bool hatch;
    private bool isHatch;
    private bool debounce = false;
    private float hatchTimer;

    private float ArmAngle;
    private float HatchDistance;
    private float climberDistance;
    private Vector2 translateValue;
    // Start is called before the first frame update
    void Start()
    {
        driveController = GetComponent<DriveController>();
        rb = GetComponent<Rigidbody>();
        hatchTimer = 0.0f;
        climbStage = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (low && !debounce)
        {
            islow = !islow;
        }

        if(hatch && !debounce)
        {
            isHatch = !isHatch;
            hatchTimer = 0.5f;
        }

        if (climb && !debounce)
        {
            climbStage += 1;
        }

        if (low || hatch || climb)
        {
            debounce = true;
        }
        else
        {
            debounce = false;
        }

        if (islow && !isIntaking)
        {
            ArmAngle = 50;
        } else if (isIntaking)
        {
            ArmAngle = 105;
        } else
        {
            ArmAngle = 10;
        }

        if (hatchTimer > 0.0f)
        {
            HatchDistance = 0.8f;
            hatchTimer -= Time.deltaTime;
        } else
        {
            HatchDistance = 0;
        }

        if (climbStage == 1)
        {
            HatchDistance = 0;
            ArmAngle = 0;
        }
        else if (climbStage == 2) {
            HatchDistance = 0;
            ArmAngle = 110;
            Climber.targetPosition = new Vector3(0, 4.3f, 0);
            if (Physics.Raycast(DriveOnClimb.position, -transform.up, 0.5f))
            {
                rb.AddForceAtPosition(translateValue.y * transform.forward * 4000, DriveOnClimb.position);
            }
            driveController.isFieldCentric = false;
        } else if (climbStage == 3)
        {
            HatchDistance = 0;
            ArmAngle = 0;
            Climber.targetPosition = new Vector3(0, 0.0f, 0);
            driveController.isFieldCentric = false;
        }

        Arm.targetRotation = Quaternion.Euler( new Vector3(-ArmAngle, 0, 0));
        HatchIntake.targetPosition = new Vector3(0, 0, HatchDistance);
    }

    public void OnIntake(InputAction.CallbackContext ctx)
    {
        isIntaking = ctx.action.triggered;
    }

    public void onLow(InputAction.CallbackContext ctx)
    {
        low = ctx.action.triggered;
    }

    public void onHatch(InputAction.CallbackContext ctx)
    {
        hatch = ctx.action.triggered;
    }

    public void OnClimb(InputAction.CallbackContext ctx)
    {
        climb = ctx.action.triggered;
    }

    public void OnTranslate(InputAction.CallbackContext ctx)
    {
        translateValue = ctx.ReadValue<Vector2>();
    }
}

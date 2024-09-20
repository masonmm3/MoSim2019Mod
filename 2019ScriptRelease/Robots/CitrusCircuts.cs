using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using static Cinemachine.CinemachineFreeLook;

public class CitrusCircuts : MonoBehaviour
{
    public Rigidbody robot;
    public DriveController controller;
    public GameObject BallIntakePivot;
    
    public ConfigurableJoint IntakePivot;

    public ConfigurableJoint ElevatorCarriage;

    public ConfigurableJoint ElevatorExtendStage;

    public GameObject ClimbMainFourBar;

    public GameObject UpperFourBarLink;

    public GameObject LowerFourBarLink;

    public GameObject DiskIntake;

    public GameObject RearClimbPivot;

    public GameObject rearLeftDrive;

    public GameObject rearRightDrive;

    public ConfigurableJoint BuddyClimbLeft;

    public ConfigurableJoint BuddyClimbRight;

    public GameObject LimeLightRotateMate;

    private HatchHandler hatchHandler;


    private bool low;

    private bool islow;
    private bool mid;
    private bool ismid;
    private bool high;
    private bool ishigh;
    private bool climb;

    private bool special;

    private bool debounce = false;

    public bool flipIntake = false;

    private bool BallIntake;

    private BallHandler ballHandler;

    public float CarriageRotation;

    private bool canFlip;

    private float climbStage;

    private float rearClimbFlipAngle;

    public Transform target;

    public float ExtendSpeed = 0.5f;

    public float rotationMultiplyer = 1;

    public float LowerRotationMultiplyer = 1;

    public float UpperRotationMultiplyer = 1;

    public Transform HomeTarget;

    public float UpperRotationTarget;

    public float LowerRotationTarget;

    public Vector3 Axis;

    private bool isIntaking;

    private float flipTimer;

    // Start is called before the first frame update
    void Start()
    {
        ballHandler = gameObject.GetComponent<BallHandler>();
        hatchHandler = gameObject.GetComponent<HatchHandler>();
        flipIntake = false;
        islow = false;
        ismid = false;
        ishigh = false;
        climbStage = 0;
        rearClimbFlipAngle = 0;
        StowIntake();
    }

    // Update is called once per frame
    void Update()
    {
        
        float extendTarget;
        float CarriageTarget;

        if ((BallIntake || ballHandler.hasBallInRobot) && !hatchHandler.hasHatchInRobot) {
            BallIntakePivot.transform.localRotation = Quaternion.RotateTowards(BallIntakePivot.transform.localRotation, Quaternion.Euler(-45, BallIntakePivot.transform.localEulerAngles.y, BallIntakePivot.transform.localEulerAngles.z), 500 * Time.deltaTime);
            
        } else {
             BallIntakePivot.transform.localRotation = Quaternion.RotateTowards(BallIntakePivot.transform.localRotation, Quaternion.Euler(0, BallIntakePivot.transform.localEulerAngles.y, BallIntakePivot.transform.localEulerAngles.z), 500 * Time.deltaTime);
        }
        if (BallIntake && !flipIntake)
        {
            extendTarget = 0.1f;
            CarriageTarget = 0.5f;
            canFlip = false;
        }
        else if (islow && ballHandler.hasBallInRobot && !flipIntake){
            extendTarget = 0.1f;
            CarriageTarget = 3.2f;
            canFlip = false;
        } else if (ismid && ballHandler.hasBallInRobot && !flipIntake ){
            extendTarget = 0.5f;
            CarriageTarget = 4.4f;
            canFlip = false;
        } else if (ishigh && ballHandler.hasBallInRobot && !flipIntake){
            extendTarget = 5.1f;
            CarriageTarget = 4.4f;
            canFlip = false;
        } else if (islow){
            extendTarget = 0.1f;
            CarriageTarget = 3.2f;
            canFlip = false;
        } else if (ismid && !flipIntake){
            extendTarget = 0.1f;
            CarriageTarget = 4.3f;
            canFlip = false;
        } else if (ishigh && !flipIntake){
            extendTarget = 4.2f;
            CarriageTarget = 4.4f;
            canFlip = false;
        } else {
            extendTarget = 0.1f;
            CarriageTarget = 0.5f;
            canFlip = true;
        }

        if(ishigh && ballHandler.hasBallInRobot && !flipIntake) {
            CarriageRotation = 35;
        }
        else if (ismid && ballHandler.hasBallInRobot && !flipIntake) {
            CarriageRotation = 40;
        } else if(islow && ballHandler.hasBallInRobot && !flipIntake) {
            CarriageRotation = 22;
        }
        else if (ballHandler.hasBallInRobot && !flipIntake)
        {
            CarriageRotation = 40;
        }
        else if (flipIntake && !ballHandler.hasBallInRobot){
            if (flipTimer > 0)
            {
                flipTimer -= Time.deltaTime;


                CarriageRotation = 80;
            } else
            {
                CarriageRotation = 177;
            }
        } else {
            CarriageRotation = 0;
        }

        if (special && !debounce && !ballHandler.hasBallInRobot && canFlip) {
            flipIntake = !flipIntake;
        }
        if (special && !debounce && !ballHandler.hasBallInRobot && !canFlip)
        {
            StartCoroutine(StowDelay());
        }

        if (low && !debounce && !flipIntake) {
            islow = !islow;
            ismid = false;
            ishigh = false;
        }
        else if (low && !debounce && flipIntake)
        {
            flipIntake = false;
            StartCoroutine(FlipDelay(1));
        }

        if (mid && !debounce && !flipIntake) {
            ismid = !ismid;
            islow = false;
            ishigh = false;
        } else if (mid && !debounce && flipIntake) {
            flipIntake = false;
            StartCoroutine(FlipDelay(2));
        }
        print(high);
        if (high && !debounce && !flipIntake) {
            ishigh = !ishigh;
            islow = false;
            ismid = false;
            flipTimer = 0.3f;
        }
        else if (high && !debounce && flipIntake)
        {
            flipIntake = false;
            StartCoroutine(FlipDelay(3));
        }

        if (climb && !debounce)
        {
            climbStage += 1;
        }

        if (special || low || mid || high || climb){
            debounce = true;
        } else {
            debounce = false;
        }

        

        if (climbStage == 1)
        {
            rearClimbFlipAngle = 0;
            CarriageRotation = 0;
            extendTarget = 4.2f;
            CarriageTarget = 4.4f;
            canFlip = false;
            StowIntake();
        }
        else if (climbStage == 2)
        {
            rearClimbFlipAngle = 97;
            CarriageRotation = 0;
            extendTarget = 4.2f;
            CarriageTarget = 4.4f;
            canFlip = false;
            MoveIntakeBar();
            BuddyClimbLeft.targetRotation = Quaternion.Euler(0, 0, -100);
            BuddyClimbRight.targetRotation = Quaternion.Euler(0, 0, 100);
        }
        else if (climbStage == 3)
        {
            robot.mass = 700;
            controller.moveSpeed = 3000;
            rearClimbFlipAngle = 97;
            CarriageRotation = 0;
            extendTarget = 4.2f;
            CarriageTarget = 4.4f;
            canFlip = false;
            MoveIntakeBar();
            extendTarget = -0.4f;
            CarriageTarget = 0.7f;
            BuddyClimbLeft.targetRotation = Quaternion.Euler(0, 0, -50);
            BuddyClimbRight.targetRotation = Quaternion.Euler(0, 0, 50);
            rearLeftDrive.transform.position = ClimbMainFourBar.transform.position - new Vector3(0, 0.6f, 0);
            rearRightDrive.transform.position = ClimbMainFourBar.transform.position - new Vector3(0,0.6f,0);
        }
        else if (climbStage == 4)
        {
            controller.moveSpeed = 4000;
            robot.drag = 0.1f;
            rearClimbFlipAngle = 97;
            CarriageRotation = 0;
            extendTarget = 4.2f;
            CarriageTarget = 4.4f;
            canFlip = false;
            MoveIntakeBar();
            extendTarget = 2.0f;
            CarriageTarget = 0.7f;
            rearLeftDrive.transform.position = ClimbMainFourBar.transform.position - new Vector3(0, 0.6f, 0);
            rearRightDrive.transform.position = ClimbMainFourBar.transform.position - new Vector3(0, 0.6f, 0);
        }
        else if (climbStage >= 5)
        {
            BuddyClimbLeft.targetRotation = Quaternion.Euler(0, 0,0 );
            BuddyClimbRight.targetRotation = Quaternion.Euler(0, 0, 0);
            controller.moveSpeed = 4000;
            robot.drag = 0.1f;
            rearClimbFlipAngle = 97;
            CarriageRotation = 0;
            extendTarget = 4.2f;
            CarriageTarget = 4.4f;
            canFlip = false;
            MoveIntakeBar();
            extendTarget = 2.0f;
            CarriageTarget = 0.7f;
            rearLeftDrive.transform.position = ClimbMainFourBar.transform.position - new Vector3(0, 0.6f, 0);
            rearRightDrive.transform.position = ClimbMainFourBar.transform.position - new Vector3(0, 0.6f, 0);
        }

        if (flipIntake)
        {
            LimeLightRotateMate.transform.localRotation = Quaternion.Euler( new Vector3(LimeLightRotateMate.transform.localRotation.eulerAngles.x, 180, LimeLightRotateMate.transform.localRotation.eulerAngles.z));
        } else
        {
            LimeLightRotateMate.transform.localRotation = Quaternion.Euler(new Vector3(LimeLightRotateMate.transform.localRotation.eulerAngles.x, 0, LimeLightRotateMate.transform.localRotation.eulerAngles.z));
        }

        IntakePivot.targetRotation = Quaternion.Euler(CarriageRotation,0,0);

        print(Quaternion.RotateTowards(Quaternion.Euler(-IntakePivot.gameObject.transform.localRotation.eulerAngles.x, 0, 0), Quaternion.Euler(CarriageRotation, 0, 0), 4000 * Time.deltaTime).eulerAngles);

        ElevatorExtendStage.targetPosition = new Vector3(0,-extendTarget,0);
        ElevatorCarriage.targetPosition = new Vector3(0,-CarriageTarget,0);

        RearClimbPivot.transform.localRotation = Quaternion.RotateTowards(RearClimbPivot.transform.localRotation, Quaternion.Euler(rearClimbFlipAngle, 0, 0), 120*Time.deltaTime);
    }

    private void StowIntake()
    {
        ClimbMainFourBar.transform.rotation = Quaternion.RotateTowards(ClimbMainFourBar.transform.rotation, HomeTarget.transform.rotation.normalized, ExtendSpeed * Time.deltaTime * rotationMultiplyer);
        ClimbMainFourBar.transform.position = Vector3.MoveTowards(ClimbMainFourBar.transform.position, HomeTarget.transform.position, ExtendSpeed * Time.deltaTime);
        UpperFourBarLink.transform.rotation = Quaternion.RotateTowards(UpperFourBarLink.transform.rotation, Quaternion.Euler(0 * Axis + HomeTarget.transform.rotation.eulerAngles), ExtendSpeed * Time.deltaTime * rotationMultiplyer);
        LowerFourBarLink.transform.rotation = Quaternion.RotateTowards(LowerFourBarLink.transform.rotation, Quaternion.Euler(0 * Axis + HomeTarget.transform.rotation.eulerAngles), ExtendSpeed * Time.deltaTime * LowerRotationMultiplyer);
    }

    private void MoveIntakeBar()
    {
        ClimbMainFourBar.transform.rotation = Quaternion.RotateTowards(ClimbMainFourBar.transform.rotation, target.rotation, ExtendSpeed * Time.deltaTime * rotationMultiplyer);
        ClimbMainFourBar.transform.position = Vector3.MoveTowards(ClimbMainFourBar.transform.position, target.position, ExtendSpeed * Time.deltaTime);
        UpperFourBarLink.transform.rotation = Quaternion.RotateTowards(UpperFourBarLink.transform.rotation, Quaternion.Euler(UpperRotationTarget * Axis + HomeTarget.transform.rotation.eulerAngles), ExtendSpeed * Time.deltaTime * UpperRotationMultiplyer);
        LowerFourBarLink.transform.rotation = Quaternion.RotateTowards(LowerFourBarLink.transform.rotation, Quaternion.Euler(LowerRotationTarget * Axis + HomeTarget.transform.rotation.eulerAngles), ExtendSpeed * Time.deltaTime * LowerRotationMultiplyer);
    }

    public IEnumerator FlipDelay(float BoolNum)
    {
        if (BoolNum == 1)
        {
            low = false;
        }
        else if (BoolNum == 2)
        {
            mid = false;
        }
        else if (BoolNum == 3) { 
            high = false;
        }
        debounce = false;
        yield return new WaitForSeconds(0.5f);
        if (BoolNum == 1)
        {
            low = true;
        }
        else if (BoolNum == 2)
        {
            mid = true;
        }
        else if (BoolNum == 3)
        {
            high = true;
        }
        yield return new WaitForSeconds(0.1f);
        if (BoolNum == 1)
        {
            low = false;
        }
        else if (BoolNum == 2)
        {
            mid = false;
        }
        else if (BoolNum == 3)
        {
            high = false;
        }
    }

    public IEnumerator StowDelay()
    {
        if (islow)
        {
            low = true;
        }
        else if (ismid)
        {
            mid = true;
        }
        else if (ishigh)
        {
            high = true;
        }
        debounce = false;
        yield return new WaitForSeconds(0.3f);
        low = false;
        mid = false;
        high = false;
        yield return new WaitForSeconds(0.1f);
        special = true;
        yield return new WaitForSeconds(0.1f);
        special = false;
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

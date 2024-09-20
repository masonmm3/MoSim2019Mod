using System;
using System.Collections;
using TMPro;
using Unity.Barracuda;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DriveController : MonoBehaviour
{
    public RobotSettings robotType;
    public DriveTrain driveTrain;
    [SerializeField] private TMP_Text[] bumperNumbers;
    [SerializeField] private bool reverseBumperAllianceText = false;

    [SerializeField] private Transform[] rayCastPoints;
    public SwerveWheel[] swerveWheels;
    [SerializeField] private float rayCastDistance;
    [SerializeField] private bool flipRayCastDir = false;

    [SerializeField] private bool flipStartingReverse;

    [SerializeField] private Collider field;

    //Handles climbing logic
    public bool isGrounded = true;
    public bool isTouchingGround = true;
    public AudioSource robotPlayer;
    public AudioSource treadPlayer;
    public AudioSource gearPlayer;
    public AudioResource intakeSound;
    public AudioResource swerveSound;
    public AudioResource gearSound;
    public float moveSpeed = 20f;
    public float maxSpeed = 5;
    public float swerveSteerMutiplyer = 1.0f;
    public bool isRedRobot = false;
    public bool areRobotsTouching;
    public bool startingReversed = false;

    public static bool canBlueRotate;
    public static bool canRedRotate;
    public static bool isTouchingWallColliderRed = false;
    public static bool isTouchingWallColliderBlue = false;
    public Vector3 velocity { get; set; }
    public bool canIntake { get; set; }
    public static bool robotsTouching;
    public static bool isPinningRed = false;
    public static bool isPinningBlue = false;
    public static bool isAmped = false;
    public static bool isRedAmped = false;
    public bool isIntaking;

    private Rigidbody rb;
    private Vector2 translateValue;
    private Vector2 rotateValue;
    public float intakeValue = 0f;
    private bool dontPlayDriveSounds = false;
    private bool useSwerveSounds;
    private bool useIntakeSounds;

    public Material[] materialPrefab;
    [SerializeField] private GameObject[] bumper;
    private Material bumperMat;
    private Color defaultBumperColor;

    public float beforeVelocity;
    private bool dontUpdateBeforeVelocity = false;

    private Vector3 centerOfMass;
    
    public bool isFieldCentric = false;

    private GameManager gameManager;

    private Vector3 startingPos;
    private Quaternion startingRot;

    public bool atTargetPos = false;
    public bool atTargetRot = false;

    public float targetOffset;

    public bool validVision = false;

    private bool onAlign;

    public LedStripVision Led;
    private void Start()
    {

        canIntake = true;

        startingPos = transform.position;
        startingRot = transform.rotation;

        if (materialPrefab != null)
        {
            if (isRedRobot) {
                bumperMat = Instantiate(materialPrefab[0]);
            } else {
                bumperMat = Instantiate(materialPrefab[1]);
            }
            

            for (var i = 0; i < bumper.Length; i++) {
                 bumper[i].GetComponent<Renderer>().material = bumperMat;
            }
           
            

            defaultBumperColor = bumperMat.color;
        }
        else { Debug.LogError("Material prefab is not assigned!"); }

        if (!reverseBumperAllianceText) 
        {
            if (isRedRobot && PlayerPrefs.GetString("redName") != "")
            {
                foreach (TMP_Text bumperNumber in bumperNumbers) 
                {
                    bumperNumber.text = PlayerPrefs.GetString("redName");
                }
            }
            else if (!isRedRobot && PlayerPrefs.GetString("blueName") != "") 
            {
                foreach (TMP_Text bumperNumber in bumperNumbers) 
                {
                    bumperNumber.text = PlayerPrefs.GetString("blueName");
                }
            }
        }
        else 
        {
            if (isRedRobot && PlayerPrefs.GetString("blueName") != "")
            {
                foreach (TMP_Text bumperNumber in bumperNumbers) 
                {
                    bumperNumber.text = PlayerPrefs.GetString("blueName");
                }
            }
            else if (!isRedRobot && PlayerPrefs.GetString("redName") != "") 
            {
                foreach (TMP_Text bumperNumber in bumperNumbers) 
                {
                    bumperNumber.text = PlayerPrefs.GetString("redName");
                }
            }
        }
        

        useSwerveSounds = PlayerPrefs.GetInt("swerveSounds") == 1;
        useIntakeSounds = PlayerPrefs.GetInt("intakeSounds") == 1;

        treadPlayer.resource = swerveSound;
        treadPlayer.loop = true;

        gearPlayer.resource = gearSound;
        gearPlayer.loop = true;

        moveSpeed = moveSpeed - (moveSpeed * (PlayerPrefs.GetFloat("movespeed") / 100f));

        //Resetting static variables on start
        canBlueRotate = true;
        canRedRotate = true;

        isTouchingWallColliderRed = false;
        isTouchingWallColliderBlue = false;

        isPinningRed = false;
        isPinningBlue = false;
        robotsTouching = false;
        velocity = new Vector3(0f, 0f, 0f);
        isAmped = false;
        isRedAmped = false;
        isIntaking = false;

        //Initializing starting transforms
        rb = GetComponent<Rigidbody>();

        if (flipStartingReverse)
        {
            startingReversed = !startingReversed;
        }
        gameManager = GameObject.Find("GameGUI").GetComponent<GameManager>();

        field = GameObject.Find("Field").GetComponent<Collider>();
    }

    private void Update()
    {
        if (Led != null)
        {
                Led.Green = validVision;     
        }

        isGrounded = CheckGround();
        rb.centerOfMass = centerOfMass;
        areRobotsTouching = robotsTouching;

        if (GameManager.GameState == GameState.Endgame || GameManager.endBuzzerPlaying)
        {
                isTouchingGround = CheckTouchingGround();
        }

        if (!dontUpdateBeforeVelocity) 
        {
            if (!isTouchingWallColliderBlue && !isRedRobot || !isTouchingWallColliderRed && isRedRobot) 
            {
                beforeVelocity = rb.velocity.magnitude;
            }
        }

        if (!isRedRobot) 
        {
            if (robotsTouching && isTouchingWallColliderBlue)
            {
                isPinningBlue = true;
            }
            else 
            {
                isPinningBlue = false;
            }
        }
        else 
        {
            if (robotsTouching && isTouchingWallColliderRed)
            {
                isPinningRed = true;
            }
            else 
            {
                isPinningRed = false;
            }
        }
    
        if (useIntakeSounds) 
        {
            if (isIntaking && !robotPlayer.isPlaying) 
            {
                robotPlayer.Play();
            }
            else if (!isIntaking && robotPlayer.isPlaying)
            {
                robotPlayer.Stop();
            }
        }

        if (useSwerveSounds)
        {
            bool isMovingOrRotating = Math.Abs(Math.Round(velocity.x)) > 0f || Math.Abs(Math.Round(velocity.z)) > 0f || Math.Abs(rotateValue.x) > 0f;

            if (isMovingOrRotating && !dontPlayDriveSounds)
            {
            }
            else
            {
                StopSwerveSounds();
            }
        }
    }

    void FixedUpdate()
    {



        rb.maxLinearVelocity = maxSpeed;

       

       

        if (GameManager.canRobotMove)
        {
            if (onAlign && validVision)
            {              
                    rotateValue.x = targetOffset/30;
            } else if (onAlign)
            {
                rotateValue.x = 0;
            }
            
                
            


            if (driveTrain == DriveTrain.Tank)
            {
                Transform LeftFront = rayCastPoints[0].transform;
                Transform RightFront = rayCastPoints[1].transform;
                Transform LeftCenter = rayCastPoints[2].transform;
                Transform RightCenter = rayCastPoints[3].transform;
                Transform LeftRear = rayCastPoints[4].transform;
                Transform RightRear = rayCastPoints[5].transform;

                rb.maxAngularVelocity = (float)((LeftCenter.position.x - RightCenter.position.x * Math.PI) * (maxSpeed));

                float LeftTankSpeed = (float)(moveSpeed * Math.Clamp(translateValue.y + (rotateValue.x * 0.8), -1, 1));
                float RightTankSpeed = (float)(moveSpeed * Math.Clamp(translateValue.y - (rotateValue.x * 0.8), -1, 1));

                if (Physics.Raycast(LeftFront.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(LeftTankSpeed * transform.forward, LeftFront.position);
                }

                if (Physics.Raycast(LeftCenter.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(LeftTankSpeed * transform.forward, LeftCenter.position);
                }

                if (Physics.Raycast(LeftRear.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(LeftTankSpeed * transform.forward, LeftRear.position);
                }

                if (Physics.Raycast(RightFront.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(RightTankSpeed * transform.forward, RightFront.position);
                }

                if (Physics.Raycast(RightCenter.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(RightTankSpeed * transform.forward, RightCenter.position);
                }

                if (Physics.Raycast(RightRear.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(RightTankSpeed * transform.forward, RightRear.position);
                }
            } else if (driveTrain == DriveTrain.HDrive) {
                Transform LeftFront = rayCastPoints[0].transform;
                Transform RightFront = rayCastPoints[1].transform;
                Transform LeftCenter = rayCastPoints[2].transform;
                Transform RightCenter = rayCastPoints[3].transform;
                Transform LeftRear = rayCastPoints[4].transform;
                Transform RightRear = rayCastPoints[5].transform;
                Transform CenterWheel = rayCastPoints[6].transform;

                rb.maxAngularVelocity = (float)((LeftCenter.position.x - RightCenter.position.x * Math.PI) * (maxSpeed));

                float LeftTankSpeed = (float)(moveSpeed * Math.Clamp(translateValue.y + (rotateValue.x * 0.8), -1, 1));
                float RightTankSpeed = (float)(moveSpeed * Math.Clamp(translateValue.y - (rotateValue.x * 0.8), -1, 1));
                float CenterWheelSpeed = (float)(moveSpeed * 4 * Math.Clamp(translateValue.x, -1, 1));

                if (Physics.Raycast(LeftFront.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(LeftTankSpeed * transform.forward, LeftFront.position);
                }

                if (Physics.Raycast(LeftCenter.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(LeftTankSpeed * transform.forward, LeftCenter.position);
                }

                if (Physics.Raycast(LeftRear.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(LeftTankSpeed * transform.forward, LeftRear.position);
                }

                if (Physics.Raycast(RightFront.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(RightTankSpeed * transform.forward, RightFront.position);
                }

                if (Physics.Raycast(RightCenter.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(RightTankSpeed * transform.forward, RightCenter.position);
                }

                if (Physics.Raycast(RightRear.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(RightTankSpeed * transform.forward, RightRear.position);
                }

                if (Physics.Raycast(CenterWheel.position, -transform.up, rayCastDistance))
                {
                    rb.AddForceAtPosition(CenterWheelSpeed * transform.right, CenterWheel.position);
                }
            }
            else if (driveTrain == DriveTrain.Swerve)
            {
                /*
                    A = STR - RCW * L/2
                    B = STR + RCW * L/2
                    C = STR - RCW * W/2
                    D = STR + RCW * W/2
                 */


                Transform LeftFront = rayCastPoints[0].transform;
                Transform RightFront = rayCastPoints[1].transform;
                Transform LeftRear = rayCastPoints[2].transform;
                Transform RightRear = rayCastPoints[3].transform;

                Vector3 DriveInput = new Vector3(translateValue.y, 0, translateValue.x);

                float angle;
                if (isRedRobot)
                {
                    angle = transform.localRotation.eulerAngles.y + 270;
                } else
                {
                    angle = transform.localRotation.eulerAngles.y + 90;
                }
                

                Vector3 FieldRelativeAngle = Quaternion.AngleAxis(angle, Vector3.up) * DriveInput;

                float FWD, STR;

                if (isFieldCentric)
                {

                    FWD = FieldRelativeAngle.x;

                    STR = FieldRelativeAngle.z;
                } else
                {
                    FWD = DriveInput.x;

                    STR = DriveInput.z;
                }
                

                float RCW = -rotateValue.x * swerveSteerMutiplyer;

                float L = LeftFront.localPosition.z - RightFront.localPosition.z;

                float W = LeftFront.localPosition.x - RightFront.localPosition.x;

                float R = Mathf.Sqrt(MathF.Pow(L,2) + Mathf.Pow(W,2));

                float A = STR - RCW*(L / R);
                float B = STR + RCW*(L / R);
                float C = FWD - RCW*(W / R);
                float D = FWD + RCW*(W / R);

                float ws1 = Mathf.Sqrt(Mathf.Pow(B,2) + Mathf.Pow(C, 2));
                float wa1 = Mathf.Atan2(B, C)*180 / Mathf.PI;

                float ws2 = Mathf.Sqrt(Mathf.Pow(B, 2) + Mathf.Pow(D, 2));
                float wa2 = Mathf.Atan2(B, D)*180 / Mathf.PI;

                float ws3 = Mathf.Sqrt(Mathf.Pow(A, 2) + Mathf.Pow(D, 2));
                float wa3 = Mathf.Atan2(A, D)*180 / Mathf.PI;

                float ws4 = Mathf.Sqrt(Mathf.Pow(A, 2) + Mathf.Pow(C, 2));
                float wa4 = Mathf.Atan2(A, C)*180 / Mathf.PI;


                if (Physics.Raycast(LeftFront.position, -transform.up, rayCastDistance))
                {
                    rayCastPoints[0].transform.localEulerAngles = new Vector3(0,wa2,0);
                    rb.AddForceAtPosition(ws2 * LeftFront.forward * moveSpeed, LeftFront.position);
                }

                if (Physics.Raycast(LeftRear.position, -transform.up, rayCastDistance))
                {
                    rayCastPoints[2].transform.localEulerAngles = new Vector3(0, wa3, 0);
                    rb.AddForceAtPosition(ws3 * LeftRear.forward * moveSpeed, LeftRear.position);
                }

                if (Physics.Raycast(RightFront.position, -transform.up, rayCastDistance))
                {
                    rayCastPoints[1].transform.localEulerAngles = new Vector3(0, wa1, 0);
                    rb.AddForceAtPosition(ws1 * RightFront.forward * moveSpeed, RightFront.position);
                }

                if (Physics.Raycast(RightRear.position, -transform.up, rayCastDistance))
                {
                    rayCastPoints[3].transform.localEulerAngles = new Vector3(0, wa4, 0);
                    rb.AddForceAtPosition(ws4 * RightRear.forward * moveSpeed, RightRear.position);
                }

                swerveWheels[0].Wa = wa2;
                swerveWheels[2].Wa = wa3;
                swerveWheels[1].Wa = wa1;
                swerveWheels[3].Wa = wa4;
            }
            else
            {

            }
        }
        
    }

    private void StopSwerveSounds()
    {
        if (treadPlayer.isPlaying || gearPlayer.isPlaying)
        {
            treadPlayer.Stop();
            gearPlayer.Stop();
        }
    }

    public IEnumerator GrayOutBumpers(float duration) 
    {
        bumperMat.color = Color.gray;

        yield return new WaitForSeconds(duration);
        
        bumperMat.color = defaultBumperColor; 
    }

    public void OnRestart(InputAction.CallbackContext ctx)
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

    public void OnTranslate(InputAction.CallbackContext ctx)
    {
        translateValue = ctx.ReadValue<Vector2>();
    }

    public void OnRotate(InputAction.CallbackContext ctx) 
    {
        rotateValue = ctx.ReadValue<Vector2>();
    }

    public void OnIntake(InputAction.CallbackContext ctx) 
    {
        intakeValue = ctx.ReadValue<float>();
    }

    public void OnAlign(InputAction.CallbackContext ctx)
    {
        onAlign = ctx.action.triggered;
    }

    public bool CheckGround()
    {
        float distanceToTheGround = rayCastDistance;
        foreach (Transform rayCastPoint in rayCastPoints) 
        {
            if (!flipRayCastDir) 
            {
                if (Physics.Raycast(rayCastPoint.position, -transform.up, distanceToTheGround))
                {
                    return true;
                }
            }
            else 
            {
                if (Physics.Raycast(rayCastPoint.position, transform.up, distanceToTheGround))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckTouchingGround()
    {
        if (field.bounds.Intersects(gameObject.GetComponent<Collider>().bounds)) 
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isRedRobot) 
        {
            if (other.gameObject.CompareTag("RedPlayer"))
            {
                robotsTouching = true;
            }
            else if (other.gameObject.CompareTag("Field") || other.gameObject.CompareTag("Wall")) 
            {
                dontUpdateBeforeVelocity = true;
                isTouchingWallColliderBlue = true;
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                robotsTouching = true;
            }
            else if (other.gameObject.CompareTag("Field") || other.gameObject.CompareTag("Wall"))
            {
                dontUpdateBeforeVelocity = true;
                isTouchingWallColliderRed = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isRedRobot) 
        {
            if (other.gameObject.CompareTag("RedPlayer"))
            {
                robotsTouching = false;
            }
            else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Field")) 
            {
                dontUpdateBeforeVelocity = false;
                if (!isRedRobot) 
                {
                    isTouchingWallColliderBlue = false;
                }
                else 
                {
                    isTouchingWallColliderRed = false;
                }
            }
        }
        else
        {
            if (other.gameObject.CompareTag("Player"))
            {
                robotsTouching = false;
            }
            else if (other.gameObject.CompareTag("Wall") || other.gameObject.CompareTag("Field")) 
            {
                dontUpdateBeforeVelocity = false;
                if (!isRedRobot) 
                {
                    isTouchingWallColliderBlue = false;
                }
                else 
                {
                    isTouchingWallColliderRed = false;
                }
            }
        }
    }
}

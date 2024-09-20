using System.Collections;
using System.Collections.Generic;
using PathCreation;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private PathCreator BallPath;

    [SerializeField] Transform BallPathEnd;

    [SerializeField] Transform BallPathEndAnchor;

    public Transform BallPathStart;

    public GameObject prefabToInstantiate;
    public Transform BallSpawn;
    public bool PreloadBall = false;
    public float EjectSpeed =  10;

    public float EjectDelay = 0;

    public float IntakeDelay = 0;

    public GameObject touchedBall { get; set; }

    public bool BallWithinIntakeCollider { get; set; }

    public GameObject hiddenBall;

    public bool hasBallInRobot;

    private bool Eject;

    private bool canEject;

    private bool Intake;

    public AudioSource player;

    public GameObject Ball;

    private bool isEjecting;

    public AudioResource EjectSound;

    private HatchHandler hatchHandler;

    // Start is called before the first frame update
    void Start()
    {
        hiddenBall.SetActive(PreloadBall);
        hasBallInRobot = PreloadBall;

        hatchHandler = GetComponent<HatchHandler>();
    }

    // Update is called once per frame
    void Update()
    {


            if (touchedBall != null && BallWithinIntakeCollider && BallPath != null)
            {
                BallPath.bezierPath.MovePoint(0, BallPath.transform.InverseTransformPoint(touchedBall.transform.position));
            }
           

    if (hasBallInRobot){
        if (Eject && !isEjecting && canEject){
            StartCoroutine(EjectBallSequence());
        }
    }

        if (BallWithinIntakeCollider && !hasBallInRobot && Intake && !hatchHandler.hasHatchInRobot) 
                {
                    hasBallInRobot = true;
                    IntakeSequence();
                }

    }


    private IEnumerator CanNotEjectWhenRunning() 
    {
        canEject = false;
        yield return new WaitForSeconds(IntakeDelay);
        canEject = true;
    }

    private void IntakeSequence() 
    {

        
       
        BallWithinIntakeCollider = false;
        StartCoroutine(BallSplineAnimation());
        StartCoroutine(CanNotEjectWhenRunning());
        
    }

    private void EjectBall()
    {
        hasBallInRobot = false;

        Ball = Instantiate(prefabToInstantiate, BallSpawn.position, BallSpawn.rotation);
        Ball.transform.localScale = new Vector3(585,585,585);
        Ball.tag = "Ball";
        Rigidbody rb = Ball.GetComponent<Rigidbody>();

        Vector3 parentVelocity = GetComponent<Rigidbody>().velocity;
        
        rb.velocity = parentVelocity + (BallSpawn.forward.normalized * EjectSpeed);

        Ball = null;
    }

    public IEnumerator EjectBallSequence()
    {
        
        isEjecting = true;
        yield return new WaitForSeconds(EjectDelay);
        player.resource = EjectSound;
        player.Play();
        EjectBall();
        hiddenBall.SetActive(false);
        isEjecting = false;
    }

    private IEnumerator BallSplineAnimation() 
    {
        GameObject ball = touchedBall;

        ball.tag = "ignore";
        Transform child = ball.transform.GetChild(0);
        child.gameObject.SetActive(false);
        ball.layer = 12;
        Destroy(ball.GetComponent<BoxCollider>());

        ball.transform.SetParent(hiddenBall.transform.parent);
        float distanceTraveled = 0f;
        float IntakeSpeed = 1.5f;

        while (distanceTraveled < BallPath.path.length)
        {
            distanceTraveled += IntakeSpeed * Time.deltaTime;
            ball.transform.position = BallPath.path.GetPointAtDistance(distanceTraveled, EndOfPathInstruction.Stop);
            ball.transform.rotation = BallPath.path.GetRotationAtDistance(distanceTraveled, EndOfPathInstruction.Stop);

            float t = distanceTraveled / BallPath.path.length;
            ball.transform.localScale = Vector3.Lerp(ball.transform.localScale, hiddenBall.transform.localScale, t);

            if (!hasBallInRobot) 
            {
                break;
            }

            yield return null;
        }

        Destroy(ball);

        if (hasBallInRobot) 
        {
            hiddenBall.SetActive(true);
        }
    }

    public void OnEject(InputAction.CallbackContext ctx)
    {
        Eject = ctx.action.triggered;
    }

    public void OnIntake(InputAction.CallbackContext ctx)
    {
        Intake = ctx.action.triggered;
    }

    public void Reset() 
    {
        player.Stop();
        StopAllCoroutines();
        isEjecting = false;
        Eject = false;
        Intake = false;
        BallWithinIntakeCollider = false;

        hiddenBall.SetActive(true);
        hasBallInRobot = PreloadBall;
    }
}

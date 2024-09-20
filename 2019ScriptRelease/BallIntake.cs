using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class BallIntake : MonoBehaviour
{
    [SerializeField] private BallHandler BallHandler;
    // Start is called before the first frame update
    private GameObject Ball;

    public bool hasBall;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Ball != null && !Ball.activeSelf || Ball == null)
        {
            Ball = null;
            BallHandler.BallWithinIntakeCollider = false;
            BallHandler.touchedBall = null;
        }

        if (BallHandler.hasBallInRobot) {
            Ball = null;
            BallHandler.BallWithinIntakeCollider = false;
            BallHandler.touchedBall = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball") )
        {
            Ball = other.gameObject;
            BallHandler.BallWithinIntakeCollider = true;
            BallHandler.touchedBall = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            Ball = other.gameObject;
            BallHandler.BallWithinIntakeCollider = true;
            BallHandler.touchedBall = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (BallHandler.hasBallInRobot) {
            Ball = null;
            BallHandler.BallWithinIntakeCollider = false;
            BallHandler.touchedBall = null;
        }else if (other.gameObject.CompareTag("Ball"))
        {
            Ball = null;
            BallHandler.BallWithinIntakeCollider = false;
            BallHandler.touchedBall = null;
        }
    }
}

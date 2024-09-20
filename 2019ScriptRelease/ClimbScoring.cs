using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbScoring : MonoBehaviour
{
    public Collider collider;
    public bool isBlue = true;
    private bool onHab1;
    private bool onHab2;
    private bool onHab3;
    public int scoreContribution;
    public bool touchingGround;
    public Transform raycast;
    private bool endgame;
    private DriveController controller;
    // Start is called before the first frame update
    void Start()
    {
        onHab1 = false;
        onHab2 = false; 
        onHab3 = false;
        endgame = false;

        controller = GetComponent<DriveController>();

        isBlue = !controller.isRedRobot;
    }

    // Update is called once per frame
    void Update()
    {
        onHab1 = false;
        onHab2 = false;
        onHab3 = false;

        Collider[] colliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Hab1"))
            {
                onHab1 = true;
            }
            else if (collider.CompareTag("Hab2"))
            {
                onHab2 = true;
            }
            else if (collider.CompareTag("Hab3"))
            {
                onHab3 = true;
            }


            if (Physics.Raycast(raycast.position, -transform.up, 0.25f))
            {
                touchingGround = true;
            }
            else
            {
                touchingGround = false;
            }

            if (onHab1)
            {
                scoreContribution = 3;
            }
            else if (onHab2 && !touchingGround)
            {
                scoreContribution = 6;
            }
            else if (onHab3 && !touchingGround)
            {
                scoreContribution = 12;
            }
            else
            {
                scoreContribution = 0;
            }
        }   
    }
}

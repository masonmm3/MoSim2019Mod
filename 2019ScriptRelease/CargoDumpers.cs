using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoDumpers : MonoBehaviour
{
    private HingeJoint joint;
    private JointSpring spring;
    // Start is called before the first frame update
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        spring.spring = 7000;
        spring.damper = 800;
    }

    // Update is called once per frame
    void Update()
    {
        if (!(GameManager.GameState == GameState.Auto))
        {
            spring.targetPosition = 16;
            spring.spring = 400000;
            spring.damper = 400;
        } else
        {
            spring.targetPosition = 0;
        }

        joint.spring = spring;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveWheel : MonoBehaviour
{
    // Start is called before the first frame update
    private WheelCollider m_WheelCollider;
    public float Wa;
    // Start is called before the first frame update
    void Start()
    {
        m_WheelCollider = GetComponent<WheelCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Wa < 0)
        {
            Wa = -Wa + 180;
        }

        m_WheelCollider.steerAngle = Wa;
        m_WheelCollider.brakeTorque = 0;
        m_WheelCollider.motorTorque = 0.000000000000000000000000000001f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HatchHandofIntake : MonoBehaviour
{
    
    // Start is called before the first frame update
    private GameObject Hatch;
    public BallHandler ballIntake;
    public HatchHandler hatchHandler;
    public GameObject hiddenHatch;
    public GameObject HatchIntake;
    public bool is2910;
    private bool isDeployed;

    private bool isIntaking;
    private bool hatchActive;
    private float HatchIntakeAngle;

    void Start()
    {
        hiddenHatch.SetActive(false);
        hatchActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (is2910 && !isDeployed && !hatchHandler.hasHatchInRobot)
        {
            isDeployed = true;
            HatchIntakeAngle = -145;
        } else if (!hatchHandler.hasHatchInRobot && isIntaking && Hatch != null && !ballIntake.hasBallInRobot && !hatchActive && is2910)
        {
            Destroy(Hatch);
            hiddenHatch.SetActive(true);
            hatchActive = true;
            HatchIntakeAngle = -145;
        }
        else if (!hatchHandler.hasHatchInRobot && hatchActive && !isIntaking && is2910)
        {
            HatchIntakeAngle = -50;

            if (HatchIntake.transform.localEulerAngles == new Vector3(360-50, 0, 0))
            {
                hatchHandler.hasHatchInRobot = true;
                hatchHandler.hiddenHatch.SetActive(true);
                hiddenHatch.SetActive(false);
                hatchActive = false;
            }
        }
        else if (is2910 && isDeployed)
        {
            HatchIntakeAngle = -145;
        }

        if (!hatchHandler.hasHatchInRobot && isIntaking && Hatch != null && !ballIntake.hasBallInRobot && !hatchActive && !is2910)
        {
            Destroy(Hatch);
            hiddenHatch.SetActive(true);
            hatchActive = true;
            HatchIntakeAngle = 90;
        } else if (!hatchHandler.hasHatchInRobot && hatchActive && !isIntaking && !is2910)
        {
            HatchIntakeAngle = 0;

            if (HatchIntake.transform.localEulerAngles == new Vector3(0,0,0))
            {
                hatchHandler.hasHatchInRobot = true;
                hatchHandler.hiddenHatch.SetActive(true);
                hiddenHatch.SetActive(false);
                hatchActive = false;
            }
        } else if (!is2910)
        {
            HatchIntakeAngle = 90;
        }

        

        HatchIntake.transform.localRotation = Quaternion.RotateTowards(HatchIntake.transform.localRotation, Quaternion.Euler(HatchIntakeAngle, 0, 0), 200 * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hatch"))
        {
            Hatch = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Hatch"))
        {
            Hatch = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Hatch"))
        {
            Hatch = null;
        }
    }

    public void OnIntake(InputAction.CallbackContext ctx)
    {
        isIntaking = ctx.action.triggered;
    }
}

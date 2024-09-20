using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchIntake : MonoBehaviour
{
   [SerializeField] private HatchHandler HatchHandler;
    // Start is called before the first frame update
    private GameObject Hatch;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (Hatch != null && !Hatch.activeSelf || Hatch == null)
        {
            Hatch = null;
            HatchHandler.HatchWithinIntakeCollider = false;
            HatchHandler.touchedHatch = null;
        }

        if (HatchHandler.hasHatchInRobot) {
            Hatch = null;
            HatchHandler.HatchWithinIntakeCollider = false;
            HatchHandler.touchedHatch = null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Hatch") )
        {
            Hatch = other.gameObject;
            HatchHandler.HatchWithinIntakeCollider = true;
            HatchHandler.touchedHatch = other.gameObject;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
        if (other.gameObject.CompareTag("Hatch"))
        {
            Hatch = other.gameObject;
            HatchHandler.HatchWithinIntakeCollider = true;
            HatchHandler.touchedHatch = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (HatchHandler.hasHatchInRobot) {
            Hatch = null;
            HatchHandler.HatchWithinIntakeCollider = false;
            HatchHandler.touchedHatch = null;
        }else if (other.gameObject.CompareTag("Hatch"))
        {
            Hatch = null;
            HatchHandler.HatchWithinIntakeCollider = false;
            HatchHandler.touchedHatch = null;
        }
    }
}

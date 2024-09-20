using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimeLight : MonoBehaviour
{
    public DriveController controller;
    public Collider Collider;
    public Transform Transform;
    public GameObject[] target;
    private int TagsSeen;
    private int tagCount;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float Angle;

            if (Transform.eulerAngles.y > 180)
            {
                Angle = Transform.eulerAngles.y - 180;
            }
        TagsSeen = 0;

        Collider[] colliders = Physics.OverlapBox(Collider.bounds.center, Collider.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Target"))
            {
                TagsSeen++;
            }
        }

        


        for(var i = 0; i < target.Length; i++)
        {
            if (target[i] != null)
            {
                float TranslationOffset = -90;

                if (controller.isRedRobot)
                {
                    TranslationOffset = 90;
                }

                float OffsetAngle = Mathf.Abs(Transform.rotation.eulerAngles.y + TranslationOffset - target[i].transform.localRotation.eulerAngles.y);

                if (OffsetAngle > 180)
                {
                    OffsetAngle -= 360;
                }

                if (Mathf.Abs(OffsetAngle) > 33)
                {
                    target[i] = null;
                }
            }
        }

        float targetAngle = 0;
        float validTargets = 0;



        for (var i = 0; i < target.Length; i++)
        {
            if (target[i] != null)
            {

                Vector3 directionToTarget = (target[i].transform.position - Transform.position).normalized; ;
                float Target = Quaternion.LookRotation(directionToTarget).eulerAngles.y - (Transform.rotation.eulerAngles.y);
                if (Mathf.Abs(Target) > 180)
                {
                    Target -= 360 * (Target/Mathf.Abs(Target));
                }
                targetAngle += Target;
                validTargets += 1;
            }
        }

        if (validTargets > 0)
        {
            controller.targetOffset = (targetAngle/validTargets);
            controller.validVision = true;
        } else
        {
            controller.validVision = false;
        }
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            if (target.Length > 0)
            {
                for (var i = 0; i < target.Length; i++)
                {
                    if (target[i] == null)
                    {
                        target[i] = other.gameObject;
                        return;
                    }
                    else if (target[i] == other.gameObject)
                    {
                        return;
                    }
                }
            }
            else
            {
                {
                    target[0] = other.gameObject;
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            for (var i = 0; i < target.Length; i++)
            {
                if (target[i] != other.gameObject && target[i] == null)
                {
                    target[i] = other.gameObject;
                    return;
                } else if (target[i] == other.gameObject)
                {
                    return;
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            for(var i=0; i<target.Length; i++)
            {
                if (target[i] == other.gameObject)
                {
                    target[i] = null;
                    
                }
            }
        }
    }
}

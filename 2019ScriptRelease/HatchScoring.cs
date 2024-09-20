using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HatchScoring : MonoBehaviour
{
   public bool hasHatch = false;
    // Start is called before the first frame update
    public GameObject HiddenHatch;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if (hasHatch) {
        HiddenHatch.SetActive(true);
       } else {
        HiddenHatch.SetActive(false);
       }
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject hatch = other.gameObject;
        if (other.gameObject.CompareTag("Hatch") && !hasHatch)
        {
            Destroy(hatch);
            hasHatch = true;
        }
    }

    private void OnTriggerStay(Collider other) 
    {
         GameObject hatch = other.gameObject;
        if (other.gameObject.CompareTag("Hatch") && !hasHatch)
        {
            Destroy(hatch);
            hasHatch = true;
        }
    }

    
}

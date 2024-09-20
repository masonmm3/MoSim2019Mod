using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CargoScoring : MonoBehaviour
{
    public Collider collider;

    private float CargoInZone;

    public bool hasCargoScored;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        CargoInZone = 0;

        Collider[] colliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Ball"))
            {
                CargoInZone++;
            }
        }

        if (CargoInZone >= 1) {
            hasCargoScored = true;
        } else {
            hasCargoScored = false;
        }
        
    }
}

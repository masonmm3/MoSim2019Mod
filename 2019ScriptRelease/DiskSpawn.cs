using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiskSpawn : MonoBehaviour
{
    public Collider collider;
    // Start is called before the first frame update
    public GameObject Hatch;

    public GameObject Spawn;

    public bool canSpawn;

    public float HatchInZone;

    public bool spawn;

    private GameObject resedint;

    void Start()
    {
        canSpawn = true;
    }

    // Update is called once per frame
    void Update()
    {
        ResetHatch();   
        CheckNumOfHatchInZone();
        if (HatchInZone < 1) {
            if (canSpawn) {
                StartCoroutine(SpawnHatch());
            }
        }
    }

    private void CheckNumOfHatchInZone() 
    {
        Collider[] colliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Hatch"))
            {
                HatchInZone++;
            }
        }
    }

    private void ResetHatch() 
    {
        HatchInZone = 0;
    }

    private IEnumerator SpawnHatch() 
    {
        canSpawn = false;
        Object.Instantiate(Hatch, Spawn.transform);
        yield return new WaitForSeconds(4f);
        canSpawn = true;
    }

    
}

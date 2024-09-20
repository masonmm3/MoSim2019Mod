using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallSpawn : MonoBehaviour
{
    public Collider collider;
    // Start is called before the first frame update
    public GameObject Ball;

    public GameObject Spawn;

    public bool canSpawn;

    public float BallInZone;

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
        CheckNumOfBallInZone();
        if (BallInZone/2 < 2) {
            if (canSpawn) {
                StartCoroutine(SpawnBall());
            }
        }
    }

    private void CheckNumOfBallInZone() 
    {
        Collider[] colliders = Physics.OverlapBox(collider.bounds.center, collider.bounds.extents, Quaternion.identity);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Ball"))
            {
                BallInZone++;
            }
        }
    }

    private void ResetHatch() 
    {
        BallInZone = 0;
    }

    private IEnumerator SpawnBall() 
    {
        canSpawn = false;
        Object.Instantiate(Ball, Spawn.transform);
        yield return new WaitForSeconds(4f);
        canSpawn = true;
    }

    
}

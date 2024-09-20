using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HatchHandler : MonoBehaviour
{
    private bool deploy;
    private bool debounce;

    public bool preloadHatch = false;

    public GameObject prefabToInstantiate;

    public GameObject touchedHatch { get; set; }

    public bool HatchWithinIntakeCollider { get; set; }

    public GameObject hiddenHatch;

    public bool hasHatchInRobot;

    public Transform HatchSpawn;

    public float ToggleDelay = 0;

    public AudioSource player;

    public GameObject Hatch;

    private bool isEjecting;

    private bool canToggle;

    public AudioResource EjectSound;

    private BallHandler ballHandler;
    private bool isIntaking;
    // Start is called before the first frame update
    void Start()
    {
        ballHandler = GetComponent<BallHandler>();
        hiddenHatch.SetActive(preloadHatch);
        hasHatchInRobot = preloadHatch;

        canToggle = true;
    }

    // Update is called once per frame
    void Update()
    { 
        if (hasHatchInRobot){
            if (!isEjecting && canToggle && !debounce && deploy){
                StartCoroutine(EjectHatchSequence());
            }
        }

        if (!hasHatchInRobot && !debounce && deploy && !ballHandler.hasBallInRobot & !isIntaking) {
            StartCoroutine(IntakeSequence());
        }

        if (deploy){
            debounce = true;
        } else {
            debounce = false;
        }
    }

    public void OnChangeDeploy(InputAction.CallbackContext ctx)
    {
        deploy = ctx.action.triggered;
    }

    private IEnumerator CanNotEjectWhenRunning() 
    {
        canToggle = false;
        yield return new WaitForSeconds(ToggleDelay);
        canToggle = true;
    }

    private IEnumerator IntakeSequence() 
    {
        isIntaking = true;
        yield return new WaitForSeconds(ToggleDelay);
        if (HatchWithinIntakeCollider)
        {
            hasHatchInRobot = true;
            GameObject hatch = touchedHatch;
            Destroy(hatch);

            if (hasHatchInRobot) 
            {
                hiddenHatch.SetActive(true);
            }
        HatchWithinIntakeCollider = false;
        }
        
        StartCoroutine(CanNotEjectWhenRunning());

        isIntaking = false;
    }

    private void EjectHatch()
    {
        hasHatchInRobot = false;

        Hatch = Instantiate(prefabToInstantiate, HatchSpawn.position, HatchSpawn.rotation);
        Hatch.transform.localScale = new Vector3(5.85f,5.85f,5.85f);
        Hatch.tag = "Hatch";
        Rigidbody rb = Hatch.GetComponent<Rigidbody>();

        Vector3 parentVelocity = GetComponent<Rigidbody>().velocity;
        
        rb.velocity = parentVelocity + (HatchSpawn.up.normalized * 2);
    }

    public IEnumerator EjectHatchSequence()
    {
        
        isEjecting = true;
        yield return new WaitForSeconds(ToggleDelay);
        player.resource = EjectSound;
        player.Play();
        EjectHatch();
        hiddenHatch.SetActive(false);
        isEjecting = false;
    }
}

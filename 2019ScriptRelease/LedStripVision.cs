using System.Collections;
using UnityEngine;

public class LedStripVision : MonoBehaviour
{
    private Material mat;
    public GameObject[] leds;

    public float intensity = 200f;

    [SerializeField] private bool useCustomColors = false;

    [SerializeField] private Color unlitColor;
    [SerializeField] private Color hasVisionTarget;

    public bool Green = false;

    private void Start() 
    {

        mat = new Material(Shader.Find("Standard"));
        mat.EnableKeyword("_EMISSION");

        foreach (GameObject led in leds) 
        {
            led.GetComponent<Renderer>().material = mat;
        }

        //Use default color scheme
        if (!useCustomColors) 
        {
            unlitColor = Color.white;
            hasVisionTarget = Color.green;
        }
        mat.color = unlitColor;
    }
    
    private void Update()
    {
            if (Green)
            {
                mat.SetColor("_EmissionColor", hasVisionTarget * intensity);
            }
            else
            {
                mat.SetColor("_EmissionColor", unlitColor * intensity);
            }        
    }
}

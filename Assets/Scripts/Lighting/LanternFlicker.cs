using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LanternFlicker : MonoBehaviour
{
    private Light2D light;
    public float minIntensity = 5f;
    public float maxIntensity = 7.5f;

    void Start()
    {
      light = GetComponentInChildren<Light2D>();  
    } 

    void Update()
    {
        // Creates a smooth, random flickering effect
        light.intensity = Mathf.Lerp(light.intensity, Random.Range(minIntensity, maxIntensity), Time.deltaTime * 10);
    }
}
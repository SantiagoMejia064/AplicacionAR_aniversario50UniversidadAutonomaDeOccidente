using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecciónQ : MonoBehaviour
{
    public Light luz;
    public Renderer circulo;
    public float velocidad = 2f;
    public float minBrillo = 0.2f, maxBrillo = 1f;
    public Color color = Color.red;

    void Update()
    {
        float t = (Mathf.Sin(Time.time * velocidad) + 1f) / 2f;
        float brillo = Mathf.Lerp(minBrillo, maxBrillo, t);

        if (luz) luz.intensity = brillo * 3f;

        if (circulo)
        {
            circulo.material.color = color * brillo;
        }
    }
}

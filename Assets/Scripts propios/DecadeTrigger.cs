using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecadeTrigger : MonoBehaviour
{
    public string decadeName; // Ejemplo: "1970"
    private DecadeLightManager lightManager;

    private void Start()
    {
        lightManager = FindObjectOfType<DecadeLightManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && lightManager != null)
        {
            lightManager.FocusOnDecade(decadeName);
            // Aquí ya puedes también llamar al asistente de voz si lo deseas
        }
    }
}

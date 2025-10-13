using System.Collections;
using System.Collections.Generic;
using Convai.Scripts.Runtime.Features;
using UnityEngine;

public class controladorAsistente : MonoBehaviour
{
    [Header("Asistente Convai")]
    [SerializeField] private GameObject assistant; // El GameObject del asistente
    [SerializeField] private ConvaiActionsHandler convaiActionsHandler;

    [Header("Targets de las secciones")]
    [SerializeField] private Transform seccion70s;
    [SerializeField] private Transform seccion80s;
    [SerializeField] private Transform seccion90s;
    [SerializeField] private Transform seccion2000;
    [SerializeField] private Transform seccion2010;

    [Header("Opciones")]
    [SerializeField] private bool activarSeccionConvai = true; // Activar sección de Narrative Design
    [SerializeField] private string seccionID70s = "Seccion_70s";
    [SerializeField] private string seccionID80s = "Seccion_80s";
    [SerializeField] private string seccionID90s = "Seccion_90s";
    [SerializeField] private string seccionID2000 = "Seccion_2000";
    [SerializeField] private string seccionID2010 = "Seccion_2010";


    // Método para mover el asistente a un target
    private void TeletransportarAsistente(Transform target, string seccionID = "")
    {
        if (target == null || assistant == null) return;

        assistant.transform.position = target.position;
        assistant.transform.rotation = target.rotation;

    }

    private void OnTriggerEnter(Collider other)
{
    // Solo si la cámara AR / jugador entra en el trigger
    if (!other.CompareTag("Player")) return;

    // DEBUG: confirmar colisión
    Debug.Log("Trigger detectado en: " + gameObject.name + " por " + other.name);

    switch (gameObject.name)
    {
        case "Seccion los 70 trigger":
            TeletransportarAsistente(seccion70s, seccionID70s);
            break;
        case "Seccion los 80 trigger":
            TeletransportarAsistente(seccion80s, seccionID80s);
            break;
        case "Seccion los 90 trigger":
            TeletransportarAsistente(seccion90s, seccionID90s);
            break;
        case "Seccion los 2000 trigger":
            TeletransportarAsistente(seccion2000, seccionID2000);
            break;
        case "Seccion los 2010 trigger":
            TeletransportarAsistente(seccion2010, seccionID2010);
            break;
        default:
            Debug.LogWarning("Trigger no reconocido: " + gameObject.name);
            break;
    }
}

}

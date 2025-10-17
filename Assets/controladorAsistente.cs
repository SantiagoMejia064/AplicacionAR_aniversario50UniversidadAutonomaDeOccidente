using UnityEngine;

public class ControladorAsistente : MonoBehaviour
{
    [Header("Asistente Convai")]
    [SerializeField] private GameObject assistant; // Tu modelo 3D del asistente

    [Header("Targets de las secciones")]
    [SerializeField] private Transform seccion70s;
    [SerializeField] private Transform seccion80s;
    [SerializeField] private Transform seccion90s;
    [SerializeField] private Transform seccion2000;
    [SerializeField] private Transform seccion2010;

    [Header("Opciones Convai")]
    [SerializeField] private bool activarSeccionConvai = true;
    [SerializeField] private string seccionID70s = "Seccion_70s";
    [SerializeField] private string seccionID80s = "Seccion_80s";
    [SerializeField] private string seccionID90s = "Seccion_90s";
    [SerializeField] private string seccionID2000 = "Seccion_2000";
    [SerializeField] private string seccionID2010 = "Seccion_2010";

    private void OnTriggerEnter(Collider other)
    {
        // Solo si quien entra es la cámara / jugador
        if (!other.CompareTag("Player")) return;

        // DEBUG: confirmar colisión
        //Debug.Log("Trigger detectado en: " + gameObject.name + " por: " + other.name);

        // Dependiendo del trigger, mover el asistente
        switch (gameObject.name)
        {
            case "Seccion los 70 Trigger":
                TeletransportarAsistente(seccion70s, seccionID70s);
                break;
            case "Seccion los 80 Trigger":
                TeletransportarAsistente(seccion80s, seccionID80s);
                break;
            case "Seccion los 90 Trigger":
                TeletransportarAsistente(seccion90s, seccionID90s);
                break;
            case "Seccion los 2000 Trigger":
                TeletransportarAsistente(seccion2000, seccionID2000);
                break;
            case "Seccion los 2010 Trigger":
                TeletransportarAsistente(seccion2010, seccionID2010);
                break;
            default:
                Debug.LogWarning("Trigger no reconocido: " + gameObject.name);
                break;
        }
    }

    private void TeletransportarAsistente(Transform target, string seccionID = "")
    {
        if (assistant == null || target == null)
        {
            Debug.LogError("Assistant o Target son null");
            return;
        }

        // Teletransportar
        assistant.transform.position = target.position;
        assistant.transform.rotation = target.rotation;

        Debug.Log("Asistente movido a: " + target.position);

    }
}

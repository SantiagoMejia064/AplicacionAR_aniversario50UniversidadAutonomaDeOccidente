using UnityEngine;

public class ControladorAsistente : MonoBehaviour
{
    [Header("Asistente Convai")]
    [SerializeField] private GameObject assistant; // Tu modelo 3D del asistente

    [Header("Botones para videos")]//Nuevo
    [SerializeField] private GameObject los80sButton;
    [SerializeField] private GameObject los90sButton;
    [SerializeField] private GameObject los2000sButton;
    [SerializeField] private GameObject los2010sButton;

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
        if (!other.CompareTag("MainCamera")) return;

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
                los80sButton.SetActive(true);
                break;
            case "Seccion los 90 Trigger":
                TeletransportarAsistente(seccion90s, seccionID90s);
                los90sButton.SetActive(true);
                break;
            case "Seccion los 2000 Trigger":
                TeletransportarAsistente(seccion2000, seccionID2000);
                los2000sButton.SetActive(true);
                break;
            case "Seccion los 2010 Trigger":
                TeletransportarAsistente(seccion2010, seccionID2010);
                los2010sButton.SetActive(true);
                break;
            default:
                Debug.LogWarning("Trigger no reconocido: " + gameObject.name);
                break;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Solo si quien sale es la cámara / jugador
        if (!other.CompareTag("MainCamera")) return;

        // Dependiendo del trigger, desactivar botones
        switch (gameObject.name)
        {
            case "Seccion los 80 Trigger":
                los80sButton.SetActive(false);
                break;
            case "Seccion los 90 Trigger":
                los90sButton.SetActive(false);
                break;
            case "Seccion los 2000 Trigger":
                los2000sButton.SetActive(false);
                break;
            case "Seccion los 2010 Trigger":
                los2010sButton.SetActive(false);
                break;
            default:
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

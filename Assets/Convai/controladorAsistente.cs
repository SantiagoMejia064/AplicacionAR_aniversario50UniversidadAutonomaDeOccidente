using UnityEngine;

public class ControladorAsistenteCamara : MonoBehaviour
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
        // Solo triggers de los cubos
        switch (other.gameObject.name)
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
                Debug.Log("Objeto en trigger no reconocido: " + other.gameObject.name);
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
        
        /*
        // Activar sección de Convai si se indica
        if (activarSeccionConvai && !string.IsNullOrEmpty(seccionID))
        {
            object value = Convai.Scripts.Runtime.Features.NarrativeDesignAPI.NarrativeDesignManager.Instance.InvokeSectionEvent(seccionID, true);
        }
        */
    }
}

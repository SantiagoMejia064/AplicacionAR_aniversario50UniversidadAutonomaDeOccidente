using UnityEngine;

public class ControladorAsistenteCamara : MonoBehaviour
{
    [Header("Asistente Convai")]
    [SerializeField] private GameObject assistant;

    [Header("Targets de las secciones")]
    [SerializeField] private Transform seccion70s;
    [SerializeField] private Transform seccion80s;
    [SerializeField] private Transform seccion90s;
    [SerializeField] private Transform seccion2000;
    [SerializeField] private Transform seccion2010;

    private void OnTriggerEnter(Collider other)
    {
        // Solo triggers de los cubos
        switch (other.gameObject.name)
        {
            case "Seccion los 70 Trigger":
                TeletransportarAsistente(seccion70s);
                break;
            case "Seccion los 80 Trigger":
                TeletransportarAsistente(seccion80s);
                break;
            case "Seccion los 90 Trigger":
                TeletransportarAsistente(seccion90s);
                break;
            case "Seccion los 2000 Trigger":
                TeletransportarAsistente(seccion2000);
                break;
            case "Seccion los 2010 Trigger":
                TeletransportarAsistente(seccion2010);
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

        assistant.transform.position = target.position;
        assistant.transform.rotation = target.rotation;
    }
}

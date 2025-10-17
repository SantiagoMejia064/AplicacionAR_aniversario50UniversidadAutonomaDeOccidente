using UnityEngine;
using System.Collections;

public class ControladorAsistente : MonoBehaviour
{
    [Header("Asistente Convai (si se instancia en runtime lo busco por Tag)")]
    [SerializeField] private GameObject assistant;     // si está vacío, lo busco
    [SerializeField] private string assistantTag = "Assistant";
    [SerializeField] private float findRetrySeconds = 0.5f;
    [SerializeField] private int findMaxTries = 30;    // 15s máx buscando

    [Header("Targets de las secciones")]
    [SerializeField] private Transform seccion70s;
    [SerializeField] private Transform seccion80s;
    [SerializeField] private Transform seccion90s;
    [SerializeField] private Transform seccion2000;
    [SerializeField] private Transform seccion2010;

    private Transform assistantT;
    private bool _searching;

    private void OnEnable()
    {
        // cache si ya está asignado en el inspector
        if (assistant != null) assistantT = assistant.transform;

        // si no hay referencia, intento encontrarlo
        if (assistantT == null && !_searching)
            StartCoroutine(TryFindAssistantRoutine());
    }

    private IEnumerator TryFindAssistantRoutine()
    {
        _searching = true;
        for (int i = 0; i < findMaxTries && assistantT == null; i++)
        {
            var go = GameObject.FindGameObjectWithTag(assistantTag);
            if (go != null)
            {
                assistant = go;
                assistantT = go.transform;
                Debug.Log("[ControladorAsistente] Asistente encontrado por Tag: " + assistantTag);
                break;
            }
            yield return new WaitForSeconds(findRetrySeconds);
        }
        if (assistantT == null)
            Debug.LogWarning("[ControladorAsistente] No encontré al asistente (tag: " + assistantTag + "). ¿El spawner ya lo instanció?");
        _searching = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Solo si quien entra es la cámara / jugador
        if (!other.CompareTag("Player")) return;

        // Si aún no se encontró el asistente, aviso y salgo
        if (assistantT == null)
        {
            Debug.LogWarning("[ControladorAsistente] Trigger " + name + " activado, pero el asistente aún no existe.");
            return;
        }

        switch (gameObject.name)
        {
            case "Seccion los 70 Trigger":
                TeletransportarAsistente(seccion70s);
                Debug.Log("Asistente movido a la sección de los 70s");
                break;

            case "Seccion los 80 Trigger":
                TeletransportarAsistente(seccion80s);
                Debug.Log("Asistente movido a la sección de los 80s");
                break;

            case "Seccion los 90 Trigger":
                TeletransportarAsistente(seccion90s);
                Debug.Log("Asistente movido a la sección de los 90s");
                break;

            case "Seccion los 2000 Trigger":
                TeletransportarAsistente(seccion2000);
                Debug.Log("Asistente movido a la sección de los 2000s");
                break;

            case "Seccion los 2010 Trigger":
                TeletransportarAsistente(seccion2010);
                Debug.Log("Asistente movido a la sección de los 2010s");
                break;

            default:
                Debug.LogWarning("Trigger no reconocido: " + gameObject.name);
                break;
        }
    }

    private void TeletransportarAsistente(Transform target)
    {
        if (assistantT == null || target == null)
        {
            Debug.LogError("Assistant o Target son null en " + name);
            return;
        }

        assistantT.position = target.position;
        assistantT.rotation = target.rotation;

        Debug.Log("Asistente movido a: " + target.position);
    }
}

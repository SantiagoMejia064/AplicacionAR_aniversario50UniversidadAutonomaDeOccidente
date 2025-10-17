using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderQuiz : MonoBehaviour
{
    [Header("Tag de la cámara o jugador")]
    [SerializeField] private string cameraTag = "Player";

    [Header("Paneles de preguntas (en orden)")]
    [SerializeField] private GameObject[] quizPanels;

    private int currentIndex = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(cameraTag)) return;

        foreach (var panel in quizPanels)
        {
            if (panel != null) panel.SetActive(false);
        }

        // Muestra el panel actual si existe
        if (currentIndex < quizPanels.Length)
        {
            quizPanels[currentIndex].SetActive(true);
            currentIndex++;
        }
        else
        {
            Debug.Log("✅ Ya se mostraron todas las preguntas de esta sección.");
        }
    }
}

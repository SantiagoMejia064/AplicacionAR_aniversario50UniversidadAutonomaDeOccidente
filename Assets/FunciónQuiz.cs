using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FunciónQuiz : MonoBehaviour
{
    [Header("Botones de esta pregunta")]
    public Button[] optionButtons;
    public int correctIndex = 0;

    [Header("Flujo de paneles")]
    public GameObject panelActual;        // Este panel (pregunta actual)
    public GameObject panelSiguiente;     // Solo para la primera pregunta
    public bool esUltimaPregunta = false; // Marca true en la segunda pregunta

    private bool yaRespondido = false;

    void Start()
    {
        for (int i = 0; i < optionButtons.Length; i++)
        {
            int idx = i;
            optionButtons[i].onClick.AddListener(() => CheckAnswer(idx));
        }
    }

    void CheckAnswer(int index)
    {
        if (yaRespondido) return;
        yaRespondido = true;

        // Pinta la respuesta
        if (index == correctIndex)
            optionButtons[index].image.color = Color.green;
        else
        {
            optionButtons[index].image.color = Color.red;
            optionButtons[correctIndex].image.color = Color.green;
        }

        // Desactiva botones
        foreach (var b in optionButtons)
            b.interactable = false;

        // Espera un poco y cambia de panel o cierra
        Invoke(nameof(AvanzarFlujo), 0.8f);
    }

    void AvanzarFlujo()
    {
        if (esUltimaPregunta)
        {
            // Si es la última, desactiva todo
            panelActual.SetActive(false);
        }
        else
        {
            // Si no, pasa al siguiente panel
            panelActual.SetActive(false);
            panelSiguiente.SetActive(true);
        }
    }
}

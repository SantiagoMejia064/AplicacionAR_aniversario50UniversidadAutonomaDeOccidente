using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]

public class CirculoFinal : MonoBehaviour
{
    [SerializeField] string cameraTag = "MainCamera"; 
    [SerializeField] GameObject panelResultado; 
    [SerializeField] TMP_Text textoResumen;     
    [SerializeField] int totalPreguntas = 10;   

    bool fired = false;

    void Start()
    {
        if (panelResultado)
        {
            panelResultado.SetActive(false);
        }
    }

    void Reset(){ 
        GetComponent<Collider>().isTrigger = true; 
    }

    void OnTriggerEnter(Collider other)
    {
        if (fired) return;
        if (!other.CompareTag(cameraTag)) return;
        fired = true;

        if (textoResumen)
            textoResumen.text = $"Felicidades, tienes {PuntajeManager.TotalCorrectas} / {totalPreguntas} correctas, eres todo un Autónomo";

        if (panelResultado)
        {
            panelResultado.SetActive(true);
        }
    }
}

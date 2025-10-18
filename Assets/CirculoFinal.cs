using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Collider))]

public class CirculoFinal : MonoBehaviour
{
    [SerializeField] string cameraTag = "Player"; 
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
            textoResumen.text = $"Correctas: {PuntajeManager.TotalCorrectas} / {totalPreguntas}";

        if (panelResultado)
        {
            panelResultado.SetActive(true);
        }
    }
}

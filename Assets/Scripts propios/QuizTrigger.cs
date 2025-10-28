using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    public GameObject panelPreguntas;
    public string playerTag = "MainCamera";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag)) panelPreguntas.SetActive(true);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag)) panelPreguntas.SetActive(false);
    }
}

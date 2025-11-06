using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideosManager : MonoBehaviour
{
    public List<GameObject> videosList = new List<GameObject>();
    public int currentVideoIndex = 0;

    [Header("Secciones de Videos")]
    public GameObject seccion80s;
    public GameObject seccion90s;
    public GameObject seccion2000s;
    public GameObject seccion2010s;

    [Header("Botones de activacion de secciones")]
    public GameObject boton80s;
    public GameObject boton90s;
    public GameObject boton2000s;
    public GameObject boton2010s;

    public void avanzarVideo()
    {
        if (currentVideoIndex < videosList.Count - 1)
        {
            videosList[currentVideoIndex].SetActive(false);
            currentVideoIndex++;
            videosList[currentVideoIndex].SetActive(true);
        }
    }

    public void retrocederVideo()
    {
        if (currentVideoIndex > 0)
        {
            videosList[currentVideoIndex].SetActive(false);
            currentVideoIndex--;
            videosList[currentVideoIndex].SetActive(true);
        }
    }

    public void activarSeccion(string seccion)
    {
        // Activar la seccion correspondiente
        switch (seccion)
        {
            case "80s":
                seccion80s.SetActive(true);
                break;
            case "90s":
                seccion90s.SetActive(true);
                break;
            case "2000s":
                seccion2000s.SetActive(true);
                break;
            case "2010s":
                seccion2010s.SetActive(true);
                break;
            default:
                Debug.LogWarning("Sección no reconocida: " + seccion);
                break;
        }
    }

    public void desactivarSeccion(string seccion)
    {
        // Desactivar la seccion correspondiente
        switch (seccion)
        {
            case "80s":
                seccion80s.SetActive(false);
                boton80s.SetActive(false);
                break;
            case "90s":
                seccion90s.SetActive(false);
                boton90s.SetActive(false);
                break;
            case "2000s":
                seccion2000s.SetActive(false);
                boton2000s.SetActive(false);
                break;
            case "2010s":
                seccion2010s.SetActive(false);
                boton2010s.SetActive(false);
                break;
            default:
                Debug.LogWarning("Sección no reconocida: " + seccion);
                break;
        }
    }
}

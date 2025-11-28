using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video; 
using UnityEngine.UI; 

public class VideosManager : MonoBehaviour
{
    public List<GameObject> videosList = new List<GameObject>();
    public int currentVideoIndex = 0;

    // =========================================================
    // REFERENCIAS DE AUDIO Y UI
    // =========================================================
    [Header("Gestión de Audio")]
    public AudioManager audioManager; 

    [Header("Control de Tiempo de Video")]
    public Slider videoSeekBar; 
    public Text timeDisplay;   
    
    // Bandera para saber si el usuario está arrastrando la barra
    private bool isSeeking = false;
    
    // =========================================================
    // VARIABLES EXISTENTES
    // =========================================================
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
    
    // =========================================================
    // CICLO DE VIDA (Update)
    // =========================================================
    
    void Update()
    {
        UpdateVideoProgress();
    }
    
    // =========================================================
    // SINCRONIZACIÓN DE LA BARRA DE TIEMPO (SEEK BAR)
    // =========================================================
    
    private void UpdateVideoProgress()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();

        if (vp != null && vp.isPrepared && videoSeekBar != null)
        {
            double duration = vp.length;
            double currentTime = vp.time;

            // 1. Establecer el valor máximo del Slider (duración total)
            videoSeekBar.maxValue = (float)duration;
            
            // 2. MANTENEMOS EL SLIDER QUIETO (SOLUCIÓN PROBADA DE AUDIO)
            // Solo actualizamos el valor si el usuario está buscando (isSeeking = true)
            // o si el video está pausado (para que el valor se mantenga).
            // Si !isSeeking, no hacemos nada para evitar el stutter.

            // 3. (Opcional) Actualizar la visualización de tiempo
            if (timeDisplay != null)
            {
                timeDisplay.text = FormatTime(currentTime) + " / " + FormatTime(duration);
            }
        }
    }
    
    // Este método se conecta al evento 'On Value Changed' del componente Slider
    public void OnSeekBarValueChange()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();

        // Solo cambia la posición si el video está preparado
        if (vp != null && vp.isPrepared)
        {
            // Establece la posición de reproducción del video al valor actual del Slider
            vp.time = videoSeekBar.value;
        }
    }
    
    // --- Métodos para manejar el arrastre del Slider (UX) ---
    public void StartSeeking()
    {
        isSeeking = true;
    }

    public void EndSeeking()
    {
        isSeeking = false;
    }

    // --- Método auxiliar para formatear segundos a mm:ss ---
    private string FormatTime(double timeInSeconds)
    {
        int minutes = Mathf.FloorToInt((float)timeInSeconds / 60);
        int seconds = Mathf.FloorToInt((float)timeInSeconds % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    // =========================================================
    // MÉTODOS DE NAVEGACIÓN (INICIO AUTOMÁTICO Y CONTROL DE MÚSICA)
    // =========================================================

    public void avanzarVideo()
    {
        GetCurrentVideoPlayer()?.Stop(); 

        if (currentVideoIndex < videosList.Count - 1)
        {
            videosList[currentVideoIndex].SetActive(false);
            currentVideoIndex++;
            videosList[currentVideoIndex].SetActive(true);
            
            IniciarVideoAutomatico();
        }
    }

    public void retrocederVideo()
    {
        GetCurrentVideoPlayer()?.Stop(); 
        
        if (currentVideoIndex > 0)
        {
            videosList[currentVideoIndex].SetActive(false);
            currentVideoIndex--;
            videosList[currentVideoIndex].SetActive(true);
            
            IniciarVideoAutomatico();
        }
    }
    
    private void IniciarVideoAutomatico()
    {
        VideoPlayer nuevoVp = GetCurrentVideoPlayer();
        
        if (nuevoVp != null)
        {
            // 1. Resetear el Slider y el tiempo para que siempre inicie en 0
            if (videoSeekBar != null)
            {
                videoSeekBar.value = 0f;
            }
            nuevoVp.time = 0; 
            
            // 2. PAUSAR LA MÚSICA DE FONDO
            audioManager?.PausarMusicaFondo(); 
            
            // 3. Iniciar la reproducción
            nuevoVp.Play();
        }
    }

    // =========================================================
    // MÉTODOS DE CONTROL DE BOTONES (Play/Pause)
    // =========================================================

    public void PlayVideo()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();
        
        if (vp != null && !vp.isPlaying)
        {
            audioManager?.PausarMusicaFondo();
            vp.Play();
        }
    }

    public void PauseVideo()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();
        
        if (vp != null && vp.isPlaying)
        {
            vp.Pause();
            audioManager?.ReanudarMusicaFondo();
        }
    }
    
    // =========================================================
    // FUNCIÓN DE LIMPIEZA AL SALIR DE LA SECCIÓN
    // =========================================================
    
    public void LimpiarVideosYReanudarMusica()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();
        
        if (vp != null && vp.isPlaying)
        {
            vp.Stop();
        }

        audioManager?.ReanudarMusicaFondo();
    }

    // =========================================================
    // MÉTODOS AUXILIARES Y EXISTENTES
    // =========================================================

    private VideoPlayer GetCurrentVideoPlayer()
    {
        if (videosList.Count > 0 && currentVideoIndex >= 0 && currentVideoIndex < videosList.Count)
        {
            GameObject currentVideoObject = videosList[currentVideoIndex];
            VideoPlayer vp = currentVideoObject.GetComponent<VideoPlayer>();
            
            if (vp == null)
            {
                Debug.LogError($"El objeto de video {currentVideoObject.name} no tiene un componente VideoPlayer.");
            }
            return vp;
        }
        return null;
    }
    
    public void activarSeccion(string seccion)
    {
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
        
        // LLAMADA CLAVE: Inicia el video actual y pausa la música inmediatamente
        IniciarVideoAutomatico(); 
    }

    public void desactivarSeccion(string seccion)
    {
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
        
        LimpiarVideosYReanudarMusica(); 
    }
}
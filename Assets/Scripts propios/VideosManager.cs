using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;

public class VideosManager : MonoBehaviour
{
    // =========================================================
    // REFERENCIAS Y VARIABLES CLAVE
    // =========================================================
    
    // Variables antiguas (se mantienen por compatibilidad)
    public List<GameObject> videosList = new List<GameObject>(); 
    public int currentVideoIndex = 0; 

    [Header("Control de Video Actual")]
    [Tooltip("El objeto de video (con VideoPlayer) actualmente seleccionado por el Trigger.")]
    private GameObject currentVideoObject = null; // El objeto de video activo.

    [Header("Control de la Interfaz")]
    [Tooltip("El GameObject padre que contiene toda la UI del reproductor (Panel, Canvas Group, etc.)")]
    public GameObject videoUIPanel; // Para hacer visible el reproductor completo

    [Header("Referencia del Botón Activador")]
    [Tooltip("El botón que se debe ocultar al iniciar la reproducción.")]
    private GameObject currentActivatorButton = null; // ¡Nueva variable para el botón!

    [Header("Gestión de Audio")]
    public AudioManager audioManager;

    [Header("Control de Tiempo de Video")]
    public Slider videoSeekBar;
    public Text timeDisplay;
    
    private bool isSeeking = false;
    
    // Variables antiguas de secciones (se mantienen por compatibilidad)
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
    // CICLO DE VIDA Y SINCRONIZACIÓN
    // =========================================================
    void Update()
    {
        UpdateVideoProgress();
    }
    
    // =========================================================
    // MÉTODOS DE CONTROL DE SELECCIÓN Y BOTÓN (¡AJUSTADOS!)
    // =========================================================
    
    // 1. Registra el botón que debe ocultarse/mostrarse
    public void RegisterActivatorButton(GameObject buttonObject)
    {
        currentActivatorButton = buttonObject;
    }

    // 2. Establece el objeto de video que se va a reproducir
    public void SetCurrentVideoObject(GameObject videoObject)
    {
        if (videoObject == null) return;
        
        // Si ya hay un video activo diferente, lo desactivamos
        if (currentVideoObject != null && currentVideoObject != videoObject)
        {
            currentVideoObject.SetActive(false);
        }
        
        // Establece el nuevo objeto
        currentVideoObject = videoObject;
        
        // Activa el GameObject del video (para que esté listo para el Render Texture)
        currentVideoObject.SetActive(true); 
    }

    // =========================================================
    // REPRODUCCIÓN (PÚBLICA PARA EL BOTÓN)
    // =========================================================
    
    public void IniciarVideoAutomatico() 
    {
        VideoPlayer nuevoVp = GetCurrentVideoPlayer();

        if (nuevoVp != null)
        {
            // 1. OCULTAR EL BOTÓN QUE LO ACTIVÓ
            if (currentActivatorButton != null)
            {
                currentActivatorButton.SetActive(false);
            }

            // 2. ACTIVAR EL PANEL PADRE DE LA UI (Hace visible el reproductor)
            if (videoUIPanel != null)
            {
                videoUIPanel.SetActive(true);
            }

            // 3. Resetear el Slider y el tiempo
            if (videoSeekBar != null)
            {
                videoSeekBar.value = 0f;
            }
            nuevoVp.time = 0;

            // 4. Pausar la música de fondo e iniciar la reproducción
            audioManager?.PausarMusicaFondo();
            nuevoVp.Play();
        }
    }
    
    // =========================================================
    // LIMPIEZA AL SALIR (¡AJUSTADO PARA REAPERTURA!)
    // =========================================================
    public void LimpiarVideosYReanudarMusica()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();

        if (vp != null)
        {
            vp.Stop();
        }
        
        // 1. Mostrar el botón de nuevo (si estamos en el Collider)
        if (currentActivatorButton != null)
        {
            currentActivatorButton.SetActive(true);
        }

        // 2. Desactivar el GameObject del video en el mundo
        if (currentVideoObject != null)
        {
             currentVideoObject.SetActive(false);
        }

        // 3. DESACTIVAR EL PANEL PADRE DE LA UI
        if (videoUIPanel != null)
        {
            videoUIPanel.SetActive(false);
        }

        audioManager?.ReanudarMusicaFondo();
        
        // ¡IMPORTANTE! NO ponemos currentVideoObject = null, para mantener la referencia 
        // y permitir que el usuario haga clic de nuevo sin salir del trigger.
    }
    
    // =========================================================
    // MÉTODOS AUXILIARES
    // =========================================================

    private VideoPlayer GetCurrentVideoPlayer()
    {
        if (currentVideoObject != null)
        {
            VideoPlayer vp = currentVideoObject.GetComponent<VideoPlayer>();
            if (vp == null)
            {
                Debug.LogError($"El objeto {currentVideoObject.name} no tiene un componente VideoPlayer.");
            }
            return vp;
        }
        return null;
    }
    
    private void UpdateVideoProgress()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();

        if (vp != null && vp.isPrepared && videoSeekBar != null)
        {
            double duration = vp.length;
            double currentTime = vp.time;

            videoSeekBar.maxValue = (float)duration;
            
            if (vp.isPlaying == false || isSeeking == true)
            {
                videoSeekBar.value = (float)currentTime;
            }

            if (timeDisplay != null)
            {
                timeDisplay.text = FormatTime(currentTime) + " / " + FormatTime(duration);
            }
        }
    }

    public void OnSeekBarValueChange()
    {
        VideoPlayer vp = GetCurrentVideoPlayer();
        if (vp != null && vp.isPrepared)
        {
            vp.time = videoSeekBar.value;
        }
    }

    public void StartSeeking() { isSeeking = true; }
    public void EndSeeking() { isSeeking = false; }
    
    private string FormatTime(double timeInSeconds)
    {
        int minutes = Mathf.FloorToInt((float)timeInSeconds / 60);
        int seconds = Mathf.FloorToInt((float)timeInSeconds % 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }
    
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
    
    // Funciones de navegación (Si se usan los botones avanzar/retroceder)
    public void avanzarVideo()
    {
        // Esta lógica DEBE USAR la lista videosList configurada en el Inspector
        // y debe llamar a SetCurrentVideoObject(videosList[nuevoIndice])
        // y luego a IniciarVideoAutomatico().
    }

    public void retrocederVideo()
    {
        // Esta lógica DEBE USAR la lista videosList configurada en el Inspector
        // y debe llamar a SetCurrentVideoObject(videosList[nuevoIndice])
        // y luego a IniciarVideoAutomatico().
    }
    
    // Funciones antiguas de secciones (dejadas por si activan otra UI)
    public void activarSeccion(string seccion)
    {
        // Lógica de activación de secciones
    }

    public void desactivarSeccion(string seccion)
    {
        // Lógica de desactivación de secciones
        LimpiarVideosYReanudarMusica(); 
    }
}
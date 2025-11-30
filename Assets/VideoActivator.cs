using UnityEngine;
using UnityEngine.UI;

public class VideoActivator : MonoBehaviour
{
    [Header("Referencias")]
    [Tooltip("El GameObject del botón 'Ver Video' que se debe activar")]
    public GameObject videoButton;

    [Tooltip("El script VideosManager en tu escena")]
    public VideosManager videosManager;
    
    [Header("Video a Reproducir")]
    [Tooltip("Arrastra aquí el GameObject de la jerarquía que contiene el VideoPlayer que debe reproducirse.")]
    public GameObject videoObjectToPlay;

    // Necesitamos el tag del jugador para detectar la colisión
    public string playerTag = "MainCamera";

    private void Start()
    {
        // Si no se asigna en el Inspector, intentamos encontrarlo
        if (videosManager == null)
        {
            videosManager = FindObjectOfType<VideosManager>();
            if (videosManager == null)
            {
                Debug.LogError("VideoActivator: No se encontró el script VideosManager.");
            }
        }

        // El botón debe empezar desactivado
        if (videoButton != null)
        {
            videoButton.SetActive(false);
        }
        
        // **IMPORTANTE**: Asegúrate de que el objeto del video esté inactivo al inicio.
        if (videoObjectToPlay != null)
        {
            videoObjectToPlay.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // 1. Establecer el video Object
            if (videosManager != null && videoObjectToPlay != null)
            {
                // Llama a la función que establece la referencia DIRECTA
                videosManager.SetCurrentVideoObject(videoObjectToPlay); 
                
                // 2. REGISTRAR EL BOTÓN con el manager para que pueda ocultarlo/mostrarlo
                if (videoButton != null)
                {
                    videosManager.RegisterActivatorButton(videoButton);
                }
            }

            // 3. Activar el botón de UI
            if (videoButton != null)
            {
                // Si el video NO está reproduciéndose, activamos el botón para que el usuario pueda presionarlo.
                if (videosManager != null && (videosManager.videoUIPanel == null || !videosManager.videoUIPanel.activeSelf))
                {
                     videoButton.SetActive(true);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // 4. Desactivar el botón de UI al salir
            if (videoButton != null)
            {
                videoButton.SetActive(false);
            }
            
            // 5. Opcional: Desregistrar el botón para limpiar la referencia
            if (videosManager != null)
            {
                videosManager.RegisterActivatorButton(null);
            }
        }
    }
}
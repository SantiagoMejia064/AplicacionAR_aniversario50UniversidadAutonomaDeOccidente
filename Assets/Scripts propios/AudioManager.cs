using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Configuración de Sonidos")]
    // Arrastra aquí el sonido corto de aparición
    public AudioClip appearSoundEffect;
    
    // Arrastra aquí el AudioSource de la música de fondo
    public AudioSource backgroundMusicSource;

    [Header("Etiqueta a Vigilar")]
    // Escribe la etiqueta del objeto que esperaremos
    public string tagToWatchFor = "MainEstructure";

    // Componentes de Audio
    private AudioSource soundEffectSource;
    
    // Control para que solo suene una vez
    private bool soundsHavePlayed = false;

    void Start()
    {
        // Añadimos y configuramos el AudioSource para efectos de sonido
        soundEffectSource = gameObject.AddComponent<AudioSource>();
        
        // Asegura que la música de fondo esté configurada como bucle (loop)
        if (backgroundMusicSource != null)
        {
            backgroundMusicSource.loop = true;
        }
    }

    void Update()
    {
        // Si los sonidos ya sonaron, no hagas nada más.
        if (soundsHavePlayed)
        {
            return;
        }

        // Buscamos en la escena si ya existe un objeto con la etiqueta especificada
        GameObject targetObject = GameObject.FindWithTag(tagToWatchFor);

        // Si el objeto fue encontrado...
        if (targetObject != null)
        {
            Debug.Log("¡Objeto encontrado! Reproduciendo sonidos.");

            // 1. Reproduce el sonido de aparición una vez
            if (appearSoundEffect != null)
            {
                soundEffectSource.PlayOneShot(appearSoundEffect);
            }

            // 2. Inicia la música de fondo
            if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }

            // 3. Marcamos que ya sonaron para no volver a ejecutar esto
            soundsHavePlayed = true;
        }
    }

    // =========================================================
    // MÉTODOS DE CONTROL DE MÚSICA PARA VIDEOS
    // =========================================================

    public void PausarMusicaFondo()
    {
        if (backgroundMusicSource != null && backgroundMusicSource.isPlaying)
        {
            // Pausa la música
            backgroundMusicSource.Pause();
            Debug.Log("Música de fondo pausada.");
        }
    }

    public void ReanudarMusicaFondo()
    {
        if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
        {
            // Reanuda la música solo si no está reproduciendo
            backgroundMusicSource.Play();
            Debug.Log("Música de fondo reanudada.");
        }
    }
}
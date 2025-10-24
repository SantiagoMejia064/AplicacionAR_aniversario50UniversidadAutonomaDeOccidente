using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Configuraci�n de Sonidos")]
    // Arrastra aqu� el sonido corto de aparici�n
    public AudioClip appearSoundEffect;
    // Arrastra aqu� la m�sica de fondo
    //public AudioClip backgroundMusic;

    [Header("Etiqueta a Vigilar")]
    // Escribe la etiqueta del objeto que esperaremos
    public string tagToWatchFor = "MainEstructure";

    // Componentes de Audio
    private AudioSource soundEffectSource;
    public AudioSource backgroundMusicSource;

    // Control para que solo suene una vez
    private bool soundsHavePlayed = false;

    void Start()
    {
        // A�adimos y configuramos los dos AudioSources necesarios
        soundEffectSource = gameObject.AddComponent<AudioSource>();
        //backgroundMusicSource = gameObject.AddComponent<AudioSource>();

        // Configura la m�sica de fondo para que sea en bucle
        backgroundMusicSource.loop = true;
        //backgroundMusicSource.clip = backgroundMusic;
    }

    void Update()
    {
        // Si los sonidos ya sonaron, no hagas nada m�s.
        if (soundsHavePlayed)
        {
            return;
        }

        // Buscamos en la escena si ya existe un objeto con la etiqueta especificada
        GameObject targetObject = GameObject.FindWithTag(tagToWatchFor);

        // Si el objeto fue encontrado...
        if (targetObject != null)
        {
            Debug.Log("�Objeto encontrado! Reproduciendo sonidos.");

            // 1. Reproduce el sonido de aparici�n una vez
            if (appearSoundEffect != null)
            {
                soundEffectSource.PlayOneShot(appearSoundEffect);
            }

            // 2. Inicia la m�sica de fondo
            if (backgroundMusicSource != null && !backgroundMusicSource.isPlaying)
            {
                backgroundMusicSource.Play();
            }

            // 3. Marcamos que ya sonaron para no volver a ejecutar esto
            soundsHavePlayed = true;
        }
    }
}
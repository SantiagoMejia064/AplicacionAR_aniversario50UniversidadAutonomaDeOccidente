using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkButtonHandler : MonoBehaviour
{
    public Button youtubeButton;
    public Button spotifyButton;

    void Start()
    {
        // Vinculamos los botones a sus respectivas funciones
        youtubeButton.onClick.AddListener(OpenYouTubeChannel);
        spotifyButton.onClick.AddListener(OpenSpotifyChannel);
    }

    // Método para abrir el canal de YouTube
    void OpenYouTubeChannel()
    {
        Application.OpenURL("https://youtube.com/@uao50anos?si=i6LLRSqTMp1uUZtY");
    }

    // Método para abrir el canal de Spotify
    void OpenSpotifyChannel()
    {
        Application.OpenURL("https://open.spotify.com/show/0Ju0hSYZcX5E0apYbCnury");
    }
}
using System;
using System.Collections;
using System.IO;
using UnityEngine;

public class PhotoBoothController : MonoBehaviour
{
    [Header("Capture Settings")]
    [Tooltip("Camera used to capture the photo")]
    public Camera photoCamera;

    [Tooltip("RenderTexture to render the capture into (set resolution in inspector)")]
    public RenderTexture photoRT;

    [Header("Storage Settings")]
    [Tooltip("Folder name inside My Documents where photos are saved")]
    public string folderName = "FOTOS_UNITY_50AÑOS";

    [Header("Referencia al mural 3D")]
    public PhotoWall3D photoWall;   // <- arrastra aquí tu objeto con el script PhotoWall3D

    [Header("Audio Settings")]
    public AudioClip shutterSound;   // arrastra aquí tu sonido de cámara
    public float volume = 1f;


    // full path resolved at Start
    private string photosFolderPath;

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        photosFolderPath = Path.Combine(Application.persistentDataPath, folderName);
#else
        photosFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), folderName);
#endif
        if (!Directory.Exists(photosFolderPath)) Directory.CreateDirectory(photosFolderPath);
    }

    // Hook this to your button OnClick
    public void PublicOnTakePhotoButton()
    {
        // 👉 reproducir sonido antes de tomar la foto
        if (shutterSound != null)
        {
            AudioSource.PlayClipAtPoint(shutterSound, transform.position, volume);
        }

        StartCoroutine(CaptureAndSaveCoroutine());
    }

    private IEnumerator CaptureAndSaveCoroutine()
    {
        if (!Directory.Exists(photosFolderPath))
        {
            try { Directory.CreateDirectory(photosFolderPath); }
            catch (Exception e)
            {
                Debug.LogError("No se pudo crear la carpeta: " + e.Message);
                yield break;
            }
        }

        if (photoCamera == null)
        {
            Debug.LogError("PhotoBoothController: photoCamera is null.");
            yield break;
        }

        if (photoRT == null)
        {
            Debug.LogError("PhotoBoothController: photoRT is null. Assign a RenderTexture in the inspector.");
            yield break;
        }

        RenderTexture previousRT = photoCamera.targetTexture;
        photoCamera.targetTexture = photoRT;

        yield return new WaitForEndOfFrame();

        RenderTexture.active = photoRT;
        Texture2D tex = new Texture2D(photoRT.width, photoRT.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, photoRT.width, photoRT.height), 0, 0);
        tex.Apply();

        byte[] pngBytes = tex.EncodeToPNG();

        UnityEngine.Object.Destroy(tex);
        RenderTexture.active = null;
        photoCamera.targetTexture = previousRT;

        string timestamp = DateTime.Now.ToString("HHmmss_yyyyMMdd");
        string fileName = $"50AÑOS_FOTO_{timestamp}.png";
        string fullPath = Path.Combine(photosFolderPath, fileName);

        try { File.WriteAllBytes(fullPath, pngBytes); }
        catch (Exception e)
        {
            Debug.LogError("Error saving photo: " + e.Message);
            yield break;
        }

        Debug.Log($"Guardada {fileName}_ en la carpeta {photosFolderPath}");

        // 👉 Añadir la foto al mural en tiempo real
        if (photoWall != null)
        {
            photoWall.AddPhotoToWall(fullPath);
        }
    }
}

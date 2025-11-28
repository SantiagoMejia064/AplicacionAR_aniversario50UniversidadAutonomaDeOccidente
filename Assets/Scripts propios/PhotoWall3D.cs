using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class PhotoWall3D : MonoBehaviour
{
    [Header("Prefab del marco 3D")]
    public GameObject photoFramePrefab;

    [Header("Carpeta de fotos")]
    public string folderName = "FOTOS_UNITY_50AÑOS";

    [Header("Distribución")]
    public int columns = 5;
    public float spacing = 1.5f;

    [Header("Refresco automático (segundos)")]
    public float refreshInterval = 2f;

    private string photosFolder;
    private HashSet<string> loadedFiles = new HashSet<string>();

    void Start()
    {
#if UNITY_ANDROID || UNITY_IOS
        photosFolder = Path.Combine(Application.persistentDataPath, folderName);
#else
        photosFolder = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments), folderName);
#endif
        if (!Directory.Exists(photosFolder))
        {
            Directory.CreateDirectory(photosFolder);
        }

        // Cargar fotos iniciales
        RefreshWall();

        // Iniciar refresco periódico
        InvokeRepeating(nameof(RefreshWall), refreshInterval, refreshInterval);
    }

    void RefreshWall()
    {
        string[] files = Directory.GetFiles(photosFolder, "*.png");

        foreach (string file in files)
        {
            if (!loadedFiles.Contains(file))
            {
                AddPhotoToWall(file);
                loadedFiles.Add(file);
            }
        }
    }

    public void AddPhotoToWall(string path)
    {
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D tex = new Texture2D(2, 2);
        tex.LoadImage(bytes);

        GameObject frame = Instantiate(photoFramePrefab, transform);

        Renderer rend = frame.GetComponent<Renderer>();
        rend.material.mainTexture = tex;

        int i = transform.childCount - 1; // índice actual
        int row = i / columns;
        int col = i % columns;
        frame.transform.localPosition = new Vector3(col * spacing, -row * spacing, 0);

        Debug.Log("Foto añadida al mural: " + Path.GetFileName(path));
    }
}
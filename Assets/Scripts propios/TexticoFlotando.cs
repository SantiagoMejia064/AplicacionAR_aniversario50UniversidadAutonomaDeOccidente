using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexticoFlotando : MonoBehaviour
{
    [Header("Movimiento flotante")]
    public float amplitude = 0.1f;
    public float speed = 1f;

    [Header("Billboard (seguir cámara)")]
    public bool soloHorizontal = false; // Si quieres que solo gire en el eje Y

    private Vector3 localStartPos;
    private Transform cameraTransform;

    void Start()
    {
        localStartPos = transform.localPosition;
        cameraTransform = Camera.main.transform; // Obtiene la cámara principal
    }

    void LateUpdate()
    {
        // --- Movimiento flotante ---
        if (amplitude > 0f)
        {
            float offset = Mathf.Sin(Time.time * speed) * amplitude;
            transform.localPosition = localStartPos + Vector3.up * offset;
        }

        // --- Billboard (seguir cámara) ---
        if (soloHorizontal)
        {
            // Solo rota en el eje Y
            Vector3 direction = transform.position - cameraTransform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            // Mira completamente hacia la cámara
            transform.LookAt(transform.position + cameraTransform.forward);
        }
    }
}




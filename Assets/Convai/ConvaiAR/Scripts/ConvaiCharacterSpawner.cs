using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// Activa un contenido hijo (escena completa) en AR en lugar de instanciar prefabs.
/// - Tap en plano: mueve y activa el root de contenido en la pose del hit.
/// - Tracked Image: hace child del tracked image y activa el contenido.
/// </summary>
public class ConvaiCharacterSpawner : MonoBehaviour
{
    [Header("Contenido a activar (hijo de Convai AR Player)")]
    [SerializeField] private GameObject contentRoot;       // EscenaCompletaRoot (inactive al iniciar)
    [SerializeField] private bool alignToHitRotation = true;
    [SerializeField] private Vector3 placementOffset = new Vector3(0f, 0f, 0f);

    [Header("AR Managers (se resuelven solos si los dejas en null)")]
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] private ARPlaneManager _arPlaneManager;
    [SerializeField] private ARTrackedImageManager _arTrackedImageManager;

    [Header("Spawning / Activación")]
    [SerializeField] private bool allowMultipleActivations = false; // normalmente false: solo una activación
    private bool _isActiveOnce = false;

    [SerializeField] private bool _isSpawnModeActive = true; // modo de colocación activo

    public Action OnContentActivated;

    private void Awake()
    {
        if (_arRaycastManager == null) _arRaycastManager = FindObjectOfType<ARRaycastManager>();
        if (_arPlaneManager == null) _arPlaneManager = FindObjectOfType<ARPlaneManager>();
        if (_arTrackedImageManager == null) _arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();

        if (contentRoot == null)
            Debug.LogError("[Spawner] Asigna el 'contentRoot' (tu escena completa desactivada).");
        else
            contentRoot.SetActive(false); // aseguramos que arranca inactivo
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnTouch;
        if (_arTrackedImageManager != null)
            _arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnTouch;
        EnhancedTouchSupport.Disable();
        if (_arTrackedImageManager != null)
            _arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTouch(Finger finger)
    {
        if (!_isSpawnModeActive) return;
        if (IsPointerOverUI(finger)) return;
        if (contentRoot == null) return;

        // Si solo permitimos una activación y ya está activo, ignoramos taps
        if (!allowMultipleActivations && _isActiveOnce) return;

        List<ARRaycastHit> hits = new();
        if (_arRaycastManager != null &&
            _arRaycastManager.Raycast(finger.screenPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var pose = hits[0].pose;

            // posicionamos el contenido
            contentRoot.transform.position = pose.position + placementOffset;
            if (alignToHitRotation) contentRoot.transform.rotation = pose.rotation;

            // activamos
            ActivateContentOnce();

            // opcional: apagar detección de planos para limpiar la escena
            TogglePlaneDetection(false);

            Debug.Log("<color=green>[Spawner] Contenido activado en plano.</color>");
        }
        else
        {
            Debug.Log("<color=yellow>[Spawner] No se encontró plano bajo el toque.</color>");
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs args)
    {
        if (_arTrackedImageManager == null || contentRoot == null) return;

        foreach (var added in args.added)
        {
            if (!allowMultipleActivations && _isActiveOnce) break;

            // Parentea el contenido a la imagen y alínealo
            contentRoot.transform.SetParent(added.transform, worldPositionStays: false);
            contentRoot.transform.localPosition = placementOffset;
            if (alignToHitRotation) contentRoot.transform.localRotation = Quaternion.identity;

            ActivateContentOnce();
            TogglePlaneDetection(false);

            Debug.Log("<color=green>[Spawner] Contenido activado sobre Tracked Image.</color>");
        }

        // Si quisieras mover el contenido cuando la imagen actualiza:
        // foreach (var updated in args.updated) { ... }
    }

    private void ActivateContentOnce()
    {
        if (contentRoot.activeSelf == false) contentRoot.SetActive(true);
        _isActiveOnce = true;
        OnContentActivated?.Invoke();
    }

    private void TogglePlaneDetection(bool enable)
    {
        if (_arPlaneManager == null) return;

        // Activa/Desactiva mallas de planos
        _arPlaneManager.SetTrackablesActive(enable);
        _arPlaneManager.enabled = enable;
    }

    private bool IsPointerOverUI(Finger finger)
    {
        if (EventSystem.current == null) return false;
        var data = new PointerEventData(EventSystem.current) { position = finger.screenPosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(data, results);
        return results.Count > 0;
    }

    public void SetSpawnMode(bool value)
    {
        _isSpawnModeActive = value;
        TogglePlaneDetection(value); // si apagas el modo, oculto/desactivo planos
    }
}
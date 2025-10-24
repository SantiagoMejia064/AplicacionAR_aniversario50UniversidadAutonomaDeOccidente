using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

/// <summary>
/// Spawns and manages Convai characters in AR space.
/// </summary>
public class ConvaiCharacterSpawner : MonoBehaviour
{
    // Prefab for the Convai character
    [SerializeField] private GameObject _characterPrefab;

    // Reference to the content root (empty GameObject that you want to activate later)
    [SerializeField] private GameObject _additionalContentRoot; // GameObject that will hold your models and scene elements

    private ARRaycastManager _arRaycastManager;
    private ARPlaneManager _arPlaneManager;
    private ARTrackedImageManager _arTrackedImageManager;

    // Flag to determine if character spawn mode is active
    private bool _isSpawnModeActive = true;

    // Flag indicating whether the maximum character count has been reached
    private bool _isMaxCharacterCountReached;

    // Counter for the number of characters spawned
    private int _spawnedCharacterCount;

    // Maximum number of characters allowed to be spawned
    private const int MAX_CHARACTER_COUNT = 5;

    // Event triggered when a character is spawned
    public Action OnCharacterSpawned;

    /// <summary>
    /// Initializes necessary components and finds AR-related managers.
    /// </summary>
    private void Awake()
    {
        _arRaycastManager = FindObjectOfType<ARRaycastManager>();
        _arPlaneManager = FindObjectOfType<ARPlaneManager>();
        _arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += ConvaiInputManager_OnTouchScreen;
        _arTrackedImageManager.trackedImagesChanged += ARTrackedImageManager_TrackedImagesChanged;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= ConvaiInputManager_OnTouchScreen;
        EnhancedTouchSupport.Disable();
        _arTrackedImageManager.trackedImagesChanged -= ARTrackedImageManager_TrackedImagesChanged;
    }

    /// <summary>
    /// Handles touch screen events to place characters on planes.
    /// </summary>
    private void ConvaiInputManager_OnTouchScreen(Finger finger)
    {
        if (IsPointerOverUIObject(finger)) return;
        if (!_isSpawnModeActive) return;
        if (_isMaxCharacterCountReached) return;
        TryPlaceCharacterOnPlane(finger.screenPosition);
    }

    /// <summary>
    /// Handles tracked image changes, spawning characters on added images.
    /// </summary>
    private void ARTrackedImageManager_TrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            // Handle Added Event
            GameObject character = SpawnCharacter();
            character.transform.parent = trackedImage.transform;
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            // Handle Update Event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle Removed Event
        }
    }

    /// <summary>
    /// Tries to place a character on a detected AR plane at the specified screen position.
    /// </summary>
    private void TryPlaceCharacterOnPlane(Vector2 touchPosition)
    {
        List<ARRaycastHit> _results = new List<ARRaycastHit>();
        if (_arRaycastManager.Raycast(touchPosition, _results, TrackableType.PlaneWithinPolygon))
        {
            GameObject character = SpawnCharacter();
            character.transform.position = _results[0].pose.position;

            // Activating additional content (your empty GameObject that holds the models)
            ActivateAdditionalContent(); // New line added
            Debug.Log("<color=green> Character Spawned! </color>");
        }
        else
        {
            Debug.Log("<color=red> Character Did Not Spawn! </color>");
        }
    }

    /// <summary>
    /// Spawns a character and updates related counters and flags.
    /// </summary>
    private GameObject SpawnCharacter()
    {
        if (_isMaxCharacterCountReached) return null;

        GameObject character = Instantiate(_characterPrefab);
        _spawnedCharacterCount++;
        if (_spawnedCharacterCount == MAX_CHARACTER_COUNT)
        {
            _isMaxCharacterCountReached = true;
            TogglePlaneManager(false);
        }

        OnCharacterSpawned?.Invoke();
        return character;
    }

    /// <summary>
    /// Activates the additional content (empty GameObject that contains the models).
    /// </summary>
    private void ActivateAdditionalContent()
    {
        if (_additionalContentRoot != null)
        {
            _additionalContentRoot.SetActive(true); // Activates your empty GameObject with models
            Debug.Log("[ActivateAdditionalContent] Additional content activated.");
        }
        else
        {
            Debug.LogWarning("[ActivateAdditionalContent] _additionalContentRoot is not assigned!");
        }
    }

    /// <summary>
    /// Toggles the AR Plane Manager to activate or deactivate AR planes.
    /// </summary>
    private void TogglePlaneManager(bool value)
    {
        _arPlaneManager.SetTrackablesActive(value);
        _arPlaneManager.enabled = value;
    }

    /// <summary>
    /// Sets the spawn mode, enabling or disabling character spawning.
    /// </summary>
    public void SetSpawnMode(bool value)
    {
        _isSpawnModeActive = value;

        if (_isSpawnModeActive)
        {
            TogglePlaneManager(true);
        }
        else
        {
            TogglePlaneManager(false);
        }
    }

    /// <summary>
    /// Checks if the pointer is over any UI object.
    /// </summary>
    private bool IsPointerOverUIObject(Finger finger)
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = finger.screenPosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}

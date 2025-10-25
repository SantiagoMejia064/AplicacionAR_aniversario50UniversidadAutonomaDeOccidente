using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecadeLightManager : MonoBehaviour
{
    [System.Serializable]
    public class DecadeLight
    {
        public string decadeName;   // "1970", "1980", etc.
        public Light lightSource;   // Spot/Area/Point (Realtime)
    }

    [Header("Luces por década")]
    public List<DecadeLight> decadeLights = new();

    [Header("Intensidades")]
    [Range(0f, 5f)] public float focusedIntensity = 3f;   // luz de la década activa
    [Range(0f, 2f)] public float dimmedIntensity = 0.3f;  // luces de décadas no activas

    [Header("Transición")]
    [Range(0.05f, 3f)] public float transitionDuration = 0.75f; // duración del fade
    public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Ambiente (opcional)")]
    public bool fadeAmbient = true;
    [Range(0f, 2f)] public float ambientWhenFocused = 0.7f; // nivel ambiente cuando hay foco
    [Range(0f, 2f)] public float ambientNormal = 1.0f;      // nivel ambiente sin foco

    private Coroutine currentRoutine;
    private string currentDecade = "";

    Dictionary<string, Light> map;

    void Awake()
    {
        map = new Dictionary<string, Light>();
        foreach (var d in decadeLights)
        {
            if (d.lightSource == null) continue;
            if (!map.ContainsKey(d.decadeName))
                map.Add(d.decadeName, d.lightSource);

            // Estado inicial: todo tenue
            d.lightSource.intensity = dimmedIntensity;
        }
        // Ambiente inicial
        if (fadeAmbient) RenderSettings.ambientIntensity = ambientNormal;
    }

    /// <summary>
    /// Llamar desde el trigger de la década.
    /// </summary>
    public void FocusOnDecade(string decadeName)
    {
        if (string.Equals(currentDecade, decadeName)) return;
        currentDecade = decadeName;

        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeTo(decadeName));
    }

    /// <summary>
    /// Restaurar todo a nivel normal (si lo necesitas).
    /// </summary>
    public void ResetLights()
    {
        if (currentRoutine != null) StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(FadeReset());
        currentDecade = "";
    }

    IEnumerator FadeTo(string decadeName)
    {
        // Capturamos intensidades de partida
        var start = new Dictionary<Light, float>();
        foreach (var d in decadeLights)
        {
            if (d.lightSource == null) continue;
            start[d.lightSource] = d.lightSource.intensity;
        }
        float startAmbient = RenderSettings.ambientIntensity;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, transitionDuration);
            float k = curve.Evaluate(Mathf.Clamp01(t));

            foreach (var d in decadeLights)
            {
                if (d.lightSource == null) continue;

                float target = (d.decadeName == decadeName) ? focusedIntensity : dimmedIntensity;
                float from = start[d.lightSource];
                d.lightSource.intensity = Mathf.LerpUnclamped(from, target, k);
            }

            if (fadeAmbient)
            {
                float targetAmb = ambientWhenFocused;
                RenderSettings.ambientIntensity = Mathf.LerpUnclamped(startAmbient, targetAmb, k);
            }

            yield return null;
        }
        currentRoutine = null;
    }

    IEnumerator FadeReset()
    {
        var start = new Dictionary<Light, float>();
        foreach (var d in decadeLights)
        {
            if (d.lightSource == null) continue;
            start[d.lightSource] = d.lightSource.intensity;
        }
        float startAmbient = RenderSettings.ambientIntensity;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / Mathf.Max(0.0001f, transitionDuration);
            float k = curve.Evaluate(Mathf.Clamp01(t));

            foreach (var d in decadeLights)
            {
                if (d.lightSource == null) continue;
                d.lightSource.intensity = Mathf.LerpUnclamped(start[d.lightSource], 1f, k);
            }

            if (fadeAmbient)
            {
                RenderSettings.ambientIntensity = Mathf.LerpUnclamped(startAmbient, ambientNormal, k);
            }

            yield return null;
        }
        currentRoutine = null;
    }
}

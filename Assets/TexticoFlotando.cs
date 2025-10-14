using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TexticoFlotando : MonoBehaviour
{
    public float amplitude = 0.1f;
    public float speed = 1f;

    private Vector3 localStartPos;

    void Start()
    {
        localStartPos = transform.localPosition;
    }

    void Update()
    {
        if (amplitude <= 0f)
        {
            transform.localPosition = localStartPos;
            return;
        }
        float offset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.localPosition = localStartPos + Vector3.up * offset;
    }
}



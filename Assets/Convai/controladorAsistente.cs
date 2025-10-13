using System.Collections;
using System.Collections.Generic;
using Convai.Scripts.Runtime.Features;
using UnityEngine;

public class controladorAsistente : MonoBehaviour
{
    [SerializeField] private ConvaiActionsHandler convaiActionsHandler;

    public void Awake()
    {
        convaiActionsHandler = GetComponent<ConvaiActionsHandler>();
    }

    public void MoveToPosition(GameObject obj)
    {
        StartCoroutine(convaiActionsHandler.MoveTo(obj));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntajeManager : MonoBehaviour
{
    public static int TotalCorrectas { get; private set; }
    public static int TotalRespondidas { get; private set; }

    public static void Reset()
    {
        TotalCorrectas = 0;
        TotalRespondidas = 0;
    }

    public static void ReportAnswer(bool isCorrect)
    {
        TotalRespondidas++;
        if (isCorrect) TotalCorrectas++;
    }
}

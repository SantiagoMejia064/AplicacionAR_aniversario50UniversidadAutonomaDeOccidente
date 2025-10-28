using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class QuizOrden : MonoBehaviour
{
    public int[] correct = {0,1,2,3};     // secuencia esperada
    public List<CardTap> cards;           // arrastra aquí tus 4 botones
    int step = 0;

    public void Select(CardTap card)
    {
        bool ok = (card.id == correct[step]);

        if (ok)
        {
            card.SetColor(Color.green);
            if (card.btn) card.btn.interactable = false;
            step++;
            if (step >= correct.Length) OnSuccess();
        }
        else
        {
            StartCoroutine(FlashWrong(card));
            ResetAll();
        }
    }

    IEnumerator FlashWrong(CardTap card)
    {
        card.SetColor(new Color(1f,0.2f,0.2f));  // rojo
        yield return new WaitForSeconds(0.35f);
        card.ResetVisual();
    }

    void ResetAll()
    {
        step = 0;
        foreach (var c in cards) c.ResetVisual();
    }

    void OnSuccess()
    {
        Debug.Log("✅ Orden correcto");
        // aquí activas tu panel de éxito o pasas de sección
    }
}

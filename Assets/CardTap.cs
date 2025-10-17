using UnityEngine;
using UnityEngine.UI;

public class CardTap : MonoBehaviour
{
    public int id;                 // orden correcto: 0..3
    public QuizOrden manager;      // ← asigna el manager en el Inspector

    [HideInInspector] public Button btn;
    [HideInInspector] public Image img;
    Color baseCol;

    void Awake()
    {
        btn = GetComponent<Button>();
        img = GetComponent<Image>();
        baseCol = img ? img.color : Color.white;
    }

    // Llama a esto desde OnClick() SIN parámetros
    public void Tap()
    {
        manager.Select(this);      // ← pasamos el CardTap, NO un int
    }

    public void SetColor(Color c){ if (img) img.color = c; }
    public void ResetVisual()
    {
        if (img) img.color = baseCol;
        if (btn) btn.interactable = true;
    }
}

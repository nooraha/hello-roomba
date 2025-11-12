using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionIndicator : MonoBehaviour
{
    TMP_Text indicatorText;

    void Awake()
    {
        indicatorText = gameObject.GetComponent<TMP_Text>();
    }

    public void UpdateIndicatorText(string text)
    {
        if (text.Length > 1)
        {
            indicatorText.text = text;
        }
        else
        {
            indicatorText.text = "";
        }
    }
}

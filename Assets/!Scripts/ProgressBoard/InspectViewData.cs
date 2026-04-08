using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public struct InspectViewData
{
    public CanvasGroup cgMain;
    public TextMeshProUGUI[] clues;
    public Image[] tickImages;

    public TextMeshProUGUI message;
    public Button proceed;
}

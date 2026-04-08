using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct QuestionareViewData
{
    public QuestionBank questionBank;
    public CanvasGroup cgMain;
    public CanvasGroup cgReport;
    public TextMeshProUGUI question;
    public Toggle[] answerToggles;
    public TextMeshProUGUI[] answerTexts;
    public Button backBtn, confirmBtn;
    public TextMeshProUGUI correctAnswers;
    public TextMeshProUGUI[] choiceDesc;
    public Image[] choiceCorrectness;
    public Sprite correctChoiceSprite, incorrectChoiceSprite;
    public Button retryBtn;
}

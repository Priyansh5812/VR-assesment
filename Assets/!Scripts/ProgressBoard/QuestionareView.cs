using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QuestionareView : IMonoState
{
    QuestionareViewData data;
    ProgressBoardController controller;
    int[] assesmentChoices;
    int assesmentIndex = 0;
    public bool IsAlreadyTriggered { get; private set; }

    public QuestionareView(ProgressBoardController controller, QuestionareViewData data)
    {
        this.controller = controller;
        this.data = data;
        assesmentChoices = new int[data.questionBank.questionData.Length];
        Array.Fill(assesmentChoices, -1);
    }

    public void OnEnable(Action OnEnableCompleted = null)
    {   
        OnEnableCompleted?.Invoke();
        data.cgMain.alpha = 1.0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = true;
        InitToggleListeners();
        data.backBtn.onClick.AddListener(GoBack);
        data.confirmBtn.onClick.AddListener(ConfirmAnswer);
        data.retryBtn.onClick.AddListener(OnRetry);
        LoadQuestion();
    }

    void InitToggleListeners()
    {
        for (int i = 0; i < data.answerToggles.Length; i++)
        {
            int index = i;

            data.answerToggles[i].onValueChanged.AddListener((bool value) =>
            {
               
                Debug.Log("Index = " + index);

                SelectAnswer(index);
            });
        }
    }


    public void Start(Action OnStartCompleted = null)
    {
        IsAlreadyTriggered = true;
        OnStartCompleted?.Invoke();
    }

    void SelectAnswer(int toggleIndex)
    {
        Debug.Log("ToggleIndex = " + toggleIndex);

        for (int i = 0; i < data.answerToggles.Length; i++)
        {
            data.answerToggles[i].SetIsOnWithoutNotify(i == toggleIndex);
        }

        assesmentChoices[assesmentIndex] = toggleIndex;
    }

    void ConfirmAnswer()
    {
        assesmentIndex++;
        if (assesmentIndex >= assesmentChoices.Length)
        {
            ShowReport();
        }
        else 
        {
            LoadQuestion();
        }
    }

    void ShowReport()
    {
        // Report View Updation 

        int c = 0;
        for (int i = 0; i < data.questionBank.questionData.Length; i++)
        {
            data.choiceDesc[i]?.SetText(data.questionBank.questionData[i].answers[assesmentChoices[i]]);
            
            if (data.questionBank.questionData[i].answerIndex == assesmentChoices[i])
            {
                data.choiceCorrectness[i].sprite = data.correctChoiceSprite;
                c++;
            }
            else
                data.choiceCorrectness[i].sprite = data.incorrectChoiceSprite;
        }

        data.correctAnswers?.SetText($"Correctly Answered : {c}/{assesmentChoices.Length}");
        



        // Displaying Report
        data.cgMain.alpha = 0.0f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = false;
        data.cgReport.alpha = 1.0f;
        data.cgReport.interactable = data.cgReport.blocksRaycasts = true;

        
    }

    void GoBack()
    {
        assesmentIndex--;
        LoadQuestion();
    }

    void LoadQuestion()
    { 
        QuestionData[] qData = data.questionBank.questionData;

        data.question?.SetText(qData[assesmentIndex].question);
        int c = 0;
        foreach (var i in qData[assesmentIndex].answers)
        {
            data.answerToggles[c].SetIsOnWithoutNotify(false);
            data.answerTexts[c]?.SetText(i);
            c++;
        }

        // Load any past assesment if there was any...
        if (assesmentChoices[assesmentIndex] != -1)
        { 
            SelectAnswer(assesmentChoices[assesmentIndex]);
        }

        // Update Buttons for traversal
        data.backBtn.gameObject.SetActive(assesmentIndex > 0);
    }

    void OnRetry()
    {
        data.cgReport.interactable = data.cgReport.blocksRaycasts = false;
        EventManager.OnRetry?.Invoke();
    }

    public void OnDisable(Action OnDisableCompleted = null)
    {
        for (int i = 0; i < data.answerToggles.Length; i++)
        {
            data.answerToggles[i].onValueChanged.RemoveAllListeners();
        }

        data.backBtn.onClick.RemoveListener(GoBack);
        data.confirmBtn.onClick.RemoveListener(ConfirmAnswer);
        data.retryBtn.onClick.AddListener(OnRetry);
        OnDisableCompleted?.Invoke();
    }
}




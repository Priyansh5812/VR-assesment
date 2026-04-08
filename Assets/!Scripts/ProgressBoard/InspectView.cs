using System;
using System.Collections;
using UnityEngine;

public class InspectView : IMonoState
{
    InspectViewData data;
    ProgressBoardController controller;

    int currProgressIndex;
    int maxCluesCount;

    public bool IsAlreadyTriggered
    {
        get; private set;
    }

    public InspectView(ProgressBoardController controller, InspectViewData data)
    { 
       this.controller = controller;
       this.data = data;
    }

    public void OnEnable(Action OnEnableCompleted = null)
    {
        EventManager.OnClueInspected.AddListener(ProcessClueInspected);
        data.proceed.onClick.AddListener(ProceedQuestionaire);
        OnEnableCompleted?.Invoke();
    }

  
    public void Start(Action OnStartCompleted = null)
    {
        IsAlreadyTriggered = true;

        currProgressIndex = 0;
        maxCluesCount = data.clues.Length;
        data.cgMain.alpha = 1f;
        data.cgMain.interactable = data.cgMain.blocksRaycasts = true;

        OnStartCompleted?.Invoke();
    }

    void ProcessClueInspected(string message)
    {
        if (currProgressIndex >= maxCluesCount)
        {
            return;
        }

        data.clues[currProgressIndex]?.SetText(message);
        data.tickImages[currProgressIndex++].gameObject.SetActive(true);
        if (currProgressIndex >= maxCluesCount)
        {
            OnAllCluesInspected();
        }
    }

    void OnAllCluesInspected()
    { 
        data.message.gameObject.SetActive(false);
        data.proceed.gameObject.SetActive(true);
    }

    void ProceedQuestionaire()
    {
        controller?.InitiateStateChange(typeof(QuestionareView));
    }

    public void OnDisable(Action OnDisableCompleted = null)
    {
        EventManager.OnClueInspected.RemoveListener(ProcessClueInspected);
        data.proceed.onClick.RemoveListener(ProceedQuestionaire);
        data.cgMain.interactable = data.cgMain.blocksRaycasts = false;
        data.cgMain.alpha = 0.0f;
        OnDisableCompleted?.Invoke();
    }

}

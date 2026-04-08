using System;
using System.Collections.Generic;
using UnityEngine;



public class ProgressBoardController : MonoBehaviour
{
    public InspectViewData inspectViewData;
    public QuestionareViewData questionareViewData;
    private readonly Dictionary<Type, IMonoState> stateReg = new();
    private bool isChangingState;
    IMonoState currentState;
    public bool IsForTutorial
    {
        get; private set;
    }
    private void Start()
    {
        ConstructMenuStates();
        InitiateStateChange(typeof(InspectView));
    }


    private void ConstructMenuStates()
    {           
        stateReg.Add(typeof(InspectView), new InspectView(this, inspectViewData));
        stateReg.Add(typeof(QuestionareView), new QuestionareView(this, questionareViewData));
    }


    public void InitiateStateChange(Type newStateType)
    {
        if (isChangingState)
        { 
            return;
        }

        IMonoState newState;
        if (stateReg.ContainsKey(newStateType))
            newState = stateReg[newStateType];
        else
        {
            return;
        }

        isChangingState = true;
        if (currentState != null)
            currentState.OnDisable(OnInitialCompleted);
        else
            OnInitialCompleted();


        void OnInitialCompleted()
        {
            currentState = newState;

            if (currentState.IsAlreadyTriggered)
                currentState.OnEnable(OnEnableCompleted);
            else
                currentState.OnEnable(OnEnableThenStart);
        }

        void OnEnableThenStart()
        {
            currentState.Start(OnStartCompleted);
        }

        void OnEnableCompleted()
        {
            isChangingState = false;
        }

        void OnStartCompleted()
        {
            isChangingState = false;
        }
    }



    private void OnDisable()
    {
        currentState?.OnDisable();
    }

}


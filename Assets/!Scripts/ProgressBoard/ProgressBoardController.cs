using System;
using System.Collections.Generic;
using UnityEngine;
// Manages the different UI states (views) for the progress board.
// Uses a simple state registry and IMonoState lifecycle calls to switch views.
public class ProgressBoardController : MonoBehaviour
{
    public InspectViewData inspectViewData;
    public QuestionareViewData questionareViewData;
    // Mapping from a view type to its IMonoState instance
    private readonly Dictionary<Type, IMonoState> stateReg = new();
    // Guard used while a state change is in progress
    private bool isChangingState;
    // Currently active view state
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
        // Create view instances and register them for quick lookup
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
        // Disable the current state first (if any), then enable the new state
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


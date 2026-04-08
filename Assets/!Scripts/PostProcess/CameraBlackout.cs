using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Handles a fullscreen blackout effect by driving a material property.
// Used on startup and when the user retries to smoothly transition the screen.
public class CameraBlackout : MonoBehaviour
{
    [SerializeField] Material material;
    [SerializeField] float blackoutDuration = 1.0f;
    private const string blackoutValue = "_tValue";
    void OnEnable()
    {
        EventManager.OnRetry.AddListener(HandleRetryBlackout);
    }

    void Start()
    {
        InitiateStartup();
    }

    void InitiateStartup()
    {
        material.SetFloat(blackoutValue, 1f);
        StartCoroutine(InitiateBlackout(0f));
    }

    void HandleRetryBlackout()
    {
        StartCoroutine(InitiateBlackout(1f , ReloadScene));

        void ReloadScene()
        {
            SceneManager.LoadScene(0);
        }
    }

    IEnumerator InitiateBlackout(float targetValue , Action onComplete = null)
    {
        float t = 0;
        float currValue = material.GetFloat(blackoutValue);
        while (t < blackoutDuration)
        { 
            t += Time.deltaTime;
            material.SetFloat(blackoutValue, Mathf.Lerp(currValue , targetValue , t));
            yield return null;
        }

        onComplete?.Invoke();
    }

    void OnDisable()
    {
        EventManager.OnRetry.RemoveListener(HandleRetryBlackout);
    }
}

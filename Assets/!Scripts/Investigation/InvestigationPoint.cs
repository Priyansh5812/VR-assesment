using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InvestigationPoint : MonoBehaviour , IInvestigationPoint
{   
    [SerializeField] float maxRadius;
    [SerializeField] float minRadius;
    [SerializeField] Investigator investigator;
    [SerializeField] CanvasGroup cg_main;
    [SerializeField, Range(0f  , 1f )] float dotThreshold = 0.4f;
    [Header("Phase-1")]
    [SerializeField] float loadingDelay = 2.5f;
    [SerializeField] CanvasGroup cg_1;
    [SerializeField] Image loadingImage;
    Coroutine routine;
    [Header("Phase-2")]
    [SerializeField] CanvasGroup cg_2;
    [field: SerializeField] public BoxCollider Collider
    {
        get; private set;
    }

    [field : SerializeField]public InvestigationPointState State
    {
        get; private set;
    }

    [Space(10)]
    [SerializeField] string clueDesc;

    void Start()
    {
        SetAsUnknown();
    }

    void Update()
    {
        cg_main.alpha = Mathf.InverseLerp( maxRadius*maxRadius, minRadius * minRadius, (investigator.transform.position - this.transform.position).sqrMagnitude);
    }

    public void OnFocusEntered()
    {
        if (State == InvestigationPointState.INVESTIGATED)
            return;

        if (Vector3.Dot(this.transform.forward, (investigator.transform.position - this.transform.position).normalized) < dotThreshold)
        {
            return;
        }

       if(routine != null)
            StopCoroutine(routine);
        routine = StartCoroutine(InvestigatingRoutine());
    }

    public void OnFocusExited()
    {
        if (State != InvestigationPointState.INVESTIGATED)
        {
            if (routine != null)
                StopCoroutine(routine);

            SetAsUnknown();
        }
    }

    IEnumerator InvestigatingRoutine()
    {
        float t = 0;
        while (t < loadingDelay)
        { 
            t+= Time.deltaTime;
            loadingImage.fillAmount = Mathf.InverseLerp(0f, loadingDelay, t);
            yield return null;
        }

        loadingImage.fillAmount = 1;
        State = InvestigationPointState.INVESTIGATED;
        StartCoroutine(SetInvestigated());
    }

    IEnumerator SetInvestigated()
    {   
        float t = 1;
        while (t > 0)
        { 
            t -= Time.deltaTime * 2.0f;
            cg_1.alpha = t;
            yield return null;
        }

        t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * 2.0f;
            cg_2.alpha = t;
            yield return null;
        }

        SetAsInvestigated();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position + Collider.center, minRadius);
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(this.transform.position + Collider.center,  maxRadius);
    }

    public void SetAsUnknown()
    {
        cg_2.alpha = 0;
        cg_1.alpha = 1;
        loadingImage.fillAmount = 0;
        State = InvestigationPointState.UNKNOWN;
    }

    public void SetAsInvestigated()
    {
        cg_2.alpha = 1;
        cg_1.alpha = 0;
        State = InvestigationPointState.INVESTIGATED;
        EventManager.OnClueInspected?.Invoke(clueDesc);
    }
}

public interface IInvestigationPoint
{
    public BoxCollider Collider { get; }
    public InvestigationPointState State { get; }
    public void OnFocusEntered();
    public void OnFocusExited();
    public void SetAsUnknown();
    public void SetAsInvestigated();
}

public enum InvestigationPointState
{ 
   UNKNOWN,
   INVESTIGATED
}
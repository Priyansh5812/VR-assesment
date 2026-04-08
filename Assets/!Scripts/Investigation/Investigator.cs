using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Investigator : MonoBehaviour
{
    [SerializeField] LayerMask InvestigationPointMask;
    [SerializeField] float focusDistance = 10.0f;
    IInvestigationPoint currInvestigationPoint;
    RaycastHit[] reserve = new RaycastHit[1];
    private void Update()
    {
        HandleDetection();    
    }

    void HandleDetection()
    {
        IInvestigationPoint investigationPoint = null;
        if (Physics.RaycastNonAlloc(
            this.transform.position,
            this.transform.forward,
            reserve,
            focusDistance,
            InvestigationPointMask
            ) > 0)
        {
            RaycastHit hit = reserve[0];
            hit.collider.gameObject.TryGetComponent<IInvestigationPoint>(out investigationPoint);
        }

        if (investigationPoint == currInvestigationPoint)
            return;

        if (investigationPoint == null)
        {
            currInvestigationPoint?.OnFocusExited();
        }
        else
        {
            investigationPoint?.OnFocusEntered();
        }

        currInvestigationPoint = investigationPoint;
    }
}

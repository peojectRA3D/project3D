using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHidingCamera : MonoBehaviour
{
    private Transform target = null;

    [SerializeField]
    private float sphereCastRadius = 1f;

    private RaycastHit[] hitBuffer = new RaycastHit[32];

    private List<OpacityObject> hiddenObjects = new List<OpacityObject>();
    private List<OpacityObject> previouslyHiddenObjects = new List<OpacityObject>();

    private GameObject tPlayer;

    private void LateUpdate()
    {
        RefreshHiddenObjects();
    }

    public void RefreshHiddenObjects()
    {
        if (tPlayer == null)
        {
            tPlayer = GameObject.FindWithTag("Player");
            if (tPlayer != null)
            {
                target = tPlayer.transform;
            }
        }

        Vector3 toTarget = (target.position - transform.position);
        float targetDistance = toTarget.magnitude;
        Vector3 targetDirection = toTarget / targetDistance;

        targetDistance -= sphereCastRadius * 1.1f;

        hiddenObjects.Clear();

        int hitCount = Physics.SphereCastNonAlloc(transform.position,
                                                    sphereCastRadius,
                                                    targetDirection,
                                                    hitBuffer,
                                                    targetDistance,
                                                    -1,
                                                    QueryTriggerInteraction.Ignore);

        for (int i = 0; i < hitCount; i++)
        {
            var hit = hitBuffer[i];
            var hideable = OpacityObject.GetRootHideByCollider(hit.collider);

            if (hideable != null)
                hiddenObjects.Add(hideable);
        }

        foreach (var hideable in hiddenObjects)
        {
            if (!previouslyHiddenObjects.Contains(hideable))
                hideable.SetVisible(false);
        }

        foreach (var hideable in previouslyHiddenObjects)
        {
            if (!hiddenObjects.Contains(hideable))
                hideable.SetVisible(true);
        }

        var temp = hiddenObjects;
        hiddenObjects = previouslyHiddenObjects;
        previouslyHiddenObjects = temp;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OpacityObject : MonoBehaviour
{
    OpacityObject opacityObject;
    private static Dictionary<Collider, OpacityObject> opacityObjectsMap = new Dictionary<Collider, OpacityObject>();

    [SerializeField]
    public GameObject Renderers;
    public Collider Collider = null;

    void Start()
    {
        InitOpacityObject();
    }
    
    void InitOpacityObject()
    {
        foreach(var obj in opacityObjectsMap.Values)
        {
            if(obj !=null && obj.Collider != null)
            {
                obj.SetVisible(true);
                obj.opacityObject = null;
            }
        }

        opacityObjectsMap.Clear();

        foreach (var obj in FindObjectsOfType<OpacityObject>())
        {
            if(obj.Collider != null)
            {
                opacityObjectsMap[obj.Collider] = obj;
            }
        }
    }

    public static OpacityObject GetRootHideByCollider(Collider collider)
    {
        OpacityObject obj;

        if (opacityObjectsMap.TryGetValue(collider, out obj))
            return GetRoot(obj);
        else
            return null;
    }

    private static OpacityObject GetRoot(OpacityObject obj)
    {
        if (obj.opacityObject == null)
        {
            return obj;
        }
        else
            return GetRoot(obj.opacityObject);
    }

    public void SetVisible(bool visible)
    {
        Renderer rend = Renderers.GetComponent<Renderer>();

        if (rend != null && rend.gameObject.activeInHierarchy && opacityObjectsMap.ContainsKey(rend.GetComponent<Collider>()))
        {
            rend.shadowCastingMode = visible ? ShadowCastingMode.On : ShadowCastingMode.ShadowsOnly;
        }
    }
}

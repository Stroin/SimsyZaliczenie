using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartObject : MonoBehaviour
{
    [SerializeField] protected string _DisplayName;
    [SerializeField] protected Transform _InteractionMarker;

    protected List<BaseInteraction> CachedInteraction = null;
    
    public Vector3 InteractionPoint => _InteractionMarker != null ? _InteractionMarker.position : transform.position;

    public string DisplayName => _DisplayName;
    public List<BaseInteraction> Interactions
    {
        get
        {
            if (CachedInteraction == null)
                CachedInteraction = new List<BaseInteraction>(GetComponents<BaseInteraction>());

                return CachedInteraction;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        SmartObjectManager.Instance.RegisterSmartObject(this);
    }

    private void OnDestroy() 
    {
        SmartObjectManager.Instance.DeregisterSmartObject(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
